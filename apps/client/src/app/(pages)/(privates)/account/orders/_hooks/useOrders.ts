import { useMemo, useState } from 'react';
import { useGetOrdersQuery } from '~/infrastructure/services/order.service';
import { OrderResponse } from '~/domain/interfaces/orders/order.interface';

type OrderStatus =
   | 'all'
   | 'PENDING_ASSIGNMENT'
   | 'PROCESSING'
   | 'SHIPPED'
   | 'DELIVERED'
   | 'CANCELED';
type SortOption = 'date-desc' | 'date-asc' | 'total-desc' | 'total-asc';

export const useOrders = () => {
   const [searchQuery, setSearchQuery] = useState('');
   const [statusFilter, setStatusFilter] = useState<OrderStatus>('all');
   const [sortBy, setSortBy] = useState<SortOption>('date-desc');
   const [currentPage, setCurrentPage] = useState(1);

   const {
      data: orderDataAsync,
      isLoading,
      isFetching,
      isError,
      error,
      isSuccess,
   } = useGetOrdersQuery();

   // Transform API data
   const orders = useMemo(() => {
      if (!orderDataAsync?.items) return [];
      return orderDataAsync.items;
   }, [orderDataAsync]);

   // Filter orders
   const filteredOrders = useMemo(() => {
      return orders.filter((order) => {
         const matchesSearch = order.order_code
            .toLowerCase()
            .includes(searchQuery.toLowerCase());
         const matchesStatus =
            statusFilter === 'all' || order.order_status === statusFilter;
         return matchesSearch && matchesStatus;
      });
   }, [orders, searchQuery, statusFilter]);

   // Sort orders
   const sortedOrders = useMemo(() => {
      return [...filteredOrders].sort((a, b) => {
         switch (sortBy) {
            case 'date-asc':
               return (
                  new Date(a.order_created_at).getTime() -
                  new Date(b.order_created_at).getTime()
               );
            case 'date-desc':
               return (
                  new Date(b.order_created_at).getTime() -
                  new Date(a.order_created_at).getTime()
               );
            case 'total-asc':
               return a.order_total_amount - b.order_total_amount;
            case 'total-desc':
               return b.order_total_amount - a.order_total_amount;
            default:
               return (
                  new Date(b.order_created_at).getTime() -
                  new Date(a.order_created_at).getTime()
               );
         }
      });
   }, [filteredOrders, sortBy]);

   // Pagination
   const ordersPerPage = 10;
   const totalPages = Math.ceil(sortedOrders.length / ordersPerPage);
   const paginatedOrders = useMemo(() => {
      const indexOfLastOrder = currentPage * ordersPerPage;
      const indexOfFirstOrder = indexOfLastOrder - ordersPerPage;
      return sortedOrders.slice(indexOfFirstOrder, indexOfLastOrder);
   }, [sortedOrders, currentPage]);

   const handleSearch = (query: string) => {
      setSearchQuery(query);
      setCurrentPage(1); // Reset to first page on search
   };

   const handleStatusFilter = (status: OrderStatus) => {
      setStatusFilter(status);
      setCurrentPage(1); // Reset to first page on filter
   };

   const handleSort = (sort: SortOption) => {
      setSortBy(sort);
   };

   const handlePageChange = (page: number) => {
      setCurrentPage(page);
   };

   const clearFilters = () => {
      setSearchQuery('');
      setStatusFilter('all');
      setCurrentPage(1);
   };

   return {
      // Data
      orders: paginatedOrders,
      totalOrders: sortedOrders.length,

      // Pagination
      currentPage,
      totalPages,
      ordersPerPage,

      // Filters
      searchQuery,
      statusFilter,
      sortBy,

      // Loading states
      isLoading,
      isFetching,
      isError,
      error,

      // Actions
      handleSearch,
      handleStatusFilter,
      handleSort,
      handlePageChange,
      clearFilters,
   };
};
