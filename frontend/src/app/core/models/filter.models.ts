export interface SearchInfo {
  fieldName: string,
  value: string,
}

export interface SortInfo {
  fieldName: string,
  ascending: boolean
}

export interface SearchCriteria {
  ConjunctionSearchInfos: SearchInfo[],
  DisjunctionSearchInfos: SearchInfo[]
}

export interface PaginationStatus {
  pageIndex: number,
  pageSize: number,
  lastPage: number,
  isLastPage: boolean
}

export interface AdvancedFilter {
  leaderName: string,
  memberName: string,
  startDateRange: DateRange,
  endDateRange: DateRange
}

export interface DateRange {
  from: string,
  to: string
}
