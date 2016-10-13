import {Component, OnInit, AfterViewInit, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

@Component({
    selector: 'ibmDominoSettings-form',
    templateUrl: '/app/profiles/components/configurator-ibm-domino-settings.component.html',
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
            'name': ['', Validators.required],
            'value': ['', Validators.required],
        });

    }

    ngOnInit() {

        this.route.params.subscribe(params => {

            this.profileEmail = params['email'];

           
                this.formTitle = "IBM Domino Settings";
                this.dataProvider.get('/services/get_ibm_domino_settings')
                    .subscribe(
                    (data) => this.ibmDominoSettingsForm.setValue(data),
                    (error) => this.errorMessage = <any>error
                    );

               
        });

    }

    ngAfterViewInit() {

        if (this.insertMode)
            this.inputName.first.nativeElement.focus();

    }

    onSubmit(profile: any): void {

        this.dataProvider.put(
            `http://localhost:1234/profiles/${profile.email}`,
            profile,
            () => this.router.navigate(['profiles']));

    }
}