import {Component, OnInit, AfterViewInit, ViewChildren,Output, EventEmitter,} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/serverSettings/server-locations-credentials-businesshours.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServerLocations implements OnInit, AfterViewInit {

    @ViewChildren('name') inputName;
    @Output() location = new EventEmitter();
    @Output() credential = new EventEmitter();
    @Output() businessHour = new EventEmitter();
    insertMode: boolean = false;
    ibmDominoSettingsForm: FormGroup;
    errorMessage: string;
    profileEmail: string;
    formTitle: string;
    deviceLocationData: any;
    deviceCredentialData: any;
    devicebusinessHourData: any;
    credentials: any;
    businessHours: any;
    deviceLocation: string = "-All-";
    deviceCredential: string = "-All-";
    devicebusinessHour: string = "-All-";
    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute) {

        this.location.emit('-All-');
        this.credential.emit('-All-');
        this.businessHour.emit('-All-');
      

        this.dataProvider.get('/Configurator/get-server-credentials-businesshours')
            .subscribe(
            (response) => {

                this.deviceLocationData = response.data.locationsData;
                this.deviceCredentialData = response.data.credentialsData;
                this.devicebusinessHourData = response.data.businessHoursData;
            },
            (error) => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
      
     
     

    }

    ngAfterViewInit() {

    }

   
}