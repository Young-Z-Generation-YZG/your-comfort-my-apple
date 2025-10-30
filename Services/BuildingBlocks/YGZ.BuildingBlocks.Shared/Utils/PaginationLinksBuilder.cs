
using System.Reflection;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.BuildingBlocks.Shared.Utils;

public static class PaginationLinksBuilder
{
    /// <summary>
    /// Builds pagination links for first, previous, next, and last pages.
    /// Legacy overload that accepts Dictionary&lt;string, string&gt; for backward compatibility.
    /// </summary>
    /// <returns>A PaginationLinks object with navigation URLs</returns>
    public static PaginationLinks Build(
        string basePath,
        Dictionary<string, string> queryParams,
        int currentPage,
        int totalPages)
    {
        // Convert to Dictionary<string, List<string>> format
        var multiValueParams = new Dictionary<string, List<string>>();
        foreach (var kvp in queryParams)
        {
            multiValueParams[kvp.Key] = new List<string> { kvp.Value };
        }

        return Build(basePath, multiValueParams, currentPage, totalPages);
    }

    /// <summary>
    /// Builds pagination links for first, previous, next, and last pages.
    /// Main overload that supports multiple values for the same key.
    /// </summary>
    /// <returns>A PaginationLinks object with navigation URLs</returns>
    public static PaginationLinks Build(
        string basePath,
        Dictionary<string, List<string>> queryParams,
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
            var paramsCopy = new Dictionary<string, List<string>>();

            // Copy all query params
            foreach (var kvp in queryParams)
            {
                paramsCopy[kvp.Key] = new List<string>(kvp.Value);
            }

            // Override page number
            paramsCopy["_page"] = new List<string> { page.ToString() };

            var queryParts = new List<string>();
            foreach (var kvp in paramsCopy)
            {
                foreach (var value in kvp.Value)
                {
                    queryParts.Add($"{kvp.Key}={Uri.EscapeDataString(value)}");
                }
            }

            var queryString = string.Join("&", queryParts);

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

    private static Dictionary<string, List<string>> BuildQueryParamsFromRequest<TRequest>(TRequest request) where TRequest : class
    {
        var queryParams = new Dictionary<string, List<string>>();
        var properties = typeof(TRequest).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var value = property.GetValue(request);

            if (value is null)
                continue;

            // If property name already starts with underscore, use it as is
            // Otherwise, add underscore prefix
            var key = property.Name.StartsWith("_") 
                ? property.Name 
                : "_" + char.ToLower(property.Name[0]) + property.Name.Substring(1);

            // Handle List<string> properties - each item becomes a separate query parameter
            if (value is List<string> list && list.Any())
            {
                queryParams[key] = list;
            }
            // Handle nullable int properties
            else if (value is int?)
            {
                var intValue = (int?)value;
                if (intValue.HasValue)
                {
                    queryParams[key] = new List<string> { intValue.Value.ToString() };
                }
            }
            // Handle string properties
            else if (value is string stringValue && !string.IsNullOrWhiteSpace(stringValue))
            {
                queryParams[key] = new List<string> { stringValue };
            }
        }

        return queryParams;
    }
}
