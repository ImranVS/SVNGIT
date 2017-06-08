import { Component, Input, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';
import {RESTService} from '../../core/services';
import {AppNavigator} from '../../navigation/app.navigator.component';
import { ServiceTab } from '../models/service-tab.interface';
import { Office365Grid } from '../../dashboards/components/office365/office365-grid.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

import * as helpers from '../../core/services/helpers/helpers';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/services/components/service-mainhealth-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper,
        helpers.GridTooltip
    ]
})
export class ServiceMainHealthGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;
    deviceId: any;
    serviceId: string;
    data: wijmo.collections.CollectionView;
    errorMessage: string;

    
    constructor(private service: RESTService, private widgetService: WidgetService, private route: ActivatedRoute,
        protected datetimeHelpers: helpers.DateTimeHelper, protected toolTip: helpers.GridTooltip) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }

    ngOnInit() {
        var serviceId = this.widgetService.getProperty('serviceId');
        if (serviceId) {
            var res = serviceId.split(';');
            this.deviceId = res[0];
        }
        else {
            this.route.params.subscribe(params => {
                if (params['service'])
                    this.deviceId = params['service'];
                else {
                    if (this.serviceId) {
                        var res = this.serviceId.split(';');
                        this.deviceId = res[0];
                    }
                }
            });
        }    
        this.service.get('/DashBoard/'+ this.deviceId +'/health-assessment')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                this.data.pageSize = 20;
            },
            (error) => this.errorMessage = <any>error
            );
        // Create custom tooltip
        this.toolTip.getTooltip(this.flex, 1, 3);
    }

    getAccessColor(access: string) {

        switch (access) {
            case 'Allow':
                return 'green';
            case 'Blocked':
                return 'red';
            default:
                return '';
        }

    }

    refreshChart(event: wijmo.grid.CellRangeEventArgs) {
        this.widgetService.refreshWidget('responseTimes')
            .catch(error => console.log(error));

    }

    onPropertyChanged(key: string, value: any) {
        if (key === 'serviceId') {
            this.serviceId = value;
            this.service.get('/DashBoard/' + this.deviceId + '/health-assessment')
                .subscribe(
                (response) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                    this.data.pageSize = 10;
                },
                (error) => this.errorMessage = <any>error
                );
            // Create custom tooltip
            this.toolTip.getTooltip(this.flex, 1, 3);
        }

    }

}