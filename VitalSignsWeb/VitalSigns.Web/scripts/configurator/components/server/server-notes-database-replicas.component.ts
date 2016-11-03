import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';


@Component({
    templateUrl: '/app/configurator/components/server/server-notes-database-replicas.component.html',
    providers: [RESTService]
})
export class NotesDatabaseReplica extends GridBase implements OnInit {
    sererNames: any;
    errorMessage: any;

    constructor(service: RESTService) {
        super(service);
        this.formName = "Notes Databse Replica";
        this.service.get('/Configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.sererNames = response.data.serversData;


            },
            (error) => this.errorMessage = <any>error
        );
      
    }
    ngOnInit() {
        this.initialGridBind('/configurator/get_notes_database_replica');
    }
    saveNotesDatabaseReplica(dlg: wijmo.input.Popup) {      
        this.saveGridRow('/configurator/save_notes_database_replica', dlg);
    }
    delteNotesDatabaseReplica() {
        this.delteGridRow('/configurator/notes_database_replica/');
    }
  

}



