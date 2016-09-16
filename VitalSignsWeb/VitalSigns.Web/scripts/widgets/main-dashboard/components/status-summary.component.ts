import {Component, Input, OnInit} from '@angular/core';
import {HTTP_PROVIDERS}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

@Component({
    templateUrl: './app/widgets/main-dashboard/components/status-summary.component.html',
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class StatusSummary implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;
    statusSummary: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/services/dashboard_summary')
            .subscribe(
            response => this.statusSummary = response.data,
            error => this.errorMessage = <any>error
        );
    }

    ngOnInit() {
        this.loadData();
    }
}