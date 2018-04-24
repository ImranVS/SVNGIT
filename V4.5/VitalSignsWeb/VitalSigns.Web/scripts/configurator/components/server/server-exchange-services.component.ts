import {Component, OnInit, ViewChild,Input} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';

import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {ActivatedRoute} from '@angular/router';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {AppComponentService} from '../../../core/services';


@Component({
    templateUrl: '/app/configurator/components/server/server-exchange-services.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Services extends GridBase implements OnInit {
    selectedServers: string;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    servicesdata: any;
    errorMessage: string;
    currentForm: FormGroup;
    selectedSettingValue: any;
    selectedName: string;

    constructor(service: RESTService, private formBuilder: FormBuilder, private route: ActivatedRoute, appComponentService: AppComponentService) {
        super(service, appComponentService);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
        });
        //this.service.get('/Configurator/get_windows_services')
        //    .subscribe(
        //    (response) => {
        //        this.servicesdata = response.data;
        //    },
        //    (error) => this.errorMessage = <any>error
        //    );

        this.service.get(`/Configurator/get_windows_services?deviceId=${this.deviceId}`)
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                    this.data.pageSize = 50;
                    var groupDesc = new wijmo.collections.PropertyGroupDescription('server_required');
                    this.data.groupDescriptions.push(groupDesc);

                } else {
                    this.appComponentService.showErrorMessage(response.message);
                }

            }, error => {
                var errorMessage = <any>error;
                this.appComponentService.showErrorMessage(errorMessage);
            });
    }

    buildPostData() {
        var listOfServiceNames = [];
        if (this.flex.collectionView) {
            if (this.flex.collectionView.items.length > 0) {
                for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
                    if (item.monitored)
                        listOfServiceNames.push(item.service_name);
                }
            }
        }

        var postData = {
            "setting": listOfServiceNames,
            "devices": [ this.deviceId ]
        };

        //this.saveGridRow('/configurator/save_server_tasks', postData, dlg)
        this.service.put('/Configurator/save_windows_services', postData)
            .subscribe(
            response => {
                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });

    }
}



