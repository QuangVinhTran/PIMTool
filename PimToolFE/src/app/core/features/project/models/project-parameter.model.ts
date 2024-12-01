import {PagingParameters} from "./paging-parameter.model";
import {SortingParameters} from "./sorting-parameter.model";

export interface ProjectParameters {
  pagingParameters: PagingParameters;
  filterParameters?: string;
  status?: string;
  sortingParameters?: SortingParameters;
}
