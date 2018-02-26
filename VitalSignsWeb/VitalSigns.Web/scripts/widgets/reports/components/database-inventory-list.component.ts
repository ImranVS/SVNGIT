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
    gridUrl: string = `/reports/database_inventory`
    constructor(private service: RESTService) { }

    loadData() {
        this.service.get(this.gridUrl)
            .subscribe(
            data => this.databaseInventory = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        if (this.settings & this.settings.url)
            this.gridUrl = this.settings.url;
        var displayDate = (new Date()).toISOString().slice(0, 10);
        this.loadData();
    }
    refresh(url) {
        this.gridUrl = url;
        this.loadData();
    }
}