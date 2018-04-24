import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import { AppComponentService } from '../../../core/services';
import { GridBase } from '../../../core/gridBase';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
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
        helpers.GridTooltip,
        gridHelpers.CommonUtils
    ]
})
export class AlertHistory extends GridBase implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 20;
    filterDate: string;
    notification_definitions: any;
    loading1 = false;
    loading2 = false;
    
    constructor(private dataProvider: RESTService, private route: ActivatedRoute, protected widgetService: WidgetService,
        protected datetimeHelpers: helpers.DateTimeHelper, protected toolTip: helpers.GridTooltip, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(dataProvider, appComponentService);
    }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("AlertHistory", this.authService.CurrentUser.email),
                value: value
            };

            this.service.put(`/services/set_name_value`, obj)
                .subscribe(
                (data) => {

                },
                (error) => console.log(error)
                );
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
            (error) => { this.errorMessage = <any>error;}
            );
        var date = new Date();
        var displayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()).toISOString();
        this.appComponentService.showProgressBar();
        this.service.get('/configurator/viewalerts')
            .subscribe(
            (response) => {
                this.appComponentService.hideProgressBar();
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                this.data.pageSize = this.currentPageSize;
                
            },
            (error) => {
                this.appComponentService.hideProgressBar();
                this.errorMessage = <any>error
            });
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("AlertHistory", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
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
                this.data.pageSize = this.currentPageSize;
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
                this.data.pageSize = this.currentPageSize;
                this.loading2 = false;
                this.toolTip.getTooltip(this.flex, 0, 7);
            },
            (error) => this.errorMessage = <any>error
            );

    }

    clearEvents() {
        this.service.put('/configurator/clear_alerts', {})
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.appComponentService.showSuccessMessage(response.message);
                }
                else {
                    this.appComponentService.showErrorMessage(response.message);
                }
            },
            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            });
    }

    deleteEvents() {
        this.service.put('/configurator/delete_alerts', {})
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.appComponentService.showSuccessMessage(response.message);
                }
                else {
                    this.appComponentService.showErrorMessage(response.message);
                }
            },
            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            });
    }
}
