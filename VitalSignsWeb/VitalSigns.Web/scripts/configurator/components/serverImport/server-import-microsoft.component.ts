import {Component, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {Router, ActivatedRoute} from '@angular/router';
import { AppComponentService } from '../../../core/services';

@Component({
    templateUrl: '/app/configurator/components/serverImport/server-import-microsoft.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class MicrosoftServerImport implements OnInit{
    @ViewChild('location') location: wijmo.input.ComboBox;
    @ViewChild('primaryExchangeServer') primaryExchangeServer: wijmo.input.ComboBox;
    @ViewChild('backupExchangeServer') backupExchangeServer: wijmo.input.ComboBox;
    exchangeServerImportData: any;
    errorMessage: any;
    authentication_type: any;
    protocol: any;
    deviceLocationData: any;
    exchangeservers: any;
    exchangeservers2: any;
    device_type: any;
    addCredentialForm: FormGroup;
    scanSettings: string = "Scan Settings";
    mailSettings: string = "Mail Settings";
    dagSettings: string = "DAG Settings";
    currentStep: string = "1";
    public formData = new FormData();
    isSelected: any;
    selObj: { isChecked: false };
    loading = false;
    modal = true;
    serverType: string;
    serverAttributes: any[];
    deviceId: any;
    deviceCredentialData: any;   
    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private Attribute: RESTService,
        private router: Router,
        private route: ActivatedRoute,
        private appComponentService: AppComponentService) {
        this.isSelected = [
            { isChecked: false }
        ];
        this.selObj = { isChecked: false };
        this.addCredentialForm = this.formBuilder.group({
            'alias': ['', Validators.required],
            'user_id': ['', Validators.required],
            'password': ['', Validators.required],
            'device_type': [''],
            'confirm_password': [''],
            'id': [''],
            'is_modified': ['']

        });
    }
    ngOnInit() {
        this.dataProvider.get(`/configurator/get_microsoft_import?device_type=${this.device_type}`)
            .subscribe(
            (response) => {
                this.exchangeServerImportData = response.data;
            },

            (error) => this.errorMessage = <any>error
            );
        this.Attribute.get('/configurator/get_credentials')
            .subscribe(
            response => {
                this.deviceCredentialData = response.data;
            },
            error => this.errorMessage = <any>error
            );
    }

    loadServers(): void {   
        this.errorMessage = "";
        this.loading = true;
        this.exchangeServerImportData.device_type = this.device_type;
            this.dataProvider.put('/configurator/load_microsoft_servers', this.exchangeServerImportData)
            .subscribe(
            response => {
                if (response.status != "Success") {
                    this.errorMessage = response.message;
                    this.loading = false;
                }
                else {
                    this.exchangeServerImportData.servers = response.data.servers;
                    this.deviceLocationData = response.data.locationList;
                    this.exchangeServerImportData.device_attributes = response.data.device_attributes;
                    if (this.device_type == "Database Availability Group") {
                        this.exchangeservers = response.data.exchange_List;
                        this.exchangeservers2 = response.data.exchange_List.slice(0);
                    }
                    for (let server of this.exchangeServerImportData.servers) {
                        server.is_selected = false;
                    }
                    this.loading = false;
                }

            },
            (error) => {
                this.errorMessage = <any>error;
                this.loading = false;
            }
                
            );
      
    }
    
    step1Click(): void {
        this.currentStep = "2";
        this.exchangeServerImportData.location = this.location.selectedItem.value;
    }

    step2Click(): void {
       
        this.dataProvider.put(`/configurator/save_microsoft_servers?device_type=${this.device_type}`, this.exchangeServerImportData)
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.currentStep = "3";
                }
                else {
                    this.errorMessage = response.message;
                }
            },
            (error) => this.errorMessage = <any>error

            );
        this.exchangeServerImportData.primary_server_id = this.primaryExchangeServer.selectedValue;
        this.exchangeServerImportData.backup_server_id = this.backupExchangeServer.selectedValue;
    }

    step3Click(): void {
        
        this.errorMessage = "";
        this.exchangeServerImportData.servers = null;
        this.deviceLocationData = [];
        this.exchangeServerImportData.device_attributes = [];
        this.exchangeservers = [];
        this.exchangeservers2 = [];
        this.currentStep = "1"; 
     }
     selectAll() {
         for (let server of this.exchangeServerImportData.servers) {
             server.is_selected = true;
         }
     }

     deselectAll() {
         for (let server of this.exchangeServerImportData.servers) {
             server.is_selected = false;
         }
     }

     isTrue(value) {
         if (typeof (value) == 'string') {
             value = value.toLowerCase();
         }
         switch (value) {
             case true:
             case "true":
             case 1:
             case "1":
             case "on":
             case "yes":
                 return true;
             default:
                 return false;
         }
     }


     addCrdential(dlg: wijmo.input.Popup) {
         if (dlg) {
             dlg.modal = this.modal;
             dlg.hideTrigger = dlg.modal ? wijmo.input.PopupTrigger.None : wijmo.input.PopupTrigger.Blur;
             dlg.show();
             this.serverType = this.serverAttributes["device_type"];

         }
     }
     SaveCredential(addCrdential: any, dialog: wijmo.input.Popup) {
         addCrdential.device_type = this.serverType;
         addCrdential.confirm_password = "";
         addCrdential.id = null;
         addCrdential.is_modified = false;
         this.Attribute.put('/Configurator/save_credentials', addCrdential)
             .subscribe(

             response => {

                 if (response.status == "Success") {

                     this.appComponentService.showSuccessMessage(response.message);
                     this.addCredentialForm.reset();
                 } else {

                     this.appComponentService.showErrorMessage(response.message);
                 }
                 this.Attribute.get('/configurator/get_credentials')
                     .subscribe(
                     response => {
                         this.deviceCredentialData = response.data;
                     },
                     error => this.errorMessage = <any>error
                     );
             });
        

         dialog.hide();

     }
}
    