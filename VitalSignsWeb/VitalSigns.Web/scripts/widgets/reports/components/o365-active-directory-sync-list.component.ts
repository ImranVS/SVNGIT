﻿import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {DominoServerTasks} from '../models/domino-server-tasks';


@Component({
    templateUrl: './app/widgets/reports/components/o365-active-directory-sync-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Office365ActiveDirectorySyncList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    data: any;
    isLoading: Boolean = true;

    constructor(private service: RESTService) { }

    loadData() {
        this.isLoading = true;
        this.service.get('/reports/active_directory_sync_report')
            .finally(() => this.isLoading = false)
            .subscribe(
                data => this.data = data.data,
                error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
    refresh(value) {
        this.data.sort((o1, o2) => {
            if (o1[value] > o2[value]) return 1;
            if (o1[value] < o2[value]) return -1;
            return 0;
        });
    }
}