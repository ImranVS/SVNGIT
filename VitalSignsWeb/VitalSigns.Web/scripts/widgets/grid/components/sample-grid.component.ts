import {Component, Input, OnInit} from '@angular/core';
import {HTTP_PROVIDERS}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';

@Component({
    template: `
<wj-flex-grid [itemsSource]="data" selectionMode="None"></wj-flex-grid>
`,
    directives: [wjFlexGrid.WjFlexGrid, wjFlexGrid.WjFlexGridColumn],
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class SampleGrid implements WidgetComponent, OnInit {
    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    
    constructor(private service: RESTService) { }
    
    ngOnInit() {
    
        this.service.get('/mobile_user_devices')
            .subscribe(
            data => this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data)),
            error => this.errorMessage = <any>error
            );

    }
}