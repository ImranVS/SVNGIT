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

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/services/get_powershell_scripts')
            .subscribe(

            data => this.data = data.data.scripts,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }

   
}