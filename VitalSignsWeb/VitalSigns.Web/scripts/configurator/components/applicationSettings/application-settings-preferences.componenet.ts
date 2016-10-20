import {Component, OnInit, AfterViewInit, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

@Component({
    selector: 'preferences-form',
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-preferences.componenet.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class PreferencesForm implements OnInit, AfterViewInit {

    @ViewChildren('name') inputName;

    insertMode: boolean = false;
    preferencesForm: FormGroup;
    errorMessage: string;
    profileEmail: string;
    formTitle: string;

    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute) {

        this.preferencesForm = this.formBuilder.group({
            'ibm_connections': [''],
            'ibm_domino': [''],
            'ibm_notes_mail': [''],
            'ibm_sametime': [''],
            'ibm_websphere': [''],
            'microsoft_active_directory': [''],
            'microsoft_exchange': [''],
            'microsoft_sharepoint': [''],
            'microsoft_skype_for_business': [''],
            'microsoft_office365': [''],
            'disabled': [''],
            'company_name': [''],
            'currency_symbol': [''],
            'monitoring_delay': [''],
            'threshold_show': [''],
            'dashboard_only': [false],
            'bing_key': ['']
        });

    }

    ngOnInit() {


        this.route.params.subscribe(params => {



            this.formTitle = "Preferences";

            this.dataProvider.get('/configurator/preferences')
                .subscribe(
                (data) => this.preferencesForm.setValue(data.data),
                (error) => this.errorMessage = <any>error

                );
        });
    }

    ngAfterViewInit() {

    }

    onSubmit(nameValue: any): void {

        this.dataProvider.put(
            '/configurator/save_preferences',
            nameValue);

    }
}