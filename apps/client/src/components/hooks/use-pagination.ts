import { PaginationResponse } from '~/domain/interfaces/common/pagination-response.interface';

const usePagination = (paginationData: PaginationResponse<any>) => {
   const isLastPage =
      paginationData.current_page === paginationData.total_pages;
   const isFirstPage = paginationData.current_page === 1;
   const isNextPage = paginationData.current_page < paginationData.total_pages;
   const isPrevPage = paginationData.current_page > 1;

   const getPageNumbers = () => {
      const pageNumbers = new Set();
      for (let i = 1; i <= paginationData.total_pages; i++) {
         if (i === paginationData.current_page) {
            pageNumbers.add(i);
         } else {
            pageNumbers.add('...');
         }

         if (i === paginationData.current_page + 1) {
            pageNumbers.add(i);
         }

         if (i === paginationData.current_page - 1) {
            pageNumbers.add(i);
         }
      }
      return Array.from(pageNumbers);
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
