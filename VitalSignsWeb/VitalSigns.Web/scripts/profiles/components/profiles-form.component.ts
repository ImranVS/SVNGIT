import {Component, OnInit, AfterViewInit, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../core/services';

@Component({
    selector: 'profiles-form',
    templateUrl: '/app/profiles/components/profiles-form.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ProfilesForm implements OnInit, AfterViewInit {

    @ViewChildren('name') inputName;

    insertMode: boolean = false;
    profileForm: FormGroup;
    errorMessage: string;
    profileEmail: string;
    formTitle: string;

    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute) {

        this.profileForm = this.formBuilder.group({
            'name': ['', Validators.required],
            'email': ['', Validators.required],
        });

    }

    ngOnInit() {

        this.route.params.subscribe(params => {

            this.profileEmail = params['email'];

            if (!this.profileEmail) {
                this.insertMode = true;
                this.formTitle = "Create profile";
            }
            else {
                this.formTitle = "Edit profile";
                this.dataProvider.get(`http://localhost:1234/profiles/${this.profileEmail}`)
                    .subscribe(
                    (data) => this.profileForm.setValue(data),
                    (error) => this.errorMessage = <any>error
                    );
            }

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