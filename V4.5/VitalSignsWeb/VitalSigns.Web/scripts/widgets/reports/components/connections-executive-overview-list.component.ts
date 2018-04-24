import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import { WidgetComponent } from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services';

import {CommunityActivity} from '../models/community-activity';


@Component({
    templateUrl: './app/widgets/reports/components/connections-executive-overview-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ConnectionsExecutiveOverviewList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    data: any;

    constructor(private service: RESTService, private widgetService: WidgetService) { }

    refresh(serviceUrl?: string) {

        this.loadData(serviceUrl);

    }

    loadData(serviceUrl?: string) {
        this.service.get(serviceUrl || this.settings.url)
        .subscribe(
            (data) => {
                this.data = data.data;
            },
            (error) => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}