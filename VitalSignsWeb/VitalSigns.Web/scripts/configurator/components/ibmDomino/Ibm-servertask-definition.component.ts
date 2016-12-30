import {Component, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';


@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-servertask-definition.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServerTaskDefinition extends GridBase {
    constructor(service: RESTService, AppComponentService: AppComponentService) {
        super(service, AppComponentService);
        this.formName = "Domino Server Task Definition";

    }
    ngOnInit() {
        this.initialGridBind('/configurator/get_server_task_definiton');
    }
    saveServerTaskDefinition(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_server_task_definition', dlg);
    }
    delteServerTaskDefinition() {
        this.delteGridRow('/configurator/delete_server_task_definition/');
    }
    addServerTask(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.task_name = "";
        this.currentEditItem.load_string = "";
        this.currentEditItem.console_string = "";
        this.currentEditItem.freeze_detect = "";
        this.currentEditItem.idle_string = "";
        this.currentEditItem.max_busy_time = 0;
        this.currentEditItem.retry_count = 0;
    }
}



