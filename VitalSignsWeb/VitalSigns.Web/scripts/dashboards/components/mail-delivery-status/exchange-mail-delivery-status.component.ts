import { Component, Input, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as gridHelpers from '../../../core/services/helpers/gridutils';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/mail-delivery-status/exchange-mail-delivery-status.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ExchangeMailDeliveryStatus implements OnInit {
    @Input() settings: any;
    deviceId: any;
    
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    errorMessage: string;

    constructor(private service: RESTService, private route: ActivatedRoute, protected gridHelpers: gridHelpers.CommonUtils) { }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }
    ExportExcel(event) {
        this.gridHelpers.ExportExcel(this.flex, "ExchangeMailDeliveryStatus.xlsx")
    }

    ngOnInit() {

        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
        });

        this.service.get('/DashBoard/get_mail_delivery_status/' + 'Exchange')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 50;
            },
            (error) => this.errorMessage = <any>error
            );
    }
}



