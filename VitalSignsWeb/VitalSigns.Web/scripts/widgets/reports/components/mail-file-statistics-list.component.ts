import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {MailFileStatistic} from '../models/mail-file-statistic';


@Component({
    templateUrl: './app/widgets/reports/components/mail-file-statistics-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class MailFileStatisticsList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    mailFileStats: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/dashboard/database?filter_by=IsMailFile&filter_value=true&order_by=Title&order_type=asc')
            .subscribe(
            data => this.mailFileStats = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}