
import {Component, OnInit} from 'angular2/core';
import {HTTP_PROVIDERS}    from 'angular2/http';

import {Chart}                      from '../../Templates/Views';
import {TravelerViewsServices}      from '../Views.services';

@Component({
    selector: 'count-user-device-by-os',
    templateUrl: '/templates/Traveler/views/CountUserDevicesByOS.html',
    providers: [
        HTTP_PROVIDERS,
        TravelerViewsServices
    ]
})
export class CountUserDevicesByOS implements OnInit {
    
    errorMessage: string;
    data: Chart;

    constructor(private _mobileDevicesService: TravelerViewsServices){ }

    getCountUserDevicesByType() {
        this._mobileDevicesService.getViewChartByName('CountUserDevicesByOS')
            .subscribe(
                chart => this.data = chart,
                error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
       this.getCountUserDevicesByType();
    }
}