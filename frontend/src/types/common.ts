export interface PagedResult<T> {
  items: T[];
  currentPage: number;
  pageSize: number;
  totalCount: number;
  itemsCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}
