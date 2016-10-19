import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

@Component({
    templateUrl: './app/widgets/main-dashboard/components/on-premises-apps.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class OnPremisesApps implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    onPremApps: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/services/status_summary_by_type')
            .subscribe(
            data => this.onPremApps = data,
            error => this.errorMessage = <any>error
        );
    }

    ngOnInit() {
        this.loadData();
    }
}