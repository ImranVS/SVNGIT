import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {LogFile} from '../models/log-file';


@Component({
    templateUrl: './app/widgets/reports/components/log-file-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class LogFileList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    logFile: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/reports/log_file')
            .subscribe(
            data => this.logFile = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}