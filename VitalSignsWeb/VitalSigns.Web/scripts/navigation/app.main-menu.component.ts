import {Component} from '@angular/core';

import * as helpers from '../core/services/helpers/helpers';

@Component({
    selector: 'app-main-menu',
    templateUrl: '/partial/sitemap',
    providers: [helpers.UrlHelperService]
})
export class AppMainMenu {

    constructor(protected urlHelpers: helpers.UrlHelperService) { }
    
}