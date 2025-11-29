using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Domain.Notifications;
using YGZ.Ordering.Domain.Notifications.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);

        builder
            .Property(n => n.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => NotificationId.Of(value));

        builder
            .Property(n => n.SenderId)
            .HasConversion(
#pragma warning disable CS8602, CS8604 // Null reference warnings - handled by null check
                id => id == null ? (Guid?)null : id.Value,
#pragma warning restore CS8602, CS8604
                value => value == null ? null! : UserId.Of(value.Value.ToString()))
            .IsRequired(false);

        builder
            .Property(n => n.ReceiverId)
            .HasConversion(
#pragma warning disable CS8602, CS8604 // Null reference warnings - handled by null check
                id => id == null ? (Guid?)null : id.Value,
#pragma warning restore CS8602, CS8604
                value => value == null ? null! : UserId.Of(value.Value.ToString()))
            .IsRequired(false);

        builder
            .Property(n => n.Title)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(n => n.Content)
            .HasMaxLength(2000)
            .IsRequired();

        builder
            .Property(n => n.Type)
            .HasConversion(
                type => type.Name,
                name => EOrderNotificationType.FromName(name, false))
            .HasColumnName("Type")
            .IsRequired();

        builder
            .Property(n => n.Status)
            .HasConversion(
                status => status.Name,
                name => EOrderNotificationStatus.FromName(name, false))
            .HasColumnName("Status")
            .IsRequired();

        builder
            .Property(n => n.IsRead)
            .HasDefaultValue(false)
            .IsRequired();

        builder
            .Property(n => n.IsSystem)
            .HasDefaultValue(true)
            .IsRequired();

        builder
            .Property(n => n.Link)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder
            .Property(n => n.CreatedAt)
            .IsRequired();

        builder
            .Property(n => n.UpdatedAt)
            .IsRequired();

        builder
            .Property(n => n.UpdatedBy)
            .HasMaxLength(450)
            .IsRequired(false);

        builder
            .Property(n => n.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder
            .Property(n => n.DeletedAt)
            .IsRequired(false);

        builder
            .Property(n => n.DeletedBy)
            .HasMaxLength(450)
            .IsRequired(false);

        // Indexes for query performance
        builder.HasIndex(n => n.ReceiverId);
        builder.HasIndex(n => n.SenderId);
        builder.HasIndex(n => n.Type);
        builder.HasIndex(n => n.Status);
        builder.HasIndex(n => n.IsRead);
        builder.HasIndex(n => n.IsDeleted);
        builder.HasIndex(n => n.CreatedAt);
    }
}
