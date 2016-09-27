import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import { DominoServerInfo } from '../models/domino-server-info';

@Component({
    templateUrl: './app/widgets/main-dashboard/components/domino-server-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class DominoServersInfo implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    dominoServers: DominoServerInfo;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/domino/info')
            .subscribe(
            data => this.dominoServers = <DominoServerInfo>data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}