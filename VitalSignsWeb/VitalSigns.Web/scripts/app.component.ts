import {Component, OnInit} from '@angular/core';

import {AppHeader} from './navigation/app.header.component';
import {AppMainMenu} from './navigation/app.main-menu.component';

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
</div>
`
})
export class AppComponent implements OnInit {
    ngOnInit() {
        bootstrapZeus();
        injectSVG();
    }
}