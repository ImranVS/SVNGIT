import {Component, OnInit} from '@angular/core';
import {HTTP_PROVIDERS}    from '@angular/http';
import {RESTService} from '../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/configurator/components/configurator-businesshours.component.html',
    directives: [
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem
    ],
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class BusinessHours  implements OnInit {
    businessHoursList: any;
    errorMessage: string;
    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }
    constructor(private service: RESTService) { }
    data: wijmo.collections.CollectionView;
    ngOnInit() {
        this.service.get('/Configurator/business_hours')
            .subscribe(
            response => {
            this.businessHoursList = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
            this.data.pageSize = 10;
            },
            error => this.errorMessage = <any>error
            );
    
        injectSVG();
        bootstrapNavigator();

    }


 
  

}


