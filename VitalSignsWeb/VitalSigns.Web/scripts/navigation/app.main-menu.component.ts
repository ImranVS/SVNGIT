import {Component} from '@angular/core';

@Component({
    selector: 'app-main-menu',
    templateUrl: '/partial/sitemap',
})
export class AppMainMenu {

    getUrlPath(rawUrl: string): string {
    
        let regex = /[^\?]*/;

        return regex.exec(rawUrl)[0];

    }

    getUrlQueryParams(rawUrl: string): any {
    
        let paramsMatcher = /[^\?]*\?(.*)/;
        
        if (paramsMatcher.test(rawUrl)) {
        
            let params = paramsMatcher.exec(rawUrl)[1];

            let paramMatcher = /\&?([^\=]*)\=([^\&]*)/g;
            
            let queryParams = {};
            let match = paramMatcher.exec(params);

            while (match != null) {

                queryParams[match[1]] = match[2];
                match = paramMatcher.exec(params);

            }
            
            return queryParams;
        }
        else
            return {};

    }

}