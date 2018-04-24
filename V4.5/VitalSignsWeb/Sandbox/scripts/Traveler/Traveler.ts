/// <reference path="../../node_modules/reflect-metadata/reflect-metadata.d.ts" />

import {Component, View, ElementRef, DynamicComponentLoader} from 'angular2/core';
import {HTTP_PROVIDERS}    from 'angular2/http';
import {Type, Injectable, Input, ViewEncapsulation, OnInit} from 'angular2/core';
import {RouteConfig, ROUTER_DIRECTIVES, RouteRegistry} from 'angular2/router';
import {Http, Response} from 'angular2/http';
import {Observable}     from 'rxjs/Observable';

import {Injector, bind }  from 'angular2/core'; 

import {CountUserDevicesByType} from './Views/CountUserDevicesByType';
import {CountUserDevicesByOS}   from './Views/CountUserDevicesByOS';
// import {TravelerViewsServices}  from './Views.services';
import {Chart}                  from '../Templates/Views';


@Component({
    selector: 'traveler',
})
@View({
    template: `
    <div>
    <h1>Traveler</h1>
    <div #container></div>
    </div>
  `
})
export class Traveler {
    loader: DynamicComponentLoader;
    elementRef: ElementRef;

    constructor(loader: DynamicComponentLoader, elementRef: ElementRef) {
        this.loader = loader;
        this.elementRef = elementRef;
    
        // Some async action (maybe ajax response with html in it)
        setTimeout(() => this.renderTemplate(), 1000);
    }

    renderTemplate() {

        System.import('./Views/CountUserDevicesByOS').then(m => { this.loader.loadIntoLocation(m, this.elementRef, 'container') });

        //this.loader.loadIntoLocation(
        //    toComponent('/templates/Traveler/MobileDevices.html', 'userdevices'),
        //    this.elementRef,
        //    'container'
        //);

        //this.loader.loadIntoLocation(
        //    toComponent('/templates/Traveler/views/CountUserDevicesByOS.html', 'CountUserDevicesByOS'),
        //    this.elementRef,
        //    'container'
        //);

        //this.loader.loadIntoLocation(
        //    toComponent('/templates/Traveler/views/CountUserDevicesByType.html', 'CountUserDevicesByType'),
        //    this.elementRef,
        //    'container'
        //)
    }
}




function toComponent(templateUrl, serviceUrl) {

    @Component({
        selector: 'generic-selector',
        templateUrl: templateUrl,
        providers: [HTTP_PROVIDERS]
    })

    class GenericComponent implements OnInit {
        errorMessage: string;
        data: any;
   
        constructor(private http: Http) { }
        private _serviceUrl = 'http://private-405397-vitalsignstravelerapi.apiary-mock.com/traveler/';
        getTravelerData() {
            return this.http.get(this._serviceUrl + serviceUrl)
                .map(res => res.json())
                .catch(this.handleError);
        }

        ngOnInit() {
            this.getTravelerData().subscribe(
                data => this.data = data,
                error => this.errorMessage = <any>error
            );
        }

        private handleError(error: Response) {
            console.error(error);
            return Observable.throw(error.json().error || 'Mobile Devices Service error');
        }
    }

    return GenericComponent;

}
    


//class MobileDevice 
//{
//    constructor(private http: Http) { }

//    private _serviceUrl = 'http://private-568d1-vitalsignstravelerapi.apiary-mock.com/traveler/userdevices';

//    getMobileDevices() {  
//    console.log(this.http.get(this._serviceUrl));

//    return this.http.get(this._serviceUrl)
//        .map(res => <MobileDevice[]>res.json())
//        .catch(this.handleError);
//}

