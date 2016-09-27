import {Component, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../core/services';

@Component({
    selector: 'profiles-list',
    templateUrl: '/app/profiles/components/profiles-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ProfilesList implements OnInit {

    profilesList: any;
    errorMessage: string;

    constructor(private service: RESTService) { }

    ngOnInit() {
        this.service.get('http://localhost:1234/profiles')
            .subscribe(
            data => this.profilesList = data,
            error => this.errorMessage = <any>error
            );
    }

    deleteProfile(email: string) {
        this.service.delete(`http://localhost:1234/profiles/${email}`, () => {
            this.ngOnInit();
        });
    }
}