import {Component, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';

@Component({
    templateUrl: '/app/configurator/components/server/IbmServerTaskDefinition.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServerTaskDefinition extends GridBase implements OnInit {
    constructor(service: RESTService) {
        super(service);
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
}



