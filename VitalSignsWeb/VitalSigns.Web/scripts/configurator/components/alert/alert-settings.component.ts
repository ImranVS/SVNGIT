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
            'enable_persitent_alerting': [false],
            'alert_interval': [''],
            'alert_duration': [''], 
        });
    }
    ngOnInit() {

        //    this.dataProvider.get('/Configurator/get_alertsettings')
        //        .subscribe(
        //        response => {
        //            console.log(response.data);
        //            this.preferencesForm.setValue(response.data);
        //        },
        //        (error) => this.errorMessage = <any>error

        //        );


        //});

    }

    onSubmit(nameValue: any): void {

        this.dataProvider.put('/Configurator/save_alertsettings', nameValue)
            .subscribe(
            response => {

            });
    }
}
