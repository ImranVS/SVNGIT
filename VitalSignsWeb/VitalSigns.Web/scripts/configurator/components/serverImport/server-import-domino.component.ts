import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {Router, ActivatedRoute} from '@angular/router';


@Component({
    templateUrl: '/app/configurator/components/serverImport/server-import-domino.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class DominoServerImport implements OnInit{
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
    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute) {
        this.url = '/configurator/upload_file';
    }
    ngOnInit() {
        this.dataProvider.get('/configurator/get_domino_import')
            .subscribe(
            (response) => {
                this.dominoServerImportData = response.data;
                console.log(this.dominoServerImportData);
            },
            (error) => this.errorMessage = <any>error
            );
    }
    loadServers(): void {       
        this.dataProvider.put('/configurator/load_domino_servers', this.dominoServerImportData)
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
      
    }
    step1Click(): void {
        this.currentStep = "2";
    }

     step2Click(): void {
        this.currentStep = "3";
    }

     step3Click(): void {
         this.dataProvider.put('/configurator/save_domino_servers', this.dominoServerImportData)
             .subscribe(
             response => {      
                     this.currentStep = "4";
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
         console.log('uploading...');
         this.postFile(fileInput);

     }
     //send post file to server 
     postFile(inputValue: any): void {
         console.log(this.url);
         var formData = new FormData();
         for (let i = 0; i < inputValue.target.files.length; i++) {
             formData.append("file-" + i.toString(), inputValue.target.files[i]);
             this.formData.append("file-" + i.toString(), inputValue.target.files[i]);
             console.log(inputValue.target.files[i]);
         }

         // this.service.post(this.url, formData);
     }

   
}