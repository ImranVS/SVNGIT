﻿import {Component, Input, OnInit} from '@angular/core';
import {HTTP_PROVIDERS}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';

@Component({
    templateUrl: '/app/widgets/charts/components/dynamic-grid.component.html',
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

        this.service.get(this.settings.url)
            .subscribe(
            response => this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data)),
            error => this.errorMessage = <any>error
            );

    }
}