import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';


@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-notes-database-replicas.component.html',
    providers: [RESTService]
})
export class NotesDatabaseReplica extends GridBase implements OnInit {
    sererNames: any;
    errorMessage: any;
    currentEditItem: any;
    constructor(service: RESTService) {
        super(service);
        this.formName = "Notes Database Replica";
        this.service.get('/Configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.sererNames = response.data.serversData;
            

               
                console.log(this.sererNames);
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
  
   

    addNotesDatabaseReplica(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.device_name = "";
        this.currentEditItem.is_enabled = "";
        this.currentEditItem.scan_interval = "";
        this.currentEditItem.off_hours_scan_interval = "";
        this.currentEditItem.domino_server_a  = "";
        this.currentEditItem.domino_server_a_file_mask = "";
        this.currentEditItem.domino_server_a_exclude_folders = "";
        this.currentEditItem.domino_server_b = "";
        this.currentEditItem.domino_server_b_file_mask = "";
        this.currentEditItem.domino_server_b_exclude_folders = "";
        this.currentEditItem.domino_server_c = "";
        this.currentEditItem.domino_server_c_file_mask = "";
        this.currentEditItem.domino_server_c_exclude_folders = "";
        this.currentEditItem.difference_threshold = "";
        
    }
}



