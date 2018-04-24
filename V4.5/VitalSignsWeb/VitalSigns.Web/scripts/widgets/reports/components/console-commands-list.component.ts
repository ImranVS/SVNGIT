import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

//import {ConsoleCommand} from '../models/console-command';

@Component({
    templateUrl: './app/widgets/reports/components/console-commands-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ConsoleCommands implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    consoleCommands: any;
    gridUrl: string = `/reports/console_command_list`
    constructor(private service: RESTService) { }

    loadData() {
        this.service.get(this.gridUrl)
            .subscribe(
            data => this.consoleCommands = data.data,
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