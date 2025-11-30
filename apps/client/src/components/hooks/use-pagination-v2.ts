import { useCallback } from 'react';
import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';

const MAX_VISIBLE_ITEM_LEFT = 2; // Number of pages to show on the LEFT side of "..."
const MAX_VISIBLE_ITEM_RIGHT = 2; // Number of pages to show on the RIGHT side of "..."

type UsePaginationV2Options = {
   pageSizeOverride?: number | null;
   currentPageOverride?: number | null;
   fallbackPageSize?: number | null;
};

const DEFAULT_FALLBACK_PAGE_SIZE = 10;

const usePaginationV2 = (
   paginationData: PaginationResponse<any>,
   options?: UsePaginationV2Options,
) => {
   const getPaginationItems = useCallback(() => {
      const items: Array<{
         type: 'nav' | 'page' | 'ellipsis';
         label: string;
         value: number | null;
         disabled?: boolean;
      }> = [];

      const totalPages = paginationData.total_pages || 1;
      const currentPage = paginationData.current_page || 1;

      if (totalPages === 0) return items;

      const isFirstPage = currentPage === 1;
      const isLastPage = currentPage === totalPages;

      // First page button
      items.push({
         type: 'nav',
         label: '<<',
         value: 1,
         disabled: isFirstPage,
      });

      // Previous page button
      items.push({
         type: 'nav',
         label: '<',
         value: currentPage > 1 ? currentPage - 1 : null,
         disabled: isFirstPage,
      });

      // Page numbers - custom logic to group left cluster, current cluster, and trailing cluster
      const addedPages = new Set<number>();
      const frontPages: number[] = [];
      const pushFrontPage = (page: number) => {
         if (page < 1 || page > totalPages) return;
         if (page >= currentPage) return;
         if (frontPages.length >= MAX_VISIBLE_ITEM_LEFT) return;
         if (addedPages.has(page)) return;
         frontPages.push(page);
         addedPages.add(page);
      };

      const rawSegmentIndex =
         Math.floor((currentPage - 1) / MAX_VISIBLE_ITEM_LEFT) - 1;
      const segmentIndex = Math.max(0, rawSegmentIndex);
      const leftStartPage = segmentIndex * MAX_VISIBLE_ITEM_LEFT + 1;

      for (let page = leftStartPage; page < currentPage; page++) {
         pushFrontPage(page);
      }

      frontPages.forEach((page) => {
         items.push({
            type: 'page',
            label: `${page}`,
            value: page,
         });
      });

      const centerEndPage = Math.min(
         totalPages,
         currentPage + MAX_VISIBLE_ITEM_LEFT,
      );
      for (let page = currentPage; page <= centerEndPage; page++) {
         if (addedPages.has(page)) continue;
         items.push({
            type: 'page',
            label: `${page}`,
            value: page,
         });
         addedPages.add(page);
      }

      const lastFrontPage =
         addedPages.size > 0 ? Math.max(...Array.from(addedPages)) : 0;

      // Show last MAX_VISIBLE_ITEM_RIGHT + 1 pages (including last page, so if RIGHT=2, show 33,34,35 for totalPages=35)
      const rightStartPage = Math.max(1, totalPages - MAX_VISIBLE_ITEM_RIGHT);
      if (rightStartPage > lastFrontPage + 1) {
         items.push({
            type: 'ellipsis',
            label: '...',
            value: null,
         });
      }
      for (let page = rightStartPage; page <= totalPages; page++) {
         if (addedPages.has(page)) continue;
         items.push({
            type: 'page',
            label: `${page}`,
            value: page,
         });
         addedPages.add(page);
      }

      // Next page button
      items.push({
         type: 'nav',
         label: '>',
         value: currentPage < totalPages ? currentPage + 1 : null,
         disabled: isLastPage,
      });

      // Last page button
      items.push({
         type: 'nav',
         label: '>>',
         value: totalPages,
         disabled: isLastPage,
      });

      return items;
   }, [paginationData]);

   const fallbackPageSize =
      options?.fallbackPageSize ?? DEFAULT_FALLBACK_PAGE_SIZE;

   const resolvedPageSize =
      options?.pageSizeOverride ?? paginationData.page_size ?? fallbackPageSize;

   const resolvedCurrentPage =
      options?.currentPageOverride ?? paginationData.current_page ?? 1;

   const resolvedTotalRecords = paginationData.total_records ?? 0;

   const safePageSize =
      resolvedPageSize > 0 ? resolvedPageSize : fallbackPageSize;
   const safeCurrentPage = Math.max(1, resolvedCurrentPage);

   const hasRecords = resolvedTotalRecords > 0 && safePageSize > 0;
   const firstItemIndex = hasRecords
      ? (safeCurrentPage - 1) * safePageSize + 1
      : 0;
   const lastItemIndex = hasRecords
      ? Math.min(safeCurrentPage * safePageSize, resolvedTotalRecords)
      : 0;

   return {
      currentPage: safeCurrentPage,
      totalPages: paginationData.total_pages,
      pageSize: safePageSize,
      totalRecords: resolvedTotalRecords,
      firstItemIndex,
      lastItemIndex,
      limitSelectValue: String(safePageSize),
      getPaginationItems,
   };
};

export default usePaginationV2;
