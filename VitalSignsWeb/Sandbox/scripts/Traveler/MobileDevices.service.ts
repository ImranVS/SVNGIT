import {Injectable}     from 'angular2/core';
import {Http, Response} from 'angular2/http';
import {Observable}     from 'rxjs/Observable';

import {MobileDevice}         from './MobileDevice';

@Injectable()
export class MobileDevicesService {
    constructor(private http: Http) { }

    private _serviceUrl = 'http://private-568d1-vitalsignstravelerapi.apiary-mock.com/traveler/userdevices';

    getMobileDevices() {  
        console.log(this.http.get(this._serviceUrl));  

        return this.http.get(this._serviceUrl)
            .map(res => <MobileDevice[]>res.json())
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Mobile Devices Service error');
    }

}
