
using System.Reflection;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.BuildingBlocks.Shared.Utils;

public static class PaginationLinksBuilder
{
    /// <summary>
    /// Builds pagination links for first, previous, next, and last pages.
    /// </summary>
    /// <returns>A PaginationLinks object with navigation URLs</returns>
    public static PaginationLinks Build(
        string basePath,
        Dictionary<string, string> queryParams,
        int currentPage,
        int totalPages)
    {
        // Handle case where there are no pages
        if (totalPages <= 0)
        {
            return new PaginationLinks
            (
                First: null,
                Prev: null,
                Next: null,
                Last: null
            );
        }

        // Helper method to build URL for a specific page
        string BuildUrl(int page)
        {
            var paramsCopy = new Dictionary<string, string>(queryParams);

            paramsCopy["_page"] = page.ToString();

            var queryString = string.Join("&",

                paramsCopy.Select(kvp =>
                {
                    var key = kvp.Key;
                    var value = kvp.Value;

                    return $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}";
                }));

            return $"{basePath}?{queryString}";
        }

        // Construct the pagination links
        return new PaginationLinks(
            First: BuildUrl(1),
            Prev: currentPage > 1 ? BuildUrl(currentPage - 1) : null,
            Next: currentPage < totalPages ? BuildUrl(currentPage + 1) : null,
            Last: BuildUrl(totalPages)
        );
    }

    /// <summary>
    /// Builds pagination links using reflection to extract query parameters from the request object.
    /// Supports properties with underscore prefix (e.g., _page, _limit, _colors, etc.)
    /// </summary>
    /// <typeparam name="TRequest">The request type containing pagination and filter properties</typeparam>
    /// <returns>A PaginationLinks object with navigation URLs</returns>
    public static PaginationLinks Build<TRequest>(
        string basePath,
        TRequest request,
        int? currentPage,
        int totalPages) where TRequest : class
    {
        var queryParams = BuildQueryParamsFromRequest(request);

        return Build(basePath, queryParams, currentPage ?? 1, totalPages);
    }

    private static Dictionary<string, string> BuildQueryParamsFromRequest<TRequest>(TRequest request) where TRequest : class
    {
        var queryParams = new Dictionary<string, string>();
        var properties = typeof(TRequest).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var value = property.GetValue(request);

            if (value is null)
                continue;

            var key = "_" + char.ToLower(property.Name[1]) + property.Name.Substring(2);

            // Handle List<string> properties
            if (value is List<string> list && list.Any())
            {
                queryParams[key] = string.Join(",", list);
            }
            // Handle nullable int properties
            else if (value is int?)
            {
                var intValue = (int?)value;
                if (intValue.HasValue)
                {
                    queryParams[key] = intValue.Value.ToString();
                }
            }
        }

        return queryParams;
    }
}
