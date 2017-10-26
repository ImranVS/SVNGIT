import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../core/widgets';
import { RESTService } from '../../core/services';
declare var injectSVG: any;

@Component({
    selector: "executive-summary-widget",
    templateUrl: '/app/dashboards/components/executive-summary-widget.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ExecutiveSummaryWidget implements WidgetComponent, OnInit {
    @Input() deviceType: any;
    @Input() settings: any;

    errorMessage: string;
    data: any;

    constructor(private service: RESTService) { }

    loadData() {
        let url = "/services/device_list?module=dashboard";
        if (this.settings && this.settings.deviceType && this.settings.deviceType != "")
            url = url + `&deviceType=${this.settings.deviceType}`
        else if (this.deviceType && this.deviceType != "")
            url = url + `&deviceType=${this.deviceType}`
        
        this.service.get(url)
            .subscribe(
            response => this.data = response.data.filter(x => x.is_enabled === true && (x.status_code != null && x.status_code.trim() != "")),
            error => this.errorMessage = <any>error
        );
    }

    ngAfterViewChecked() {
        injectSVG();
    }

    ngOnInit() {
        this.loadData();
      
    }
    refresh() {
        this.loadData();
      
    }
}