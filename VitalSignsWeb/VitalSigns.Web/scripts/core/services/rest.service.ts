﻿import {Injectable}     from '@angular/core';
import {Http, Headers, RequestOptions, Response} from '@angular/http';
import {Observable}     from 'rxjs/Observable';

import {AppConfig} from '../../app.config';

import {AuthenticationService} from '../../profiles/services/authentication.service';

@Injectable()
export class RESTService {

<<<<<<< .mine
    private serverUrl;
||||||| .r1638
  // serverUrl = 'http://private-f4c5b-vitalsignssandboxserver.apiary-mock.com';
   // serverUrl = 'http://private-ad10c-ibm.apiary-mock.com';
    //serverUrl ='http://usdopvsdev11svc.jnittech.com';
    //serverUrl = 'http://localhost:1234';
    serverUrl = 'http://localhost:5000';
    constructor(protected http: Http) { }
=======
  // serverUrl = 'http://private-f4c5b-vitalsignssandboxserver.apiary-mock.com';
   // serverUrl = 'http://private-ad10c-ibm.apiary-mock.com';
    //serverUrl ='http://usdopvsdev11svc.jnittech.com';
   serverUrl = 'http://localhost:1234';
   // serverUrl = 'http://localhost:5000';
    constructor(protected http: Http) { }
>>>>>>> .r1734

    private get requestOptions(): RequestOptions {

        let headers = new Headers({ 'Authorization': 'Bearer ' + this.authService.UAT });

        return new RequestOptions({ headers: headers });

    }

    constructor(
        private config: AppConfig,
        private http: Http,
        private authService: AuthenticationService
    ) {

        this.serverUrl = config.getConfig('apiEndpoint');

    }

    get(path: string) {

        let serviceUrl: string = path.indexOf('://') > -1 ? path : this.serverUrl + path;

        return this.http.get(serviceUrl, this.requestOptions)
            .map(res => res.json())
            .catch(this.handleError);
    }

    post(path: string, body: any, callback?: () => void) {

        let serviceUrl: string = path.indexOf('://') > -1 ? path : this.serverUrl + path;

        this.http.post(serviceUrl, body, this.requestOptions)
            .subscribe(res => { callback(); });

    }

    putAndCallback(path: string, body: any, callback: () => void) {

        let serviceUrl: string = path.indexOf('://') > -1 ? path : this.serverUrl + path;

        this.http.put(serviceUrl, body, this.requestOptions)
            .subscribe(res => { callback(); });

    }

    put(path: string, body: any) {

        let serviceUrl: string = path.indexOf('://') > -1 ? path : this.serverUrl + path;

        return this.http.put(serviceUrl, body, this.requestOptions)
            .map(res => res.json())
            .catch(this.handleError);

    }

    deleteAndCallback(path: string, callback?: () => void) {

        let serviceUrl: string = path.indexOf('://') > -1 ? path : this.serverUrl + path;

        this.http.delete(serviceUrl, this.requestOptions)
            .subscribe(res => { callback(); });
    }

    delete(path: string) {
        let serviceUrl: string = path.indexOf('://') > -1 ? path : this.serverUrl + path;
        return this.http.delete(serviceUrl)
            .map(res => res.json())
            .catch(this.handleError);
    }

    private handleError(error: Response) {

        return Observable.throw(error || 'Server error');

    }

}