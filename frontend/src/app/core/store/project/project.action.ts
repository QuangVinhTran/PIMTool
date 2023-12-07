import {createAction, props} from "@ngrx/store";
import {Project} from "../../models/project/project.models";

export enum ProjectActionTypes {
  SET_PROJECTS = 'Set project'
}

export const setProjects = createAction(
  ProjectActionTypes.SET_PROJECTS,
  props<{ projects: Project[] }>()
)
