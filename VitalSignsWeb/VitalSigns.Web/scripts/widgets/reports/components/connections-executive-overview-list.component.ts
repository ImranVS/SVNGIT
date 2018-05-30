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
                try {
                    console.log("Here")
                    console.log(data.data)
                    data.data.sort(function (a, b) {
                        if (a.base_type < b.base_type) return -1;
                        if (a.base_type > b.base_type) return 1;
                        return 0;
                    });
                    var newData = [];
                    for (var i = 0; i < data.data.length; i++) {
                        var curr = data.data[i];
                        console.log("Executing 1 :")
                        console.log(curr)
                        if (!curr.base_types)
                            curr.base_types = [];
                        for (var y = 0; y < curr.types.length; y++) {
                            console.log("executing 2:");
                            console.log(curr, curr.types[y]);
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
                    console.log(data)
                    //console.log(data.data);
                    //data.data.forEach(y => y.types.forEach(x => x => x["base_type"] = x.type.replace("Community").trim() === "" ? x.type : x.type.replace("Community").trim()));
                    //console.log(data.data)
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