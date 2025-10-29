import { PaginationResponse } from '~/src/domain/interfaces/common/pagination-response.interface';

const usePagination = (paginationData: PaginationResponse<any>) => {
   const isLastPage =
      paginationData.current_page === paginationData.total_pages;
   const isFirstPage = paginationData.current_page === 1;
   const isNextPage = paginationData.current_page < paginationData.total_pages;
   const isPrevPage = paginationData.current_page > 1;

   const getPageNumbers = () => {
      const totalPages = paginationData.total_pages;

      if (totalPages <= 3) {
         // Show all pages: 1 2 3
         return Array.from({ length: totalPages }, (_, i) => i + 1);
      } else {
         // Show: 1 2 ... lastPage
         return [1, 2, '...', totalPages];
      }
   };

   return {
      totalRecords: paginationData.total_records,
      totalPages: paginationData.total_pages,
      currentPage: paginationData.current_page,
      pageSize: paginationData.page_size,
      paginationItems: paginationData.items,

      // helpers
      isLastPage,
      isFirstPage,
      isNextPage,
      isPrevPage,

      getPageNumbers,
   };
};

export default usePagination;
