import {Component, Output, EventEmitter, OnInit}  from '@angular/core';
import {RESTService} from '../../core/services';
@Component({
    selector: 'search-server-list',
    templateUrl: '/app/services/components/search-server-list.component.html',
    providers: [
        RESTService
    ]
})
export class SearchServerList implements OnInit  {
    @Output() name = new EventEmitter();
    @Output() type = new EventEmitter();
    @Output() status = new EventEmitter();
    @Output() location = new EventEmitter();
    deviceTypeData: any;
    deviceStatusData: any;
    deviceLocationData: any;
    errorMessage: any;

    constructor(private service: RESTService) { }


    deviceName: string = "";
    deviceType: string = "-All-";
    deviceLocation: string = "-All-";
    deviceStatus: string = "-All-";
    changeDeviceName() {
        if (!this.deviceName) {
            this.deviceName = '';
        }      
        this.name.emit(this.deviceName);
    }
    selectDeviceType() {
        if (!this.deviceType) {
            this.deviceType = '';
        }
        this.type.emit(this.deviceType);
    }
    selectDeviceStatus() {
        if (!this.deviceStatus) {
            this.deviceStatus = '';
        }
        this.status.emit(this.deviceStatus);
    }
    selectDeviceLocation() {
        if (!this.deviceLocation) {
            this.deviceLocation = '';
        }
        this.location.emit(this.deviceLocation);
    }
    ngOnInit() {
        this.name.emit('');
        this.type.emit('-All-');
        this.status.emit('-All-');
        this.location.emit('-All-');
        this.service.get('/services/server_list_selectlist_data')
            .subscribe(
            (response) => {
                this.deviceTypeData = response.data.deviceTypeData;
                this.deviceStatusData = response.data.deviceStatusData;
                this.deviceLocationData = response.data.deviceLocationData;                
            },
            (error) => this.errorMessage = <any>error
            );
    }
}
