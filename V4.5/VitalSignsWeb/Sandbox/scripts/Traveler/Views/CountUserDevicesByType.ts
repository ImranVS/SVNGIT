
import {Component, OnInit} from 'angular2/core';
import {HTTP_PROVIDERS}    from 'angular2/http';

import {Type, Injectable, Input, ViewEncapsulation} from 'angular2/core';
import {RouteConfig, ROUTER_DIRECTIVES, RouteRegistry} from 'angular2/router';

import {Chart}                      from '../../Templates/Views';
import {TravelerViewsServices}      from '../Views.services';

@Component({
    selector: 'count-user-device-by-type',
    templateUrl: '/templates/Traveler/views/CountUserDevicesByType.html',
    providers: [
        HTTP_PROVIDERS,
        TravelerViewsServices
    ],
    properties: ['myParentConfig: my-parent-config']
})
export class CountUserDevicesByType implements OnInit {

    errorMessage: string;
    data: Chart;

    constructor(private _mobileDevicesService: TravelerViewsServices) { }

    getCountUserDevicesByType() {
        this._mobileDevicesService.getViewChartByName('CountUserDevicesByType')
            .subscribe(
            chart => this.data = chart,
            error => this.errorMessage = <any>error
            );
    }

    getRoutes(component: Type) {
        return Reflect.getMetadata('annotations', component)
            .filter(a => {
                console.log(a.constructor.name);
                return a.constructor.name === 'ComponentMetadata';
            }).pop();
    }

    ngOnInit() {
        console.log(this.getRoutes(this.constructor));
        this.getCountUserDevicesByType();
    }
}