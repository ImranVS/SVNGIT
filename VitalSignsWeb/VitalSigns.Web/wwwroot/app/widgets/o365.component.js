//import {Component, Input, OnInit} from '@angular/core';
//import {HTTP_PROVIDERS}    from '@angular/http';
//import {WidgetComponent} from '../core/widgets';
//import {Office365Service} from '../core/services';
//@Component({
//    template: `Hello from O365: {{result}}`,
//    providers: [
//        HTTP_PROVIDERS,
//        Office365Service
//    ]
//})
//export class Office365Component implements WidgetComponent, OnInit {
//    @Input() settings: any;
//    errorMessage: string;
//    result: any;
//    constructor(private service: Office365Service) { }
//    loadData() {
//        this.result = this.service.get('/mobile_user_devices').status;
//    }
//    ngOnInit() {
//        this.loadData();
//    }
//} 
//# sourceMappingURL=o365.component.js.map