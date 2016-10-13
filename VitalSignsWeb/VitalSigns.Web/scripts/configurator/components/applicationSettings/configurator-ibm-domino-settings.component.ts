import {Component, OnInit, AfterViewInit, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

@Component({
    selector: 'ibmDominoSettings-form',
    templateUrl: '/app/configurator/components/applicationSettings/configurator-ibm-domino-settings.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class IbmDominoSettingsForm implements OnInit, AfterViewInit {

    @ViewChildren('name') inputName;

    insertMode: boolean = false;
    ibmDominoSettingsForm: FormGroup;
    errorMessage: string;
    profileEmail: string;
    formTitle: string;

    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute) {

        this.ibmDominoSettingsForm = this.formBuilder.group({
            'notes_program_directory': [''],
            'notes_user_id': [''],
            'notes_ini': [''],
            'notes_password': [''],
            'enableex_journal': [''],
            'enable_domino_console_commands': [''],
            'exjournal_threshold': [''],
            'consecutive_telnet': ['']
            //'id': [''],
            //'name': [''],
            //'value': [''],
            //'category': ['']
          

        });

    }

    ngOnInit() {
      
       // alert("Hi");
        this.route.params.subscribe(params => {

            this.profileEmail = params['email'];
            
           
            this.formTitle = "IBM Domino Settings";
          //  this.dataProvider.get('/services/get_ibm_domino_settings')
                this.dataProvider.get('/services/get_ibm_domino_settings')
                    .subscribe(
                    (data) => this.ibmDominoSettingsForm.setValue(data.data),
                    (error) => this.errorMessage = <any>error
                   
                    );

              
        });

    }

    ngAfterViewInit() {

    

    }

    onSubmit(nameValue: any): void {

        this.dataProvider.put(
            '/services/get_ibm_domino_settings',
            nameValue,
            () => this.router.navigate(['profiles']));

    }
}