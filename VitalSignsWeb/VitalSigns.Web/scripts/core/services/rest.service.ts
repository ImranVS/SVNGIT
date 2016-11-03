import {Injectable}     from '@angular/core';
import {Http, Response} from '@angular/http';
import {Observable}     from 'rxjs/Observable';

@Injectable()
export class RESTService {

  // serverUrl = 'http://private-f4c5b-vitalsignssandboxserver.apiary-mock.com';
   // serverUrl = 'http://private-ad10c-ibm.apiary-mock.com';
    //serverUrl ='http://dev11.vsplus.jnitinc.com:5000';
    //serverUrl = 'http://localhost:1234';
    serverUrl = 'http://localhost:5000';
    constructor(protected http: Http) { }

    get(path: string) {
        
        let serviceUrl: string = path.indexOf('://') > -1 ? path : this.serverUrl + path;
        
        return this.http.get(serviceUrl)
            .map(res => res.json())
            .catch(this.handleError);
    }

    post(path: string, body: any, callback?: () => void) {

        let serviceUrl: string = path.indexOf('://') > -1 ? path : this.serverUrl + path;

        this.http.post(serviceUrl, body)
            .subscribe(res => { callback(); });

    }

    putAndCallback(path: string, body: any, callback: () => void) {

        let serviceUrl: string = path.indexOf('://') > -1 ? path : this.serverUrl + path;

        this.http.put(serviceUrl, body)
            .subscribe(res => { callback(); });

    }

    put(path: string, body: any) {

        let serviceUrl: string = path.indexOf('://') > -1 ? path : this.serverUrl + path;

      return  this.http.put(serviceUrl, body)
          .map(res => res.json())
          .catch(this.handleError);

    }

    delete(path: string, callback?: () => void) {

        let serviceUrl: string = path.indexOf('://') > -1 ? path : this.serverUrl + path;
        
        this.http.delete(serviceUrl)
            .subscribe(res => { callback(); });
    }

    private handleError(error: Response) {
        return Observable.throw(error || 'Server error');
    }

}