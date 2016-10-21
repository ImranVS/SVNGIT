import {Component, OnInit, ViewChildren} from '@angular/core';
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
export class PreferencesForm implements OnInit {

    @ViewChildren('licencekey') licencekey;

    insertMode: boolean = false;
    preferencesForm: FormGroup;
    errorMessage: string;
    profileEmail: string;
    licenceKey: string;   
    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute) {

        this.preferencesForm = this.formBuilder.group({
            'company_name': ['', Validators.required],
            'currency_symbol': ['', Validators.required],
            'monitoring_delay': ['', Validators.required],
            'threshold_show': ['', Validators.required],
            'dashboardonly_exec_summary_buttons': [false],
            'bing_key': ['']
        });
    }

    ngOnInit() {
        this.dataProvider.get('/configurator/get_preferences')
            .subscribe(
            response => {
                console.log(response.data);
                this.preferencesForm.setValue(response.data);
            },
            (error) => this.errorMessage = <any>error

            );
    }

    onSubmit(nameValue: any): void {
        this.dataProvider.put('/configurator/save_preferences', nameValue)
            .subscribe(
            response => {});
    }

    saveLicence(dialog: wijmo.input.Popup) {
        var licencekey=this.licencekey.first.nativeElement.value;
        if (licencekey == "") {
            alert("error");
        } else
        {
            this.dataProvider.get('/configurator/save_licence/' + licencekey)
                .subscribe(
                response => {
                    this.licencekey.first.nativeElement.value = "";
                });
            dialog.hide();
        }
    }
}