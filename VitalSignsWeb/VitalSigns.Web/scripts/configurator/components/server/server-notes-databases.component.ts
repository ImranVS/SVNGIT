import {Component, OnInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';

@Component({
    templateUrl: '/app/configurator/components/server/server-notes-databases.component.html',
    providers: [
        RESTService
    ]
})
export class NotesDatabases extends GridBase implements OnInit {
    sererNames: any;   
    errorMessage: string;

    constructor(service: RESTService) {
        super(service);
        this.formName = "Notes Database";


        this.service.get('/configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.sererNames = response.data.serversData;

            },
            (error) => this.errorMessage = <any>error
            );

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



