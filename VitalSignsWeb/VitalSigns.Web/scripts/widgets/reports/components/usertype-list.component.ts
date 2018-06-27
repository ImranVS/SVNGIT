
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
    templateUrl: './app/widgets/reports/components/usertype-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class UserTypeList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;
    isLoading: boolean = true;
    data: any;

    constructor(private service: RESTService) { }

    

    
    loadData() {
        this.isLoading = true;
        let deviceType = this.settings.deviceType;
        let inactive = this.settings.inactive;
        this.service.get(`/reports/usertype?inactive=${inactive}`)
            .finally(() => this.isLoading = false)
            .subscribe(
                data => this.data = data.data,
                error => this.errorMessage = <any>error
            );
    }
    ngOnInit() {
        this.loadData();
    }
}