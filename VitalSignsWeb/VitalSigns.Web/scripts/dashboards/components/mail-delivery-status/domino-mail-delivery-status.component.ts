import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';

import * as helpers from '../../../core/services/helpers/helpers';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import { CommonUtils } from '../../../core/services/helpers/gridutils';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/mail-delivery-status/domino-mail-delivery-status.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper,
        gridHelpers.CommonUtils
    ]
})
export class DominoMailDeliveryStatus implements OnInit {
    @Input() settings: any;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 20;

    private abc: CommonUtils = new CommonUtils();

    constructor(private service: RESTService, private route: ActivatedRoute, protected datetimeHelpers: helpers.DateTimeHelper, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("DominoMailDeliveryStatus", this.authService.CurrentUser.email),
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
    

   
    //
    
    ngOnInit() {
        

        this.service.get('/DashBoard/get_mail_delivery_status')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                this.data.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
            );
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("DominoMailDeliveryStatus", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }


}



