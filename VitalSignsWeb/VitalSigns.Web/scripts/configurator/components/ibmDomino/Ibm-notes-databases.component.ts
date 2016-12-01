import {Component, OnInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';


@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-notes-databases.component.html',
    providers: [
        RESTService
    ]
})
export class NotesDatabases extends GridBase implements OnInit {
    sererNames: any;   
    errorMessage: string;

    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "Notes Database";


        this.service.get('/configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.sererNames = response.data.serversData;

            },
            (error) => this.errorMessage = <any>error
            );

    }

    addNotesDatabase(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.name = "";
        this.currentEditItem.is_enabled = "";
        this.currentEditItem.domino_server_name = "";
        this.currentEditItem.database_file_name = "";
        this.currentEditItem.scan_interval = "8";
        this.currentEditItem.retry_interval = "2";
        this.currentEditItem.off_hours_scan_interval = "30";
        this.currentEditItem.trigger_type = "";
        this.currentEditItem.trigger_value = "";

    }
    ngOnInit() {
        this.initialGridBind('/configurator/get_notes_databases');
    }
    saveNotesDatabase(dlg: wijmo.input.Popup) {      
        this.saveGridRow('/configurator/save_notes_databases', dlg);
    }
    deleteNotesDatabase() {
        this.delteGridRow('/configurator/delete_notes_database/');
    }
  

}



