
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { HttpModule } from '@angular/http';
import { WidgetComponent } from '../../../core/widgets';
import { RESTService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';


@Component({
    templateUrl: './app/widgets/reports/components/usergroup-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class UserGroupList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    data: any;

    constructor(private service: RESTService) { }

    

    
    loadData() {
        let deviceType = this.settings.deviceType;
        let type = this.settings.type;
        this.service.get(`/reports/usergroup?type=${type}`)
            .subscribe(
            data => this.data = data.data,
            error => this.errorMessage = <any>error
            );
    }
    ngOnInit() {
        this.loadData();
    }
}