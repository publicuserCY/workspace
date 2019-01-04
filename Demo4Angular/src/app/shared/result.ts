export interface OperationResult<T> {
  isSuccess: boolean;
  code?: string;
  message?: string;
  date?: Date;
  data?: T;
}

export interface PaginatedList {
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  totalPageCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
