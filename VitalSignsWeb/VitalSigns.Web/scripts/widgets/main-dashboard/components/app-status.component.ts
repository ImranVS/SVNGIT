import {Component, Input, OnInit, AfterViewChecked} from '@angular/core';
import {HTTP_PROVIDERS}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

declare var injectSVG: any;

@Component({
    templateUrl: './app/widgets/main-dashboard/components/app-status.component.html',
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class AppStatus implements WidgetComponent, OnInit, AfterViewChecked {
    @Input() settings: any;

    errorMessage: string;

    appStatus: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('http://private-f4c5b-vitalsignssandboxserver.apiary-mock.com/status_widget/' + this.settings.serviceId)
            .subscribe(
            data => this.appStatus = data,
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
                return 'No issue';
            case 'notResponding':
                return 'Not responding';
            case 'issue':
                return 'Issues';
            case 'maintenance':
                return 'Maintenance';
        }

    }
}