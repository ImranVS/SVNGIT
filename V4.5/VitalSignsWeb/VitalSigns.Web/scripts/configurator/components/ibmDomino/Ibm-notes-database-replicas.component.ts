import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';



@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-notes-database-replicas.component.html',
    providers: [RESTService, gridHelpers.CommonUtils]
})
export class NotesDatabaseReplica extends GridBase implements OnInit {
    sererNames: any;
    errorMessage: any;
    currentPageSize: any = 20;
    currentEditItem: any;
    constructor(service: RESTService, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, appComponentService);
        this.formName = "Notes Database Replica";
        this.service.get('/Configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.sererNames = response.data.serversData;
            },
            (error) => this.errorMessage = <any>error
        );
        this.appComponentService = appComponentService;
    }
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("NotesDatabaseReplica", this.authService.CurrentUser.email),
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
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("NotesDatabaseReplica", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        this.initialGridBind('/configurator/get_notes_database_replica');
    }
    saveNotesDatabaseReplica(dlg: wijmo.input.Popup) {     
        if (this.currentEditItem.domino_server_a != this.currentEditItem.domino_server_b) {
            this.saveGridRow('/configurator/save_notes_database_replica', dlg);
        }
        else {
            this.appComponentService.showErrorMessage("You must select two different servers to be added as cluster members.");
        }
         
        
    }
    delteNotesDatabaseReplica() {
        this.deleteGridRow('/configurator/notes_database_replica/');
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
        this.currentEditItem.domino_server_c = null;
        this.currentEditItem.domino_server_c_file_mask = "";
        this.currentEditItem.domino_server_c_exclude_folders = "";
        this.currentEditItem.difference_threshold = "";
        
    }
}



