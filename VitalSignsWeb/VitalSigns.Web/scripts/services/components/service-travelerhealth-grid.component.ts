import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';
import {RESTService} from '../../core/services';
import {AppNavigator} from '../../navigation/app.navigator.component';
import {ServiceTab} from '../models/service-tab.interface';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../core/services/helpers/helpers';
declare var injectSVG: any;


@Component({
    templateUrl: '/app/services/components/service-travelerhealth-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip
    ]
})
export class ServiceTravelerHealthGrid implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    //data1: wijmo.collections.CollectionView;
    maildata: wijmo.collections.CollectionView;
    errorMessage: string;

    constructor(private service: RESTService, private widgetService: WidgetService, private route: ActivatedRoute, protected toolTip: helpers.GridTooltip) { }

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

     
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.service.get(`/dashboard/traveler-health?deviceid=${this.deviceId}`)
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                //this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
        );
        this.toolTip.getTooltip(this.flex, 0, 2);
        this.toolTip.getTooltip(this.flex, 0, 10, 0)
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

    onItemsSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        // autosize first header row
        this.flex.autoSizeRow(0, true);

    }
}