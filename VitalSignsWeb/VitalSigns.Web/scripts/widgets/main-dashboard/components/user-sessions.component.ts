import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

@Component({
    templateUrl: './app/widgets/main-dashboard/components/user-sessions.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class UserSessions implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;
    responses: string;

    usersessionsStatus: any;

    constructor(private service: RESTService) { }

    loadData() {

        this.service.get('/services/dashboard_stats')
            .subscribe(
            response => this.usersessionsStatus = response.data,
          
            error => this.errorMessage = <any>error
        );
    }

    ngOnInit() {
        this.loadData();
       
    }
    refresh() {
        this.loadData();
    }

}