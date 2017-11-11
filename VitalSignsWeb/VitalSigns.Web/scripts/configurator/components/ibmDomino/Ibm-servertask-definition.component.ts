import {Component, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';


@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-servertask-definition.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class ServerTaskDefinition extends GridBase {
    currentPageSize: any = 20;
    errorMessage: string;
    constructor(service: RESTService, AppComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, AppComponentService);
        this.formName = "Domino Server Task Definition";

    }
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("ServerTaskDefinition", this.authService.CurrentUser.email),
                value: value
            };

            this.service.put(`/services/set_name_value`, obj)
                .subscribe(
                (data) => {

                },
                (error) => console.log(error)
                );

        }
    }

    ngOnInit() {
        
        this.initialGridBind('/configurator/get_server_task_definiton');
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("ServerTaskDefinition", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }
    saveServerTaskDefinition(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_server_task_definition', dlg);
    }
    delteServerTaskDefinition() {
        this.deleteGridRow('/configurator/delete_server_task_definition/');
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



