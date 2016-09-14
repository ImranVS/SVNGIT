import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HTTP_PROVIDERS}    from '@angular/http';
import {WidgetComponent, WidgetService} from '../../core/widgets';
import {RESTService} from '../../core/services';
import {AppNavigator} from '../../navigation/app.navigator.component';
import {ServiceTab} from '../models/service-tab.interface';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/services/components/service-servertasks-grid.component.html',
    directives: [
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem,
        AppNavigator
    ],
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class ServiceServerTasksGrid implements OnInit {
    @Input() settings: any;
    deviceId: any;
    //data: wijmo.collections.CollectionView;
    monitoredData: wijmo.collections.CollectionView;
    nonMonitoredData: wijmo.collections.CollectionView;
    errorMessage: string;

    constructor(private service: RESTService, private widgetService: WidgetService, private route: ActivatedRoute) { }

    get pageSize(): number {
        if (this.nonMonitoredData.pageSize) {
            return this.nonMonitoredData.pageSize;
        }
        else {
            return this.monitoredData.pageSize;
        }

    }



    set pageSize(value: number) {
        if (this.nonMonitoredData.pageSize != value) {
            this.nonMonitoredData.pageSize = value;
            this.nonMonitoredData.refresh();
        }
        if (this.monitoredData.pageSize != value) {
            this.monitoredData.pageSize = value;
            this.monitoredData.refresh();
        }
    }

    ngOnInit() {

       
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.service.get('/DashBoard/' + this.deviceId + '/monitoredtasks')
            .subscribe(
            (response) => {
                //this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.nonMonitoredData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.nonMonitoredData));
                this.nonMonitoredData.pageSize = 10;
                this.monitoredData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.monitoredData));
                this.monitoredData.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );

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

        console.log(event.panel.grid.selectedItems);

        this.widgetService.refreshWidget('responseTimes')
            .catch(error => console.log(error));

    }
}