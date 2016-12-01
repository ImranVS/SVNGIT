import {Component, OnInit} from '@angular/core';

import {AppHeader} from './navigation/app.header.component';
import {AppMainMenu} from './navigation/app.main-menu.component';
import {AppComponentService} from './core/services';

declare var bootstrapZeus: any;
declare var injectSVG: any;

@Component({
    selector: 'app',
    template: `
<div id="zeusMain">
    <div id="zeusMenu">
        <app-main-menu></app-main-menu>
    </div>
    <div id="zeusHeader">

        <app-header></app-header>
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
`,
    providers: [
        AppComponentService
    ]
})

export class AppComponent implements OnInit {
     dismissOnTimeout: number;
     type: string;
     message: string;
     private closed: boolean = true;
     private closeable: boolean = true;
     private classes: string;  
     
     constructor(private appComponentService: AppComponentService) {

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

     onClose(event: MouseEvent) {        
         this.closed = true;
     }
    ngOnInit() {
        bootstrapZeus();
        injectSVG();
    }
}