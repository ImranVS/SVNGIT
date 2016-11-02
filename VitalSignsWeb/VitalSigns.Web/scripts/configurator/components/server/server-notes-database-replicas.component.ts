import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';

import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;


@Component({
    templateUrl: '/app/configurator/components/server/server-notes-database-replicas.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class NotesDatabaseReplica extends GridBase {
    sererNames: any;
    errorMessage: any;

    constructor(service: RESTService) {
        super(service, '/configurator/get_notes_database_replica');
        this.formName = "Notes Databse Replica";
        this.service.get('/Configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.sererNames = response.data.serversData;


            },
            (error) => this.errorMessage = <any>error
        );
      
    }
    saveNotesDatabaseReplica(dlg: wijmo.input.Popup) {      
        this.saveGridRow1('/configurator/save_notes_database_replica', dlg);
    }
    delteNotesDatabaseReplica() {
        this.delteGridRow('/configurator/notes_database_replica/');
    }
  

}



