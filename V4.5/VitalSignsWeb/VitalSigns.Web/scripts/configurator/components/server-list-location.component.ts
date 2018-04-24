import {Component, OnInit, ViewChild, AfterViewInit, Output, Input, EventEmitter} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../core/services';
import {GridBase} from '../../core/gridBase';
import {ServersLocationService} from './serverSettings/serverattributes-view.service';


@Component({
    selector: 'server-location-list',
    templateUrl: '/app/configurator/components/server-list-location.component.html',
    providers: [
        HttpModule,
        RESTService,
        
           
    ]
})
export class ServersLocation implements OnInit {
   
    @Output() checkedDevices = new EventEmitter();  
    //_deviceList: any;
    _deviceList: string[] = [];
    @Input() deviceType: any; 
    @Input() deviceTypes: any; 
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    devices: string[] = [];
    deviceTypeNames: any;
    rows: wijmo.grid.RowCollection;
    @Input() isVisible: boolean = false;

    @Input() public set deviceList(val: string[]) {
        if (val) {
            if (val.length > 0) {             
                this._deviceList = val;
                this.devices = [];
                this.refreshCheckedDevices();
            }
            else {
                this._deviceList = [];
                this.devices = [];
                this.refreshCheckedDevices();
            }
        }
    }
    constructor(private service: RESTService, private serversLocationsService: ServersLocationService) {
        
           this.serversLocationsService.registerServerLocation(this);
        
    } 
    refreshCheckedDevices() {
        if (this.flex.collectionView) {
            if (this.flex.collectionView.items.length > 0) {
                //(<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
                    if (this._deviceList) {
                        //var value = this._deviceList.filter((record) => record.indexOf(item.id) !== -1);
                        var value = this._deviceList.filter((record) => record == item.id);
                        if (value.length > 0) {
                            item.is_selected = true;
                            this.devices.push(item.id)
                        }
                        else {
                            //this.devices.splice(this.devices.indexOf(value[0]), 1);
                            item.is_selected = false;
                        }
                    }
                    else {
                        item.is_selected = false;
                    }

                }
            }
        }
    }
   
    serverCheck(value,event) {
     
        if (event.target.checked)
        {
            this.devices.push(value);
         //   this.flex.collectionView.currentItem.is_selected = true;
        }
        else {          
            this.devices.splice(this.devices.indexOf(value), 1);
           // this.flex.collectionView.currentItem.is_selected = false;
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
    selectAll() {
        for (var _i = 0; _i < this.flex.collectionView.items.length; _i++) {
            var item = this.flex.collectionView.items[_i];
            this.devices.push(item.id);        
        }  
        this.checkedDevices.emit(this.devices);
        //this.flex.refresh();
    }

    deselectAll() {
        for (var _i = 0; _i < this.flex.collectionView.items.length; _i++) {
            var item = this.flex.collectionView.items[_i];
            //item.is_selected = false;
            //this.flex.collectionView.sourceCollection[_i].is_selected = false;
            this.devices.splice(this.devices.indexOf(item.id), 1);
        }
        this.checkedDevices.emit(this.devices);
        //this.flex.refresh();
    }
    onDeviceListChange(deviceType: string) {
        this.deviceType = deviceType;
        this.service.get("/Configurator/device_list")
            .subscribe(
            response => {
               // this.serversLocationsService.registerServerLocation(this);
                if (this.deviceType) {
                    var resultData = response.data;
                    
                        resultData = resultData.filter((record) => record.device_type == this.deviceType);                        
                    
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(resultData));
                    this.data.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription("location_name"));
                    //this.data.pageSize = 10;
                }
            });
        this.flex.refresh(true);
    }
    ngOnInit() {
       
        this.service.get("/Configurator/device_list")
            .subscribe(
            response => {
                var resultData: any = [];
                var resultDataNew: any = [];
                for (var item of response.data) {
                    if (this._deviceList) {
                        var value = this._deviceList.filter((record) => record.toLocaleLowerCase().indexOf(item.id.toLocaleLowerCase()) !== -1);    
                        if (value != null) {
                            if (value.length > 0) {
                                item.is_selected = true;
                                this.devices.push(item.id)
                            }
                        }
                    } 
                    resultData.push(item);
                } 
                this.deviceList = [];               
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
                //this.data.pageSize = 10;               
            });        
    }  
}



