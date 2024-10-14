export class ResponseDto {
    data: any;
    isSuccess: boolean;
    error: string;
    constructor(data: any, isSuccess: boolean, error: string) {
        this.data = data;
        this.isSuccess = isSuccess;
        this.error = error;
    }

}