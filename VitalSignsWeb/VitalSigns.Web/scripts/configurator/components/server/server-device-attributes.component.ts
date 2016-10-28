import {Component, AfterViewChecked, OnInit, Input} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {Router} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
declare var injectSVG: any;
import {DeviceAttributeValue} from '../../models/device-attribute';


@Component({
    selector: 'server-device-attribute',
    templateUrl: '/app/configurator/components/server/server-device-attributes.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
//export class ServerDiskSettings implements OnInit, AfterViewInit {
export class ServerAttribute implements OnInit, AfterViewChecked {
    devices: string;
    deviceId: any;
    ServerAttributeForm: FormGroup;
    errorMessage: any;
    category: string;
    type: string;
    defaultvalue: string;
    fieldName: string;
    serverAttributes: any[];
    attributes: any[];
    scanSettings: string = "Scan Settings"
    mailSettings: string = "Mail Settings"
    travelerSettings: string = "Traveler Settings"
    optionalSettings: string ="Optional Settings"


    constructor(
        private formBuilder: FormBuilder,
        private Attribute: RESTService,
        private router: Router,
        private route: ActivatedRoute) {
        this.ServerAttributeForm = this.formBuilder.group({
            
            'setting': [''],
            'value': [''],
            'devices': ['']

        });

    }

    ngOnInit() {
       

        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
            this.loadData();
        });
    }

    loadData() {

        this.Attribute.get('/configurator/' + this.deviceId + '/servers_attributes')
            .subscribe(
            response => {
                this.serverAttributes = response.data;             
                //this.attributes = response.data.device_attributes;
              
            },
            error => this.errorMessage = <any>error


            );


    }
   
    applySetting() {
  
        var postData = {
            "setting": "",
            "value": this.serverAttributes,
            "devices": ""
        };
       
        this.ServerAttributeForm.setValue(postData);
        console.log(postData);
        this.Attribute.put('/configurator/save_servers_attributes/' + this.deviceId, postData)
            .subscribe(
            response => {

            });
    }


    ngAfterViewChecked() {
       // injectSVG();
    }


    //changeInDevices(server: string) {
    //    this.devices = server;

}