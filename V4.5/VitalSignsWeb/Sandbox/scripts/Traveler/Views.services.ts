import {Injectable}     from 'angular2/core';
import {Http, Response} from 'angular2/http';
import {Observable}     from 'rxjs/Observable';

import {Chart}       from '../Templates/Views';

@Injectable()
export class TravelerViewsServices {
    constructor(private http: Http) { }

    private _serviceUrl = 'http://private-405397-vitalsignstravelerapi.apiary-mock.com/traveler/views/';

    getViewChartByName(viewName: string) {
        return this.http.get(this._serviceUrl + viewName)
            .map(res => <Chart>res.json())
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'TravelerViewsServices Service error');
    }

}
