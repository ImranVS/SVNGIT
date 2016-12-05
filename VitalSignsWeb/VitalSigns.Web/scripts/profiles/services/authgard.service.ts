import { Injectable } from '@angular/core';
import { Router, CanActivate, CanActivateChild } from '@angular/router';

import { AuthenticationService } from './authentication.service';

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild {

    constructor(
        private router: Router,
        private authService: AuthenticationService) { }

    canActivate() {

        if (this.authService.isLoggedIn)
            return true;
        
        this.router.navigate(['/login']);

        return false;

    }

    canActivateChild() {

        return this.canActivate();

    }
}