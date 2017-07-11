import {Component, Input, OnInit} from '@angular/core';
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

declare var injectSVG: any;


@Component({
    templateUrl: '/app/services/components/service-travelermailservers-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class ServiceTravelerMailServersGrid implements OnInit {
    @Input() settings: any;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    //data1: wijmo.collections.CollectionView;
    //maildata: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 20;


    constructor(private service: RESTService, private widgetService: WidgetService, private route: ActivatedRoute, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService
) { }

    get pageSize(): number {
        return this.data.pageSize;
    }
    
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("ServiceTravelerMailServersGrid", this.authService.CurrentUser.email),
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

     
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.service.get('/DashBoard/' + this.deviceId + '/traveler_mailstats')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
        );
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("ServiceTravelerMailServersGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        //this.route.params.subscribe(params => {
        //    this.deviceId = params['service'];

        //});

        //this.service.get('/DashBoard/' + this.deviceId + '/traveler_mailstats')
        //    .subscribe(
        //    (response) => {
        //        this.maildata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.maildata));
        //        this.maildata.pageSize = 10;
        //    },
        //    (error) => this.errorMessage = <any>error
        //    );

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

}