import {Component, OnInit, AfterViewInit, ViewChild, Output, EventEmitter} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {GridBase} from '../../../core/gridBase';
import {RESTService} from '../../../core/services';
import {DiskSttingsValue} from '../../models/server-disk-settings';

import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/server/server-maintenance-windows.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper
    ]
})
//export class ServerDiskSettings implements OnInit, AfterViewInit {
export class MaintenanceWindows implements OnInit {
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
     @ViewChild('flex') flex: wijmo.grid.FlexGrid;

    constructor(
        private dataProvider: RESTService,
        private formBuilder: FormBuilder, private route: ActivatedRoute, private datetimeHelpers: helpers.DateTimeHelper) {

        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.dataProvider.get('/Configurator/get_server_maintenancedata/' + this.deviceId +'')
            .subscribe(
            response => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                this.data.pageSize = 10;

            });

       
    }

 ngOnInit() {
       
    }

   get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }
   
}