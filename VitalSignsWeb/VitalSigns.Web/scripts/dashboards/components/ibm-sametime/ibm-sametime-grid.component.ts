import {Component, Input, OnInit} from '@angular/core';
import {HTTP_PROVIDERS}    from '@angular/http';

import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    templateUrl: './app/dashboards/components/ibm-sametime/ibm-sametime-grid.component.html',
    directives: [
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem
    ],
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class IBMSametimeGrid implements WidgetComponent, OnInit {
    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    serviceId: string;
    
    constructor(private service: RESTService, private widgetService: WidgetService) { }

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

        this.service.get('/services/status_list?type=Sametime')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
                this.data.moveCurrentToPosition(0);
                this.serviceId = this.data.currentItem.device_id;
            },
            (error) => this.errorMessage = <any>error
        );
        
    }

    ngAfterViewInit() {
        //console.log('after init) device id is: ' + this.serviceId);
        //this.widgetService.refreshWidget('responseTimes', `/services/statistics?statName=ResponseTime&deviceid=${this.serviceId}&operation=hourly`)
        //    .catch(error => console.log(error));
        console.log('after view init: ' + this.widgetService.exists('responseTimes'));
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

        console.log(`/services/statistics?statName=ResponseTime&deviceid=${event.panel.grid.selectedItems[0].device_id}&operation=hourly`);
        this.widgetService.refreshWidget('responseTimes', `/services/statistics?statName=ResponseTime&deviceid=${event.panel.grid.selectedItems[0].device_id}&operation=hourly`)
            .catch(error => console.log(error));
        this.widgetService.refreshWidget('dailyUserLogins', `/services/statistics?statName=Users&deviceid=${event.panel.grid.selectedItems[0].device_id}&operation=hourly`)
            .catch(error => console.log(error));
    }
}