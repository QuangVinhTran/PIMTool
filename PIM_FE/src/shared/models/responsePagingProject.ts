import { Project } from "./project";

export class ResponsePagingProject {
    totalPage?: number;
    data : Project[] = [];
    setTotalPage(total : number) {
        this.totalPage = total;
    }
    // constructor(totalPage : number, data : Project[]) {
    //     this.totalPage = totalPage;
    //     this.data = data;
    // }
}