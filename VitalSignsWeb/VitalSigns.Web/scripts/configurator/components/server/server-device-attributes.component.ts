import {Component, AfterViewChecked, OnInit, Input} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {Router} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
declare var injectSVG: any;
import {DeviceAttributeValue} from '../../models/device-attribute';
import {AppComponentService} from '../../../core/services';



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
    selectedplatform: any;
    selectedIbmDb2Credential: string;
    selectedCredential: string;
    selectedPlatform: string;
    deviceCredentialData: any;
    attributes: any[];
    modal = true;
    addCredentialForm: FormGroup;
    serverType: string;
    Types: string;
    platform: string;
    costAttr: string = "Cost Settings";
    scanSettings: string = "Scan Settings";
    mailSettings: string = "Mail Settings";
    travelerSettings: string = "Traveler Settings";
    optionalSettings: string = "Optional Settings";
    webSphereSettings: string = "WebSphere Settings";
    chatSettings: string = "Chat Settings";
    searchText: string = "Optional Search Text";
    usernameorPassword: string = "Optional Username/Password";
    visiblity: boolean;
    documentschecked: boolean;
    module: any;

 
    constructor(
        private formBuilder: FormBuilder,
        private Attribute: RESTService,
        private router: Router,
        private route: ActivatedRoute,
        private appComponentService: AppComponentService) {
        this.ServerAttributeForm = this.formBuilder.group({

            'setting': [''],
            'value': [''],
            'devices': ['']

        });
        this.addCredentialForm = this.formBuilder.group({
            'alias': ['', Validators.required],
            'user_id': ['', Validators.required],
            'password': ['', Validators.required],
            'device_type': [''],
            'confirm_password': [''],
            'id': [''],
            'is_modified': ['']

        });
        this.appComponentService = appComponentService;
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
                this.serverAttributes = response.data.serverresult;
                this.deviceCredentialData = response.data.credentialsData;
               // this.platform = response.data.serverresult.platform;
                //  this.selectedplatform = response.data.platform;
                //this.attributes = response.data.device_attributes;
            },
            error => this.errorMessage = <any>error


            );


    }

    adddominoCredentials(dlg: wijmo.input.Popup) {
        if (dlg) {
            dlg.modal = this.modal;
            dlg.hideTrigger = dlg.modal ? wijmo.input.PopupTrigger.None : wijmo.input.PopupTrigger.Blur;
            dlg.show();
            this.serverType = "Domino"

        }
    }
    addSametimeCredentials(dlg: wijmo.input.Popup) {
        if (dlg) {
            dlg.modal = this.modal;
            dlg.hideTrigger = dlg.modal ? wijmo.input.PopupTrigger.None : wijmo.input.PopupTrigger.Blur;
            dlg.show();
            this.serverType = "Sametime"

        }
    }

    SaveCredential(addCrdential: any, dialog: wijmo.input.Popup) {
        if (this.serverType == "Domino")
            addCrdential.device_type = "Domino";
        else if (this.serverType = "Sametime")
            addCrdential.device_type = "Sametime";
        addCrdential.confirm_password = "";
        addCrdential.id = null;
        addCrdential.is_modified = true;
        this.Attribute.put('/Configurator/save_credentials', addCrdential)
            .subscribe(

            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);
                    this.addCredentialForm.reset();
                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
        this.Attribute.get('/configurator/' + this.deviceId + '/servers_attributes')
            .subscribe(
            response => {           
                this.deviceCredentialData = response.data.credentialsData;       
            },
            error => this.errorMessage = <any>error


            );

        dialog.hide();

    }

    isChecked(ischecked: boolean) {
        this.documentschecked = ischecked;
    }

    handleClick(index: any) {     
        this.platform = index;
        if (index == "WebSphere") {
            this.platform = "Domino";
        }
        else {
            this.platform = "WebSphere";
        }
        this.router.navigateByUrl('/services/configurator/' + this.deviceId +'?platform=' + this.platform);
    }


    applySetting() {

        var postData = {
            "setting": "",
            "value": this.serverAttributes,
            "devices": ""
        };

        this.ServerAttributeForm.setValue(postData);
        this.Attribute.put('/configurator/save_servers_attributes/' + this.deviceId, postData)
            .subscribe(
            response => {
                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
    }


    ngAfterViewChecked() {
        // injectSVG();
    }


    //changeInDevices(server: string) {
    //    this.devices = server;

}
