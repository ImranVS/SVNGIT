﻿import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';


declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-overall-domino-statistics.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class OverallDominoStat implements OnInit {
    @Input() settings: any;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;

    constructor(private service: RESTService, private route: ActivatedRoute) { }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }
    ngOnInit() {

        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
        });
        this.service.get('/configurator/overall_domino_stat')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );
    }
}


