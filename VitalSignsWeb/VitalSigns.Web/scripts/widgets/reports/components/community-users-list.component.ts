import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {CommunityUsers} from '../models/community-users';


@Component({
    templateUrl: './app/widgets/reports/components/community-users-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class CommunityUsersList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    communityUsers: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/reports/community_users')
            .subscribe(
            data => this.communityUsers = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}