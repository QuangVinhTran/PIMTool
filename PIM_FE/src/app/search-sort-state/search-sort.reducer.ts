import { ActionReducer, INIT, UPDATE, createReducer, on } from "@ngrx/store";
import { SearchSortModel } from "./search-sort.model";
import { clearSearchSort, addSearchSort, removeSearchSort } from "./search-sort.action";

export const initialSearchSort : SearchSortModel[] = [];

export const searchSortReducer = createReducer (
    initialSearchSort,

    on(clearSearchSort, _ => []),

    on(addSearchSort, (entries, searchSort) => {
        const entriesClone : SearchSortModel[] = JSON.parse(JSON.stringify(entries));
        const isSort = searchSort.name.includes('sort');
        if(isSort) {
            const found = entriesClone.find(e => e.name.includes('sort'));
            if(found) {
                entriesClone.slice(entriesClone.indexOf(found), 1);
            }
        }
        const found = entriesClone.find(e => e.name == searchSort.name);
        if(found) {
            const index = entriesClone.indexOf(found);
            entriesClone[index].value = searchSort.value;
        } else {
            entriesClone.push(searchSort);
        }
        return entriesClone;
    }),
    
    on(removeSearchSort, (entries, searchSort) => {
        const entriesClone : SearchSortModel[] = JSON.parse(JSON.stringify(entries));
        const found = entriesClone.find(e => e.name == searchSort.name);
        if(found) {
            entriesClone.slice(entriesClone.indexOf(found), 1)
        }
        return entriesClone;
    })
)

export const metaReducerLocalStorage = (reducer : ActionReducer<any>) : ActionReducer<any> => {
    return (state, action)  => {
        if(state.type === INIT || action.type === UPDATE) {
            const storageValue = localStorage.getItem('searchSortState');
            if(storageValue) {
                try {
                    return JSON.parse(storageValue);
                } catch {
                    localStorage.removeItem('searchSortState');
                }
            }
        } 
        const nextState = reducer(state, action);
        localStorage.setItem('searchSortState', JSON.stringify(nextState));
        return nextState;
    }
}