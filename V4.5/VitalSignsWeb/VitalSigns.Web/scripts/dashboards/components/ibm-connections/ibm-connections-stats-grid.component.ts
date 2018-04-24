import {Component, Input, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import {IBMConnectionsGrid} from './ibm-connections-grid.component';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-stats-grid',
    templateUrl: './app/dashboards/components/ibm-connections/ibm-connections-stats-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class IBMConnectionsStatsGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    serviceId: string;
    errorMessage: string;
    _serviceId: string;
    url: string;
    currentPageSize: any = 20;
    
    
    constructor(private service: RESTService, private widgetService: WidgetService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("IBMConnectionsStatsGrid", this.authService.CurrentUser.email),
                value: value
            };

            this.service.put(`/services/set_name_value`, obj)
                .subscribe(
                (data) => {

                },
                (error) => console.log(error)
                );
        }
    }

    ngOnInit() {
        //this.route.params.subscribe(params => {
        //    if (params['service'])
        //        this.serviceId = params['service'];
        //    else {

        //        this.serviceId = this.widgetService.getProperty('serviceId');
        //    }
        //});

        this.serviceId = this.widgetService.getProperty('serviceId');
        var date = new Date();
        var displayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()).toISOString();

        if (this.widgetService.getProperty("tabname") != "OVERVIEW") {
            this.url = `/services/summarystats?statName=NUM_OF_${this.widgetService.getProperty("tabname")}_*&deviceId=${this.serviceId}&isChart=false&startDate=${displayDate}&endDate=${displayDate}&regex=^(?:(?!_YESTERDAY).)*?$`;
        }
        else {
            this.url = `/services/summarystats?statName=NUM_OF_*&deviceId=${this.serviceId}&isChart=false&startDate=${displayDate}&endDate=${displayDate}&regex=^(?:(?!_YESTERDAY).)*?$`;
        }
        this.service.get(this.url)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
            );
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("IBMConnectionsStatsGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }

    //refresh(serviceUrl?: string) {

    //    var date = new Date();
    //    var displayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()).toISOString();

    //    this.service.get(`/services/summarystats?statName=NUM_OF_${this.widgetService.getProperty("tabname")}_*&deviceId=${this.serviceId}&isChart=false&startDate=${displayDate}&endDate=${displayDate}&regex=^(?:(?!_YESTERDAY).)*?$`)
    //        .subscribe(
    //        (data) => {
    //            this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
    //            this.data.pageSize = 10;
    //        },
    //        (error) => this.errorMessage = <any>error
    //        );

    //}

    onPropertyChanged(key: string, value: any) {
        if (key === 'serviceId') {
            this.serviceId = value;
            var date = new Date();
            var displayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()).toISOString();
            if (this.widgetService.getProperty("tabname") != "OVERVIEW") {
                this.url = `/services/summarystats?statName=NUM_OF_${this.widgetService.getProperty("tabname")}_*&deviceId=${this.serviceId}&isChart=false&startDate=${displayDate}&endDate=${displayDate}&regex=^(?:(?!_YESTERDAY).)*?$`;
            }
            else {
                this.url = `/services/summarystats?statName=NUM_OF_*&deviceId=${this.serviceId}&isChart=false&startDate=${displayDate}&endDate=${displayDate}&regex=^(?:(?!_YESTERDAY).)*?$`;
            }
            this.service.get(this.url)
                .subscribe(
                (data) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                    this.data.pageSize = this.currentPageSize;
                },
                (error) => this.errorMessage = <any>error
                );
            
        }

    }

}