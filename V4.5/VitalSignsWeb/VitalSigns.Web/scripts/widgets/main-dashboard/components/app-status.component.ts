import {Component, Input, OnInit, AfterViewChecked} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';
import * as helpers from '../../../core/services/helpers/helpers';
declare var injectSVG: any;

@Component({
    templateUrl: './app/widgets/main-dashboard/components/app-status.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper
    ]
})
export class AppStatus implements WidgetComponent, OnInit, AfterViewChecked {
    @Input() settings: any;

    errorMessage: string;

    appStatus: any;

    constructor(private service: RESTService, protected datetimeHelpers: helpers.DateTimeHelper) { }

    loadData() {
        this.service.get(`/dashboard/get_last_update`)
            .subscribe(
            response => this.appStatus = this.datetimeHelpers.toLocalDateTime(response.data),
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }

    ngAfterViewChecked() {
        injectSVG();
    }

    getStatusDescription(status: string) {

        switch (status) {
            case 'noIssue':
                return 'No issues';
            case 'notResponding':
                return 'Not responding';
            case 'issue':
                return 'Issues';
            case 'maintenance':
                return 'Maintenance';
        }

    }
}