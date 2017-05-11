import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;

@Component({
    templateUrl: '/app/configurator/components/alert/alerts-allalert-history.component.html',
    providers: [
        WidgetService,
        HttpModule,
        RESTService,
        helpers.DateTimeHelper,
        helpers.GridTooltip
    ]
})
export class AlertHistory implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    filterDate: string;
    notification_definitions: any;
    loading1 = false;
    loading2 = false;

    constructor(private service: RESTService, private route: ActivatedRoute, protected widgetService: WidgetService,
        protected datetimeHelpers: helpers.DateTimeHelper, protected toolTip: helpers.GridTooltip) { }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            this.flex.refresh();
            this.toolTip.getTooltip(this.flex, 0, 7);
        }
    }
    ngOnInit() {

        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
        });
        this.service.get('/configurator/notifications_selector')
            .subscribe(
            (data) => {
                this.notification_definitions = data.data.notificationsList;
            },
            (error) => this.errorMessage = <any>error
            );
        var date = new Date();
        var displayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()).toISOString();
        this.service.get('/configurator/viewalerts')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
        );
        var today = new Date();
        this.filterDate = today.toISOString().substr(0, 10);
        // Create custom tooltip
        this.toolTip.getTooltip(this.flex, 0, 7);
    }

    filterAlerts() {
        this.loading1 = true;
        var dt = new Date(this.filterDate);
        this.service.get('/configurator/viewalerts?statdate=' + dt.toISOString().substr(0, 10))
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                this.data.pageSize = 10;
                this.loading1 = false;
                this.toolTip.getTooltip(this.flex, 0, 7);
            },
            (error) => this.errorMessage = <any>error
            );

    }

    filterAlertsByDef(notification_sel: wijmo.input.ComboBox) {
        this.loading2 = true;
        this.service.get('/configurator/notifications_by_id?id=' + notification_sel.selectedValue)
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                this.data.pageSize = 10;
                this.loading2 = false;
                this.toolTip.getTooltip(this.flex, 0, 7);
            },
            (error) => this.errorMessage = <any>error
            );

    }
}



