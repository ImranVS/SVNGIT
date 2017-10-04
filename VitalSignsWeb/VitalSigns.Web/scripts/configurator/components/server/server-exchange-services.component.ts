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
        this.initialGridBind(`/Configurator/get_windows_services?deviceId=${this.deviceId}`);
    }

    buildPostData(setting: string, dlg) {
        var postData = {
            "setting": setting,

        };
        this.currentForm.setValue(postData);

        //this.saveGridRow('/configurator/save_server_tasks', postData, dlg)
        this.service.put('/Configurator/save_domino_server_tasks', postData)
            .subscribe(
            response => {

            });

    }
}



