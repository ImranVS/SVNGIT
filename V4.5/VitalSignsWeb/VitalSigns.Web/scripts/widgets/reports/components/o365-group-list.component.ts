import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {MailFileStatistic} from '../models/mail-file-statistic';


@Component({
    templateUrl: './app/widgets/reports/components/o365-group-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class O365GroupList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    data: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/reports/group_collections')
            .subscribe(
            data => this.data = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}