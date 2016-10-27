import {Component, AfterViewChecked, OnInit, Input} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {Router} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
declare var injectSVG: any;


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
  
    deviceId: any;
    ServerAttribute: FormGroup;
    errorMessage: any;
    category: string;
    type: string;
    defaultvalue: string;
    FieldName: string;
    Attributes: any[];
    scanSettings:string ="Scan Settings"

    constructor(
        private formBuilder: FormBuilder,
        private Attribute: RESTService,
        private router: Router,
        private route: ActivatedRoute) {
        this.ServerAttribute = this.formBuilder.group({
            'id': [],
            'DefaultValue':[]

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
                console.log(response.data);
                this.Attributes = response.data;
               
            },
            error => this.errorMessage = <any>error


            );


    }
    onSubmit(serverattributes: any): void {

        this.Attribute.put('/Configurator/save_servers_attributes', serverattributes)
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