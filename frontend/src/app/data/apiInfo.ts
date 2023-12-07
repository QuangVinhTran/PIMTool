export const BASE_URL = "http://localhost:20000"

export enum EndPoints {
  PROJECTS = "projects",
  CREATE_PROJECT = "projects/create",
  UPDATE_PROJECT = "projects/update",
  SEARCH_PROJECTS = "projects/search",
  DELETE_PROJECT = "projects/delete",
  IMPORT_PROJECTS = "projects/import-from-file",
  EXPORT_PROJECTS = "projects/export-to-excel",
  VALIDATE_PROJECT_NUMBER = "projects/validate",
  LOGIN = "auth/login",
  VALIDATE_TOKEN = "auth/validate-token",
  REFRESH_TOKEN = "auth/refresh",
  GROUPS = "groups",
  GROUPS_ALL = "groups/all",
  EMPLOYEES_ALL = "employees/all",
}
export default BASE_URL
