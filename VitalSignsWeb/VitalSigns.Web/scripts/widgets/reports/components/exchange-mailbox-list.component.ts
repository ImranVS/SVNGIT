import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {MailFileStatistic} from '../models/mail-file-statistic';


@Component({
    templateUrl: './app/widgets/reports/components/exchange-mailbox-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ExchnagemailboxList implements WidgetComponent, OnInit {
    @Input() settings: any;
    errorMessage: string;
    data: any;
    constructor(private service: RESTService) { }

    loadData() {
        this.service.get(`/reports/exchnage_mailboxes_prohbited_warning`)
            .subscribe(
            data => this.data = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}