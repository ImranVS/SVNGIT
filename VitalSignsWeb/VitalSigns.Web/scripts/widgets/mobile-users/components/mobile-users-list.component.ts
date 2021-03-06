﻿import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {MobileUser} from '../models/mobile-user';

@Component({
    templateUrl: './app/widgets/mobile-users/components/mobile-users-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class MobileUsers implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    mobileUsers: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/dashboard/mobile_user_devices')
            .subscribe(
            data => this.mobileUsers = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}