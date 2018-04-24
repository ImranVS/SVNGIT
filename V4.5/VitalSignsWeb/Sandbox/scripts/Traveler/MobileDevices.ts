import {Component, OnInit} from 'angular2/core';
import {HTTP_PROVIDERS}    from 'angular2/http';

import {MobileDevice}             from './MobileDevice';
import {MobileDevicesService}     from './MobileDevices.service';

@Component({
    selector: 'mobile-devices',
    templateUrl: '/templates/Traveler/MobileDevices.html',
    providers: [
        HTTP_PROVIDERS,
        MobileDevicesService
    ]
})
export class MobileDevices implements OnInit {
    
    errorMessage: string;
    devices: MobileDevice[];

    constructor(private _mobileDevicesService: MobileDevicesService) { }

    getMobileDevices() {
        this._mobileDevicesService.getMobileDevices()
            .subscribe(
                devices => this.devices = devices,
                error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.getMobileDevices();
    }
}