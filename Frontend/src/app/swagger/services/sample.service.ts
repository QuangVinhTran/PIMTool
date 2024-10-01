/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { Observable } from 'rxjs';
import { map as __map, filter as __filter } from 'rxjs/operators';

import { SampleDto } from '../models/sample-dto';
@Injectable({
  providedIn: 'root',
})
class SampleService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @return OK
   */
  SampleGetResponse(): Observable<StrictHttpResponse<Array<SampleDto>>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      'GET',
      this.rootUrl + `/api/Sample`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as StrictHttpResponse<Array<SampleDto>>;
      })
    );
  }
  /**
   * @return OK
   */
  SampleGet(): Observable<Array<SampleDto>> {
    return this.SampleGetResponse().pipe(
      __map(_r => _r.body)
    );
  }

  /**
   * @param sample undefined
   * @return OK
   */
  SamplePutResponse(sample: SampleDto): Observable<StrictHttpResponse<SampleDto>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = sample;
    let req = new HttpRequest<any>(
      'PUT',
      this.rootUrl + `/api/Sample`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as StrictHttpResponse<SampleDto>;
      })
    );
  }
  /**
   * @param sample undefined
   * @return OK
   */
  SamplePut(sample: SampleDto): Observable<SampleDto> {
    return this.SamplePutResponse(sample).pipe(
      __map(_r => _r.body)
    );
  }

  /**
   * @param sample undefined
   * @return OK
   */
  SamplePostResponse(sample: SampleDto): Observable<StrictHttpResponse<SampleDto>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = sample;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/Sample`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as StrictHttpResponse<SampleDto>;
      })
    );
  }
  /**
   * @param sample undefined
   * @return OK
   */
  SamplePost(sample: SampleDto): Observable<SampleDto> {
    return this.SamplePostResponse(sample).pipe(
      __map(_r => _r.body)
    );
  }

  /**
   * @param id undefined
   * @return OK
   */
  SampleGet_1Response(id: number): Observable<StrictHttpResponse<SampleDto>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    let req = new HttpRequest<any>(
      'GET',
      this.rootUrl + `/api/Sample/${id}`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as StrictHttpResponse<SampleDto>;
      })
    );
  }
  /**
   * @param id undefined
   * @return OK
   */
  SampleGet_1(id: number): Observable<SampleDto> {
    return this.SampleGet_1Response(id).pipe(
      __map(_r => _r.body)
    );
  }

  /**
   * @param id undefined
   */
  SampleDeleteResponse(id: number): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    let req = new HttpRequest<any>(
      'DELETE',
      this.rootUrl + `/api/Sample/${id}`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as StrictHttpResponse<null>;
      })
    );
  }
  /**
   * @param id undefined
   */
  SampleDelete(id: number): Observable<null> {
    return this.SampleDeleteResponse(id).pipe(
      __map(_r => _r.body)
    );
  }
}

module SampleService {
}

export { SampleService }
