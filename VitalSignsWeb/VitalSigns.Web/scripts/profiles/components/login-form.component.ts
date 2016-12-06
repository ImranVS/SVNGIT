import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '../services/authentication.service';

@Component({
    templateUrl: '/app/profiles/components/login-form.component.html'
})
export class LoginForm {

    model: any = {};
    loading = false;
    error = '';

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService) { }

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

}