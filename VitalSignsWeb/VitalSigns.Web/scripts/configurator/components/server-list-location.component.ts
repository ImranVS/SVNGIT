import {Component, OnInit, ViewChild, AfterViewInit, Output, Input, EventEmitter} from '@angular/core';
import { CommonModule } from '@angular/common';EventEmitter
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../core/services';
import {GridBase} from '../../core/gridBase';

import {AppNavigator} from '../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;


@Component({
    selector: 'server-location-list',
    templateUrl: '/app/configurator/components/server-list-location.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServersLocation implements OnInit {
   
    @Output() checkedDevices = new EventEmitter();  
    _deviceList: any;
    @Input() deviceType: any; 
    @Input() deviceTypes: any; 
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    devices: string[] = [];
    deviceTypeNames: any;
    @Input() isVisible: boolean = false;

    @Input() public set deviceList(val: string) {
        this._deviceList = val;
        this.devices = [];
        this.refreshCheckedDevices();  
             
    }
    refreshCheckedDevices() {
        console.log(this._deviceList);
        if (this.flex.collectionView) {
            (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection).pageIndex = 0;
            for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
                var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
                if (this._deviceList) {
                    var value = this._deviceList.filter((record) => record.toLocaleLowerCase().indexOf(item.id.toLocaleLowerCase()) !== -1);
                    if (value.length > 0) {
                        item.is_selected = true;
                        this.devices.push(item.id)
                    }
                    else {
                        this.devices.splice(this.devices.indexOf(value), 1);
                        item.is_selected = false;
                    }
                }
                else {
                    item.is_selected = false;
                }

            }
            
        }
    }
    constructor(private service: RESTService) {
       
    } 
    serverCheck(value,event) {
     
        if (event.target.checked)
        {
            this.devices.push(value);
            this.flex.collectionView.currentItem.is_selected = true;
        }
        else {          
            this.devices.splice(this.devices.indexOf(value), 1);
            this.flex.collectionView.currentItem.is_selected = false;
        }
        this.checkedDevices.emit(this.devices);
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
    onDeviceListChange() {
        console.log(this.deviceType);
    }

    ngOnInit() {
        this.service.get("/Configurator/device_list")
            .subscribe(
            response => {
                console.log("server selector reloaded");
                var resultData: any = [];
                var resultDataNew: any = [];
                for (var item of response.data) {
                    if (this._deviceList) {
                        var value = this._deviceList.filter((record) => record.toLocaleLowerCase().indexOf(item.id.toLocaleLowerCase()) !== -1);                       
                        if (value.length > 0) {
                            item.is_selected = true;
                            this.devices.push(item.id)
                        }
                    } 
                    resultData.push(item);
                } 
                this.deviceList = "";               
                if (this.deviceType) {
                    resultData = resultData.filter((record) => record.device_type == this.deviceType);
                }    
                if (this.deviceTypes) {
                    if (this.deviceTypes != "") {
                        this.deviceTypeNames = this.deviceTypes.split(',');
                    for (var name of this.deviceTypeNames) {
                     resultDataNew = resultDataNew.concat(resultData.filter((record) => record.device_type == name));
                                }
                        resultData = resultDataNew;
                    }

                }
           
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(resultData));
                this.data.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription("location_name"));
                this.data.pageSize = 10;               
            });        
    }  
}



