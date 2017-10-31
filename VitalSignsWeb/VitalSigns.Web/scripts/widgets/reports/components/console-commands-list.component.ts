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

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/reports/console_command_list')
            .subscribe(
            data => this.consoleCommands = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}