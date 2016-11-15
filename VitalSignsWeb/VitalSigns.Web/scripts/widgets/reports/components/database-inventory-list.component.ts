import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {DatabaseInventory} from '../models/database-inventory';


@Component({
    templateUrl: './app/widgets/reports/components/database-inventory-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class DatabaseInventoryList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    databaseInventory: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get(this.settings.url)
            .subscribe(
            data => this.databaseInventory = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}