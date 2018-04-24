import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {MailThreshold} from '../models/mail-threshold';


@Component({
    templateUrl: './app/widgets/reports/components/mail-threshold-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class MailThresholdList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    mailThreshold: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/reports/domino_mail_threshold')
            .subscribe(
            data => this.mailThreshold = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}