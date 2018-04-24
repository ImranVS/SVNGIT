import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-domino/console-commands-report.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class ConsoleCommands extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    gridUrl: string = `/reports/console_command_list`
    currentDeviceType: string = "Domino";
    currentWidgetName: string = `consoleCommandsTable`;
    currentWidgetURL: string = this.gridUrl;
    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/domino_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'consoleCommandsTable',
                title: 'Console Commands by User',
                name: 'ConsoleCommands',
                settings: { url: this.gridUrl }
            }
        ];
        injectSVG();
        

    }
}