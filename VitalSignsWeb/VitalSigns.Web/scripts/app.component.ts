import {Component, OnInit} from '@angular/core';

import {AuthenticationService} from './profiles/services/authentication.service';

import {AppHeader} from './navigation/app.header.component';
import {AppMainMenu} from './navigation/app.main-menu.component';
<<<<<<< .mine

import {AlertService} from './core/services/alert.service';
||||||| .r1638
import {AlertService} from './core/services/alert.service';
=======
import {AppComponentService} from './core/services';
>>>>>>> .r1734

declare var bootstrapZeus: any;
declare var injectSVG: any;

@Component({
    selector: 'app',
    template: `
<div id="zeusMain">
    <div id="zeusMenu" *ngIf="authService.isLoggedIn">
        <app-main-menu></app-main-menu>
    </div>
    <div id="zeusHeader">
        <app-header [anonymous]="!authService.isLoggedIn"></app-header>
    </div>
    <div id="zeusWrapper">
        <router-outlet></router-outlet>
    </div>
    <div class="topCorner">
        <div class="alert" role="alert" [class]="classes" *ngIf="!closed">
            <button *ngIf="closeable" type="button" class="close" (click)="onClose($event)">
                <span aria-hidden="true">&times;</span>
                <span class="sr-only">Close</span>
            </button>
            <span> {{message}}</span>
        </div>
    </div>
</div>
<<<<<<< .mine
`
||||||| .r1638
`,
    providers: [
        AlertService
    ]
=======
`,
    providers: [
        AppComponentService
    ]
>>>>>>> .r1734
})

export class AppComponent implements OnInit {
<<<<<<< .mine
||||||| .r1638
     dismissOnTimeout: number;
     type: string;
     message: string;
     private closed: boolean = true;
     private closeable: boolean = true;
     private classes: string;  
     
     constructor(private alertService: AlertService) {
=======
     dismissOnTimeout: number;
     type: string;
     message: string;
     private closed: boolean = true;
     private closeable: boolean = true;
     private classes: string;  
     
     constructor(private appComponentService: AppComponentService) {
>>>>>>> .r1734

<<<<<<< .mine
    dismissOnTimeout: number;
    type: string;
    message: string;
||||||| .r1638
         this.alertService.registerAppComponentView(this);
     }
     showAlert(_type: string, _message: string, _dismissOnTimeout: number) {
         this.closed = false;
         this.type = _type || 'warning';
         this.message = _message;
         this.classes ="alert "
         this.classes = this.classes + ' alert-' + this.type;
         if (this.closeable) {
             this.classes = this.classes + ' alert-dismissible';
         }
         this.dismissOnTimeout = _dismissOnTimeout ||0;
         if (this.dismissOnTimeout > 0) {
             let close = this.onClose.bind(this);
             setTimeout(close, this.dismissOnTimeout);
         }
     }
=======
         this.appComponentService.registerAppComponentView(this);
     }
     showAlert(_type: string, _message: string, _dismissOnTimeout: number) {
         this.closed = false;
         this.type = _type || 'warning';
         this.message = _message;
         this.classes ="alert "
         this.classes = this.classes + ' alert-' + this.type;
         if (this.closeable) {
             this.classes = this.classes + ' alert-dismissible';
         }
         this.dismissOnTimeout = _dismissOnTimeout ||0;
         if (this.dismissOnTimeout > 0) {
             let close = this.onClose.bind(this);
             setTimeout(close, this.dismissOnTimeout);
         }
     }
>>>>>>> .r1734

    private closed: boolean = true;
    private closeable: boolean = true;
    private classes: string;

    constructor(private authService: AuthenticationService, private alertService: AlertService) {

        this.alertService.registerAppComponentView(this);

    }

    showAlert(_type: string, _message: string, _dismissOnTimeout: number) {

        this.closed = false;
        this.type = _type || 'warning';
        this.message = _message;
        this.classes = "alert "
        this.classes = this.classes + ' alert-' + this.type;

        if (this.closeable) {

            this.classes = this.classes + ' alert-dismissible';

        }

        this.dismissOnTimeout = _dismissOnTimeout || 0;

        if (this.dismissOnTimeout > 0) {

            let close = this.onClose.bind(this);
            setTimeout(close, this.dismissOnTimeout);

        }

    }

    onClose(event: MouseEvent) {

        this.closed = true;

    }

    ngOnInit() {

        bootstrapZeus();
        injectSVG();

    }
}