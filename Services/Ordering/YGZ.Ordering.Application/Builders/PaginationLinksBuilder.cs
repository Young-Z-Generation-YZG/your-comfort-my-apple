
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Ordering.Application.Builders;

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
}
