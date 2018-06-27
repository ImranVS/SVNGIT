import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {MailFileStatistic} from '../models/mail-file-statistic';


@Component({
    templateUrl: './app/widgets/reports/components/mailbox-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class MailboxList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    data: any;
    isLoading: boolean = true;
    constructor(private service: RESTService) { }

    loadData() {
        this.isLoading = true;
        let deviceType = this.settings.deviceType;
        let mailboxType = this.settings.mailboxType;
        this.service.get(`/reports/mailboxes?deviceType=${deviceType}&mailboxType=${mailboxType}`)
            .finally(() => this.isLoading = false)
            .subscribe(
            data => this.data = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}