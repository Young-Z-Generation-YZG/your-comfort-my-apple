

using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.Discount.Application.Data;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly DiscountDbContext _context;
    private readonly ILogger<DiscountRepository> _logger;

    public DiscountRepository(DiscountDbContext context, ILogger<DiscountRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Coupon> GetByCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code cannot be null or empty.", nameof(code));

        try
        {
            var result = await _context.Coupons
                .FirstOrDefaultAsync(c => c.Id == Code.Create(code) && !c.IsDeleted);

            if(result is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
            }

            return result!;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<(List<Coupon> coupons, int TotalCount, int TotalPages)> GetAllAsync(int _page, int _limit, DiscountStateEnum? state = null)
    {
        if (_page < 1) throw new ArgumentException("Page must be greater than 0.", nameof(_page));
        if (_limit < 1) throw new ArgumentException("Limit must be greater than 0.", nameof(_limit));

        var query = _context.Coupons
            .Where(c => !c.IsDeleted);

        if (state is not null)
        {
            query = query.Where(c => c.State == state);
        }

        try
        {
            int totalCount = await query.CountAsync();

            var coupons = await query
                .OrderBy(c => c.CreatedAt) // Example ordering; adjust as needed
                .Skip((_page - 1) * _limit)
                .Take(_limit)
                .ToListAsync();

            int totalPages = (int)Math.Ceiling((double)totalCount / _limit);

            return (coupons, totalCount, totalPages);
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public async Task<bool> CreateAsync(Coupon coupon)
    {
        try
        {
            _context.Coupons.Add(coupon);

            int affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public async Task<bool> DeleteAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code cannot be null or empty.", nameof(code));

        try
        {
            var coupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.Id == Code.Create(code));

            if (coupon is null)
                return false; // Coupon not found or already deleted

            // Soft delete
            coupon.IsDeleted = true;
            coupon.DeletedAt = DateTime.UtcNow;

            // Optionally set DeletedByUserId if you have user context, e.g.:
            // coupon.DeletedByUserId = currentUserId;

            int affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public async Task<bool> UpdateAsync(string code, Coupon coupon)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code cannot be null or empty.", nameof(code));

        try
        {
            var existingCoupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.Id == Code.Create(code) && !c.IsDeleted);

            if (existingCoupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
            }

            // Update properties
            existingCoupon.Title = coupon.Title;
            existingCoupon.Description = coupon.Description;
            existingCoupon.Type = coupon.Type;
            existingCoupon.State = coupon.State;
            existingCoupon.DiscountValue = coupon.DiscountValue;
            existingCoupon.MinPurchaseAmount = coupon.MinPurchaseAmount;
            existingCoupon.MaxDiscountAmount = coupon.MaxDiscountAmount;
            existingCoupon.ValidFrom = coupon.ValidFrom;
            existingCoupon.ValidTo = coupon.ValidTo;
            existingCoupon.AvailableQuantity = coupon.AvailableQuantity;
            existingCoupon.UpdatedAt = DateTime.UtcNow; // Update audit field

            int affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
}
