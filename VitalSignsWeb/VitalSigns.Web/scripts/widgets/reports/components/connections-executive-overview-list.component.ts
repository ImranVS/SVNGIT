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
    isLoading: boolean = true;
    data: any;

    constructor(private service: RESTService, private widgetService: WidgetService) { }

    refresh(serviceUrl?: string) {

        this.loadData(serviceUrl);

    }

    loadData(serviceUrl?: string) {
        this.isLoading = true;
        this.service.get(serviceUrl || this.settings.url)
            .finally(() => this.isLoading = false)
            .subscribe(
            (data) => {
                try {
                    data.data.sort(function (a, b) {
                        if (a.base_type < b.base_type) return -1;
                        if (a.base_type > b.base_type) return 1;
                        return 0;
                    });
                    var newData = [];
                    for (var i = 0; i < data.data.length; i++) {
                        var curr = data.data[i];
                        if (!curr.base_types)
                            curr.base_types = [];
                        for (var y = 0; y < curr.types.length; y++) {
                            var currType = curr.types[y];
                            var entry = curr.base_types.find(x => x.base_type == currType.base_type);
                            if (!entry) {
                                entry = {
                                    base_type: currType.base_type,
                                    types: []
                                }
                                curr.base_types.push(entry);
                            }
                            entry.types.push({
                                type: currType.type,
                                new_count: currType.new_count,
                                total: currType.total
                            });
                        }
                    }
                    this.data = data.data;
                } catch (ex) {
                    console.log(ex);
                }
            },
                (error) => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}