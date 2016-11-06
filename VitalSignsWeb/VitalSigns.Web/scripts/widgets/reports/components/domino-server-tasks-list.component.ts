import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {DominoServerTasks} from '../models/domino-server-tasks.ts';


@Component({
    templateUrl: './app/widgets/reports/components/domino-server-tasks-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class DominoServerTasksList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    dominoServerTasks: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/reports/domino_server_tasks')
            .subscribe(
            data => this.dominoServerTasks = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}