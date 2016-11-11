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
    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute) {
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

   
}