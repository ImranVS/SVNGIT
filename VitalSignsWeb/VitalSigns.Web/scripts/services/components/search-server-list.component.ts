import {Component, Output, EventEmitter, OnInit}  from '@angular/core';
import { ActivatedRoute, Router, NavigationStart, NavigationEnd} from '@angular/router';
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

    constructor(private service: RESTService, private route: ActivatedRoute, private router: Router) { }
    
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


    refreshSearchFields() {
        let paramstatus = null;
        let paramtype = null;
        let paramName = null;
        let module = null;
        //Get a query parameter if the page is called from the main dashboard via a link in a status component
        this.route.queryParams.subscribe(params => [paramstatus = params['status'] || '-All-',
        paramtype = params['type'] || '-All-', paramName = params['devicename'] || '']);
        this.name.emit('');
        this.type.emit(paramtype);
        this.status.emit(paramstatus);
        this.location.emit('-All-');
        this.name.emit(paramName);
        //Set a selected value of the Status drop down box to the passed query parameter or -All- if no parameter is available
        this.deviceType = paramtype;
        this.deviceStatus = paramstatus;
        this.deviceName = paramName;
    }

    ngOnInit() {
        
        this.service.get('/services/server_list_selectlist_data')
            .subscribe(
            (response) => {
                this.deviceTypeData = response.data.deviceTypeData;
                this.deviceStatusData = response.data.deviceStatusData;
                this.deviceLocationData = response.data.deviceLocationData;                
            },
            (error) => this.errorMessage = <any>error
        );
        this.refreshSearchFields();

        this.router.events.subscribe(event => {
            if (event instanceof NavigationEnd) {
                this.refreshSearchFields();
            }
        });

    }
}
