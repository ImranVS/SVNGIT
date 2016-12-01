import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/configurator/components/alert/alerts-allalert-history.component.html',
    providers: [
        WidgetService,
        HttpModule,
        RESTService
    ]
})
export class AlertHistory implements OnInit {
    @Input() settings: any;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    filterDate: Date;

    constructor(private service: RESTService, private route: ActivatedRoute, protected widgetService: WidgetService) { }

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
        var date = new Date();
        var displayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()).toISOString();
        this.service.get('/configurator/viewalerts?statdate=' + displayDate)
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );
    }

    filterAlerts() {
        this.service.get('/configurator/viewalerts?statdate=' + this.filterDate)
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );

    }
}



