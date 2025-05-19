export interface PaginationResponse<T> {
   total_records: number;
   total_pages: number;
   page_size: number;
   current_page: number;
   items: T[];
   links: PaginationLinks;
}

export interface PaginationLinks {
   first?: string | null;
   prev?: string | null;
   next?: string | null;
   last?: string | null;
}
