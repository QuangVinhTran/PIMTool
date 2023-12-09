export interface PageList {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  items: any[];
  hasPrevious: boolean;
  hasNext: boolean;
}
