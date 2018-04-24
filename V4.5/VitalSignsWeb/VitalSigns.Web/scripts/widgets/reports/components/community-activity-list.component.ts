import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {CommunityActivity} from '../models/community-activity';


@Component({
    templateUrl: './app/widgets/reports/components/community-activity-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class CommunityActivityList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    communityActivity: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get(this.settings.url)
            .subscribe(
            data => this.communityActivity = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}