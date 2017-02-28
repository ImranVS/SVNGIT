import {Component, AfterViewChecked, OnInit, Input, ViewChildren} from '@angular/core'
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';

import {ActivatedRoute} from '@angular/router';
import {Router} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {ServicesViewService} from '../services/services-view.service';
import * as helpers from '../../core/services/helpers/helpers';

import {RESTService} from '../../core/services';

declare var injectSVG: any;

@Component({
    templateUrl: '/app/services/components/services-view.component.html',
    providers: [
        HttpModule,
        RESTService,
        ServicesViewService,
        helpers.DateTimeHelper
    ]
})
export class ServicesView implements OnInit, AfterViewChecked {

  //  @ViewChildren('deviceName') deviceName;
    filterCount = 0;
    _searchText: string;
    serversData: FormGroup;
    @Input('searchText')
    public set searchText(val: string) {
        this._searchText = val;
        this.filterCount = 0;
    }
    public get searchText():string {
        return this._searchText ;
    }
    
    _searchType: string;
    @Input('searchType')
    public set searchType(val: string) {
        this._searchType = val;
        this.filterCount = 0;
    }
    public get searchType(): string {
        return this._searchType;
    }


    _searchStatus: string;
    @Input('searchType')
    public set searchStatus(val: string) {
        this._searchStatus = val;
        this.filterCount = 0;
    }
    public get searchStatus(): string {
        return this._searchStatus;
    }
      
    _searchLocation: string;
    @Input('searchType')
    public set searchLocation(val: string) {
        this._searchLocation = val;
        this.filterCount = 0;
    }
    public get searchLocation(): string {
        return this._searchLocation;
    } 

    errorMessage: string;
    module: string;
    status: string;
    services: any[];
    addServersForm: FormGroup;
    deviceLocationData: any;
    selectedLocation: string;
    selectedType: string;
    Type: string;
    selectedBusinessHour: string;
    devicebusinessHourData: any;
     deviceTypeData:any;
     postData: any;
     url: boolean;
     modal = true;
     constructor(
         private formBuilder: FormBuilder,
         private service: RESTService,
         private router: Router,
         private route: ActivatedRoute,
         private servicesViewService: ServicesViewService,
         private datetimeHelpers: helpers.DateTimeHelper
     ) {

        this.addServersForm = this.formBuilder.group({
            'device_name': ['', Validators.required],
            'device_type': ['', Validators.required],
            'description': ['', Validators.required],
            'location_id': ['', Validators.required],
            'ip_address': ['', Validators.required],
            'business_hours_id': ['', Validators.required],
            'monthly_operating_cost': [''],
            'ideal_user_count': [''],
            'category':['']

        });

        this.servicesViewService.registerServicesView(this);

    }

    selectService(service: any) {

        // Activate selected tab
        this.services.forEach(service => service.active = false);
        service.active = true;  
        
        this.router.navigate(['services/' + this.module,  service.id ]);
    }
    setFilterCount(index: any) {
        this.filterCount= index+1;
    }
    onServerTypeSelectedIndexChanged(event: wijmo.EventArgs, deviceType: wijmo.input.ComboBox) {
      
        this.Type = deviceType.selectedValue;
    }

  
    loadData() {
        this.service.get('/Services/device_list')
            .subscribe(
            response => {
                if (this.module == "dashboard") {                  
                    var value = response.data.filter((item) => item.is_enabled == true);
                    this.services = value;
                }
                else {
                    var value = response.data.filter((item) => item.type != 'NotesMail Probe');
                    this.services = value;
                  
                } 
                this.services = this.datetimeHelpers.toLocalDateTime(this.services);
            },
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.service.get('/Configurator/get_server_credentials_businesshours')
            .subscribe(
            (response) => {

                this.deviceLocationData = response.data.locationsData;
                this.deviceTypeData = response.data.serverTypeData;
                //console.log("csdcsdc");
                //console.log(response.data.serverTypeData);
                this.devicebusinessHourData = response.data.businessHoursData;
            },
            (error) => this.errorMessage = <any>error
            );
        this.route.params.subscribe(params => {
            this.module = params['module'];
            this.loadData(); 
        });
        
    }

    ngAfterViewChecked() {
        injectSVG();
    }
    onSubmit(addServer: any, dialog: wijmo.input.Popup) {
        this.service.put('/Configurator/save_servers', addServer)
            .subscribe(
            response => {              
                this.addServersForm.reset();
                this.router.navigate(['services/' + this.module, response.data]);
            });
        this.addServersForm.reset();
        dialog.hide();
        
    }
    addServer(dlg: wijmo.input.Popup) {
        if (dlg) {
            dlg.modal = this.modal;
            dlg.hideTrigger = dlg.modal ? wijmo.input.PopupTrigger.None : wijmo.input.PopupTrigger.Blur;
            dlg.show();

        }
    }
}