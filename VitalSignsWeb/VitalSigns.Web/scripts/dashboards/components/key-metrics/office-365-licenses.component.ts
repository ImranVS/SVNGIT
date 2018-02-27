import { Component, Input, OnInit, ViewChild, ComponentFactoryResolver, ViewChildren, QueryList } from '@angular/core';
import { WidgetController, WidgetContainer, WidgetContract } from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';

import { MicrosoftPowerShellScripts, initSettings } from '../../../services/components/microsoft-powershell-scripts.component';

import {HttpModule} from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';
import * as widgets from '../../../app.widgets';

declare var injectSVG: any;

@Component({
    templateUrl: './app/dashboards/components/key-metrics/office-365-licenses.component.html',
    providers: [
        WidgetService,
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        gridHelpers.CommonUtils,
        helpers.DateTimeHelper,
    ]
})
export class Office365Licenses implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChildren(WidgetContainer) containers: QueryList<WidgetContainer>;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 20;

    widgets: WidgetContract[] = [
        {
            id: 'mobileDevicesOSChart',
            title: 'Mobile devices by OS (click to drill down)',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/DashBoard/office_365_licenses_used_available',
                chart: {
                    chart: {
                        renderTo: 'mobileDevicesOSChart',
                        type: 'pie',
                        height: 240
                    },
                    title: { text: '' },
                    subtitle: { text: '' },
                    xAxis: {
                        categories: []
                    },
                    yAxis: {
                        min: 0,
                        endOnTick: false,
                        allowDecimals: false,
                        title: {
                            enabled: false
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: false
                            },
                            showInLegend: true,
                            innerSize: '70%'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    exporting: {
                        enabled: false
                    },
                    legend: {
                        labelFormatter: function () {
                            return '<div style="font-size: 10px; font-weight: normal;">' + this.name + '</div>';
                        }
                    },
                    series: []
                }
            }
        }]
    showPowerScripts: boolean = false;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, protected toolTip: helpers.GridTooltip,
        protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService, protected datetimeHelpers: helpers.DateTimeHelper) {
        //super(resolver, widgetService);
    }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("Office365LicensesGrid", this.authService.CurrentUser.email),
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

    ExportExcel(event) {
        this.gridHelpers.ExportExcel(this.flex, "Office 365 Licenses.xlsx")
    }
    loadData() {
        this.service.get(`/reports/disabled_users_with_license`)
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                this.data.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
            );

        
    }

    ngOnInit() {
        this.loadData();
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("Office365MailboxViewGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }

    ngAfterViewChecked() {
        injectSVG();
    }

    gridSourceChanged() {
        this.flex.autoSizeColumns();
    }

}