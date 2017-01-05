﻿import {Component, OnInit, ViewChild} from '@angular/core';
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
    @ViewChild('selectserver') selectserver;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    sererNames: any;   
    errorMessage: string;
    usersByserver: any = [];
    checkedItems: any[];
    serversData: any;
    dominoServer: any;
    dominoServers: any;
   

    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "Notes Database";
    }

    addNotesDatabase(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.name = "";
        this.currentEditItem.is_enabled = false;
        this.currentEditItem.domino_server_name = "";
        this.currentEditItem.database_file_name = "";
        this.currentEditItem.scan_interval = "8";
        this.currentEditItem.retry_interval = "2";
        this.currentEditItem.off_hours_scan_interval = "30";
        this.currentEditItem.trigger_type = "";
        this.currentEditItem.trigger_value = "";
        this.currentEditItem.initiate_replication = false;
        this.currentEditItem.replication_destination = "";
        this.checkedItems = [];
        this.usersByserver = [];
    }
    ngOnInit() {
       
        this.initialGridBind('/configurator/get_notes_databases');
        this.service.get('/configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.sererNames = response.data.serversData;
                //this.dominoServers = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.serversData));
                this.dominoServers = response.data.serversData;
            },
            (error) => this.errorMessage = <any>error
            );
    }
    saveNotesDatabase(dlg: wijmo.input.Popup) {  
        this.serversChecked();
        this.currentEditItem.replication_destination = this.usersByserver;  
        this.saveGridRow('/configurator/save_notes_databases', dlg);
    }
    deleteNotesDatabase() {
        this.delteGridRow('/configurator/delete_notes_database/');
    }

    serversChecked() {
        let options = this.selectserver.nativeElement.options;
        for (let i = 0; i < options.length; i++) {
            if (options[i].selected) {
                var ind1 = options[i].value.indexOf("'");
                var ind2 = options[i].value.lastIndexOf("'");
                this.usersByserver.push(options[i].value.substring(ind1 + 1, ind2));
            }
        }
    }

    editNotesDatabase(dlg: wijmo.input.Popup) {
        var replicationdestination = this.flex.collectionView.currentItem.replication_destination;
        this.dominoServer = replicationdestination;
        this.editGridRow(dlg);
    }

    initialized(multiselect: wijmo.input.MultiSelect) {
        if (multiselect != null) {
            multiselect.isDroppedDown = true;
        }
    }

    selectServers() {
        if (this.currentEditItem.trigger_type == "Replication") {

        }
    }
}



