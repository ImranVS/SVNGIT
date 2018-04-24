import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {DominoServerTasks} from '../models/domino-server-tasks';


@Component({
    templateUrl: './app/widgets/reports/components/o365-disabled-users-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Office365DisabledUsersList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    data: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/reports/disabled_users_with_license')
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