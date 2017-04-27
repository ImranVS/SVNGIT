import { Component, ViewChildren } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { RESTService } from '../../core/services';
import { AuthenticationService } from '../services/authentication.service';
import {AppComponentService} from '../../core/services';

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
    errorMessage: string;
    appComponentService: AppComponentService;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private authenticationService: AuthenticationService, private service: RESTService, appComponentService: AppComponentService) { this.appComponentService = appComponentService; }

    login() {
        this.emailid = "";
        this.loading = true;
        this.authenticationService.login(this.model.username, this.model.password)
            .subscribe(result => {

                if (result === true) {

                    let referrer = this.route.snapshot.params['ref'];

                    if (referrer)
                        this.router.navigateByUrl(referrer);
                    else
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
            this.appComponentService.showErrorMessage("E-mail is empty");
        }
        else {
            this.service.get(`/Token/reset_password?emailId=${email}`)
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        this.appComponentService.showSuccessMessage("Password has been sent to your e-mail address");
                        this.emailid.first.nativeElement.value = "";
                    }
                    else
                    {
                        this.appComponentService.showErrorMessage("The e-mail does not exist");
                    }
                },
                error => this.errorMessage = <any>error
            );
            dialog.hide();
        }
    }
}