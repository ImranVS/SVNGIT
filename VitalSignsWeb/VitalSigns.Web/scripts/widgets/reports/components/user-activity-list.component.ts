import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {CommunityActivity} from '../models/community-activity';


@Component({
    templateUrl: './app/widgets/reports/components/user-activity-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class UserActivityList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    userActivity: any;
    objectTypes: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get(this.settings.url)
        .subscribe(
            (data) => {
                this.userActivity = data.data[0];
                this.objectTypes = data.data[1];
            },
            (error) => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}