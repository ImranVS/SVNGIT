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
    usersByserver: any = [];
    checkedItems: any[];
    serversData: any;
    dominoServers: any;
   

    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "Notes Database";


       

    }

    addNotesDatabase(dlg: wijmo.input.Popup, servers: wijmo.input.MultiSelect) {
        this.addGridRow(dlg);
        this.currentEditItem.name = "";
        this.currentEditItem.is_enabled = false;
        this.currentEditItem.domino_server_name = "";
        this.currentEditItem.database_file_name = "";
        this.currentEditItem.scan_interval = "8";
        this.currentEditItem.retry_interval = "2";
        this.currentEditItem.off_hours_scan_interval = "30";
        this.currentEditItem.trigger_type = null;
        this.currentEditItem.trigger_value = null;
        this.currentEditItem.initiate_replication = false;
        this.currentEditItem.replication_destination = null;
        this.checkedItems = null;
        this.usersByserver = null;
        if(servers)
             servers.refresh(true);
    }
    ngOnInit() {
       
        this.initialGridBind('/configurator/get_notes_databases');
        this.service.get('/configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.sererNames = response.data.serversData;
                this.dominoServers = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.serversData));
            },
            (error) => this.errorMessage = <any>error
            );
    }
    saveNotesDatabase(dlg: wijmo.input.Popup) {  
        this.currentEditItem.replication_destination = this.usersByserver;  
        this.saveGridRow('/configurator/save_notes_databases', dlg);
    }
    deleteNotesDatabase() {
        this.delteGridRow('/configurator/delete_notes_database/');
    }

    //serversChecked(servers: wijmo.input.MultiSelect) {
    //    this.usersByserver = null;
    //    for (var item of servers.checkedItems) {
    //        this.usersByserver.push(item.value);
    //    }
    //}

    editNotesDatabase(dlg: wijmo.input.Popup) {
        this.editGridRow(dlg);
        //this.checkedItems = null;
        //this.usersByserver = null;
        //var replicationdestination = this.currentEditItem.replication_destination;
        //if (replicationdestination) {
        //    if (servers)
        //         servers.refresh(true);
        //    for (var travelerItem of (<wijmo.collections.CollectionView>this.dominoServers).sourceCollection) {
        //        var server = replicationdestination.filter((item) => item == travelerItem.value);
        //        if (server.length > 0)
        //            this.checkedItems.push(travelerItem);
        //    }
        //}
    }
}



