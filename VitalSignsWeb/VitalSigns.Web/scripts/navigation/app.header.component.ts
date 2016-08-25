import {Component} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

@Component({
    selector: 'app-header',
    templateUrl: '/app/navigation/app.header.component.html',
    directives: [ROUTER_DIRECTIVES]
})
export class AppHeader { }