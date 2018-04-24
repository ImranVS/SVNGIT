import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {CommunityUsers} from '../models/community-users';


@Component({
    templateUrl: './app/widgets/reports/components/inactive-community-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class InactiveCommunityList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    data: any;

    constructor(private service: RESTService) { }

    loadData(url) {
        this.service.get(url)
            .subscribe(
            data => this.data = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData('/reports/connections/inactive_community_list');
    }

    refresh(url: string) {
        this.loadData(url);
    }
}