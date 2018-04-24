import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {MailThreshold} from '../models/mail-threshold';


@Component({
    templateUrl: './app/widgets/reports/components/websphere-threshold-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class WebsphereThresholdList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    websphereThreshold: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/reports/websphere_threshold')
            .subscribe(
            data => this.websphereThreshold = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}