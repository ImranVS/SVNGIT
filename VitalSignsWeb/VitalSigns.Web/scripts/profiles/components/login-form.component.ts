import { Component, ViewChildren } from '@angular/core';
import { Router } from '@angular/router';
import {RESTService} from '../../core/services';
import { AuthenticationService } from '../services/authentication.service';

@Component({
    templateUrl: '/app/profiles/components/login-form.component.html',
    providers: [
        RESTService
    ]
})
export class LoginForm {
    @ViewChildren('emailid') emailid;
    model: any = {};
    loading = false;
    error = '';
    success = '';

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService,private service: RESTService) { }

    login() {

        this.loading = true;
        this.authenticationService.login(this.model.username, this.model.password)
            .subscribe(result => {

                if (result === true) {

                    this.router.navigate(['/']);
                    
                } else {

                    this.error = 'Username or password is incorrect';
                    this.loading = false;

                }
            });
    }
    changePassword(dialog: wijmo.input.Popup) {
        var email = this.emailid.first.nativeElement.value;
        if (email == "") {
            this.error = "Email is empty"
        } else {
            this.service.get(`/Token/reset_password?emailId=${email}`)
                .subscribe(
                response => {
                    this.success = "Password sent to your email..."
                    this.emailid.first.nativeElement.value = "";
                });
            dialog.hide();
        }          
    }

}