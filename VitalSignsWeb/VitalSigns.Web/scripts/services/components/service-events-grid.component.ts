﻿import { Component, Input, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';
import {RESTService} from '../../core/services';
import {AppNavigator} from '../../navigation/app.navigator.component';
import { ServiceTab } from '../models/service-tab.interface';
import { AuthenticationService } from '../../profiles/services/authentication.service';
import * as gridHelpers from '../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';


import * as helpers from '../../core/services/helpers/helpers';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/services/components/service-events-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper,
        helpers.GridTooltip,
        gridHelpers.CommonUtils
    ]
})
export class ServiceEventsGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;
    deviceId: any;
    serviceId: string;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 20;

    constructor(private service: RESTService, private widgetService: WidgetService, private route: ActivatedRoute,
        protected datetimeHelpers: helpers.DateTimeHelper, protected toolTip: helpers.GridTooltip, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService
) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("ServiceEventsGrid", this.authService.CurrentUser.email),
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
        this.loadData();
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("ServiceEventsGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        this.toolTip.getTooltip(this.flex, 0, 3);
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

    loadData() {
        var serviceId = this.widgetService.getProperty('serviceId');
        if (serviceId) {
            var res = serviceId.split(';');
            this.deviceId = res[0];
        }
        else {
            this.route.params.subscribe(params => {
                if (params['service']) {
                    this.serviceId = params['service']
                    if (this.serviceId) {
                        var res = this.serviceId.split(';');
                        this.deviceId = res[0];
                    }
                }
            });
        }
        this.service.get('/dashboard/' + this.deviceId + '/notifications')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                this.data.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
            );
    }

    refresh(settings: any) {
        this.loadData();
    }
}