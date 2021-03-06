﻿import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {DominoServerTasks} from '../models/domino-server-tasks';


@Component({
    templateUrl: './app/widgets/reports/components/powershell-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class PowershellList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    data: any;
    allData: any[];

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/services/get_powershell_scripts')
            .subscribe(

            data => {
                this.data = data.data.scripts;
                this.allData = this.data;
            },
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }

    refresh(value: any[]) {
        if (value.length === 0 || value.findIndex(x => x.device_type === "All") !== -1) {
            this.data = this.allData;
        } else {
            this.data = this.allData.filter(x => value.findIndex(y => y.device_type === x.device_type) !== -1);
        }
    }

   
}