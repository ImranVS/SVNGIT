import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { JwtHelper } from 'angular2-jwt';

import { AppConfig } from '../../app.config';

@Injectable()
export class AuthenticationService {

    private _uat: string = null;
    private _currentUser: any = null;
    private _storage: Storage;

    get UAT(): string {

        return this._uat;

    }

    set UAT(token: string) {

        this._uat = token;

    }

    get isLoggedIn(): boolean {

        return this._uat != null;

    }

    get CurrentUser(): any {

        if (this._currentUser == null) {

            let jwtHelper: JwtHelper = new JwtHelper();

            this._currentUser = jwtHelper.decodeToken(this._uat);

        }

        return this._currentUser;

    }

    constructor(
        private config: AppConfig,
        private http: Http
    ) {
    
        switch (this.config.getConfig('uatStorage')) {

            case 'local':
                this._storage = localStorage;
                break;

            default:
                this._storage = sessionStorage;
                
        }

        var currentUser = JSON.parse(this._storage.getItem('uat'));
        this._uat = currentUser && currentUser.token;

    }
    
    login(username: string, password: string): Observable<boolean> {

        return this.http.post(`${this.config.getConfig('apiEndpoint')}/token`, { username: username, password: password })
            .map((response: Response) => {
                return this.wesTest(response, username);                    
            });

    }

    wesTest(response,username) {
        let token = response.json() && response.json().token;

        if (token) {

            this._uat = token;
            this._storage.setItem('uat', JSON.stringify({ username: username, token: token }));

            return true;

        } else
            return false;
    }

    logout(): void {

        this._uat = null;
        this._storage.removeItem('uat');

    }

    isCurrentUserInRole(roleName: string): boolean {

        if (this.CurrentUser != null) {

            let roles: Array<string> = this.CurrentUser.role;

            return roles && roles.indexOf(roleName) > -1;

        } else
            return false;
       
    }

}