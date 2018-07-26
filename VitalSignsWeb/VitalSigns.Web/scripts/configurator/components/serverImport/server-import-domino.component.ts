import {Component, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {HttpModule, Headers, RequestOptions}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {Router, ActivatedRoute} from '@angular/router';
import { AppComponentService } from '../../../core/services';

@Component({
    templateUrl: '/app/configurator/components/serverImport/server-import-domino.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class DominoServerImport implements OnInit{
    @ViewChild('location') location: wijmo.input.ComboBox;
    dominoServerImportData: any;
    dominoServer: string;
    errorMessage: any;
    selectedLocation: string;
    deviceLocationData: any;
    postData: any;
    scanSettings: string = "Scan Settings";
    mailSettings: string = "Mail Settings";
    currentStep: string = "1";
    public formData = new FormData();
    public file: any;
    public url: string;
    selectedFiles: any;
    isSelected: any;
    selObj: { isChecked: false };
    loading = false;

    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute,
        appComponentService: AppComponentService) {
        this.url = '/configurator/upload_file';
        this.isSelected = [
            { isChecked: false }
        ];
        this.selObj = { isChecked: false };
    }
    ngOnInit() {
        this.dataProvider.get('/configurator/get_domino_import')
            .subscribe(
            (response) => {
                this.dominoServerImportData = response.data;
            },
            (error) => this.errorMessage = <any>error
            );
    }
    loadServers(): void {   
        this.errorMessage = "";
        this.loading = true;
        this.dataProvider.put('/configurator/load_domino_servers', this.dominoServerImportData)
            .subscribe(
            response => {
                if (response.status != "Success") {
                    this.errorMessage = response.message;
                    this.loading = false;
                }
                else {
                    this.dominoServerImportData.servers = response.data.serverList;
                    this.deviceLocationData = response.data.locationList;
                    for (let server of this.dominoServerImportData.servers) {
                        server.is_selected = false;
                    }
                    this.loading = false;
                    //this.resize(this.isSelected, this.dominoServerImportData.servers.length, false);
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
        this.dominoServerImportData.location = this.location.selectedItem.value;
    }

     step2Click(): void {
        this.currentStep = "3";
    }

     step3Click(): void {
         this.dataProvider.put('/configurator/save_domino_servers', this.dominoServerImportData)
             .subscribe(
             response => {      
                 if (response.status == "Success") {
                     this.currentStep = "4";
                 }
                 else {
                     //this.appComponentService.showErrorMessage(response.message);
                     this.errorMessage = response.message;
                 }
             },
             (error) => this.errorMessage = <any>error
                
             );
       
     }
     step4Click(): void {
         this.currentStep = "1";
         this.ngOnInit();

     }
     uploadFiles(fileInput: any): void {
         //this.dataProvider.post(this.url, this.formData);
         this.errorMessage = "";
         let headers = new Headers({ 'Content-Type': 'multipart/form-data' });
         let requestOptions = new RequestOptions({ headers: headers });
         
         this.dataProvider.put(this.url, this.formData)
             .subscribe(
             response => {
                 if (response.status != "OK") {
                     this.errorMessage = response.message;
                 }
                 else {
                     this.dominoServerImportData.servers = response.data.serverList;
                     this.deviceLocationData = response.data.locationList;

                 }

             },
             (error) => this.errorMessage = <any>error

         );

         this.formData = null;
     }

     changeListener(fileInput: any) {
         this.postFile(fileInput);

     }
     //send post file to server 
     postFile(inputValue: any): void {
         var formData = new FormData();
         for (let i = 0; i < inputValue.target.files.length; i++) {
             formData.append("file-" + i.toString(), inputValue.target.files[i]);
             this.formData.append("file-" + i.toString(), inputValue.target.files[i]);
         }
     } 

     selectAll() {
         for (let server of this.dominoServerImportData.servers) {
             server.is_selected = true;
         }
     }

     deselectAll() {
         for (let server of this.dominoServerImportData.servers) {
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
}
    