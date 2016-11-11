﻿import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent, WidgetService} from '../../../core/widgets';

@Component({
    selector: 'server-filter',
    templateUrl: '/app/reports/filters/components/server-filter.component.html',
})
export class ServerFilter {
    @ViewChildren('multisel1') multisel1: wijmo.input.MultiSelect;
    selectedServers: any;
    serverType: string;
    @Input() deviceType: any; 
    @Input() widgetName: string;
    @Input() widgetURL: string;
    @Input() hideDatePanel: boolean;
    @Input() hideServerControl: boolean;
    startDate: Date = new Date();
    endDate: Date = new Date();
    deviceNameData: any;
    errorMessage: any;

    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) { }
    ngOnInit() {
        //this.route.queryParams.subscribe(params => {
        //    this.serverType = params['server-type'];
        //});

        this.service.get(`/services/server_list_dropdown?type=${this.deviceType}`)
            .subscribe(
            (response) => {
                this.deviceNameData = response.data.deviceNameData;
            },
            (error) => this.errorMessage = <any>error
        );
        if (this.hideDatePanel == true) {
            var v = <HTMLDivElement>document.getElementById("dtPanel");
            v.style.display = "none";
        }
        if (this.hideServerControl == true) {
            var v1 = <HTMLDivElement>document.getElementById("dtServer");
            v1.style.display = "none";
        }
        if (this.hideServerControl == true && this.hideServerControl == true) {
            var v2 = <HTMLDivElement>document.getElementById("dtFilter");
            v2.style.display = "none";
        }
        //Set a selected value of the Status drop down box to the passed query parameter or -All- if no parameter is available
        //this.deviceStatus = paramstatus;
    }
    applyFilters(multisel1: wijmo.input.MultiSelect) {
        console.log(multisel1.checkedItems);
       
        //var v = multisel1.checkedItems;
        var selectedServers = "";
        for (var item of multisel1.checkedItems) {
            if (selectedServers == "") 
                selectedServers = item.id;
            else 
                selectedServers += "," + item.id;
        }
        
        this.widgetURL += selectedServers + `&start=` + this.startDate.toISOString() + `&end=` + this.endDate.toISOString();
        console.log(this.widgetURL);
        //});
        //this.widgetService.refreshWidget('avgcpuutilchart', `/reports/summarystats_chart?statName=Platform.System.PctCombinedCpuUtil&deviceId=` + selectedServers + `&start=` + this.startDate.toISOString() + `&end=` + this.endDate.toISOString())
        //    .catch(error => console.log(error));
        this.widgetService.refreshWidget(this.widgetName, this.widgetURL.toString() )
            .catch(error => console.log(error));

    }

}