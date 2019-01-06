export class OperationResult<T> {
  isSuccess?: boolean;
  code?: string;
  message?: string;
  date?: Date;
  data?: T;
  constructor(isSuccess: boolean) {
    this.isSuccess = isSuccess;
  }
}

export class PaginatedResult<T> {
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  totalPageCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  list: Array<T>;
  constructor() {
    this.list = [];
  }
}
