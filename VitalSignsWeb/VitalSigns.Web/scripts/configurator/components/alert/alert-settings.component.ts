import {Component, OnInit, AfterViewInit, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

@Component({
    templateUrl: '/app/configurator/components/alert/alert-settings.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class AlertSettings implements OnInit {
    @ViewChildren('name') inputName;
    insertMode: boolean = false;
    alertSettings: FormGroup;
    errorMessage: string;
    profileEmail: string;
    formTitle: string;

    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute) { 

        this.alertSettings = this.formBuilder.group({
            'primary_host_name': [''],
            'primary_from': [''],
            'primary_user_id': [''], 
            'primary_port': [''],
            'primary_auth': [''],
            'primary_ssl': [''], 
            'primary_pwd': [''], 
            'secondary_host_name': [''],
            'secondary_from': [''],
            'secondary_user_id': [''], 
            'secondary_pwd': [''],
            'secondary_port': [''],
            'secondary_auth': [''], 
            'secondary_ssl': [''],
            'sms_account_sid': [''],
            'sms_auth_token': [''], 
            'sms_from': [''],
            'enable_persitent_alerting': [''],
            'alert_interval': [''],
            'alert_duration': [''],
            //'e_mail': [''],
            //'enable_alert_limits': [''],
            //'total_maximum_alerts_per_definition': [''],
            //'total_maximum_alerts_per_day': [''],
            //'enable_SNMP_traps': [''],
            //'host_name': [''],
            'alert_about_recurrences_only': [''],
            'number_of_recurrences': ['']
        });
    }
    ngOnInit() {

        this.route.params.subscribe(params => {



           
            this.dataProvider.get('/Configurator/get_alertsettings')
                .subscribe(
                (data) => this.alertSettings.setValue(data.data),
                (error) => this.errorMessage = <any>error

                );


        });


    }

    onSubmit(nameValue: any): void {

        this.dataProvider.put('/Configurator/save_alertsettings', nameValue)
            .subscribe(
            response => {

            });
    }
}
