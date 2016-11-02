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
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';


@Component({
    templateUrl: '/app/configurator/components/server/server-notes-databases.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class NotesDatabases extends GridBase {
    sererNames: any;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    errorMessage: string;

    constructor(service: RESTService) {
        super(service, '/configurator/get_notes_databases');
        this.formName = "Notes Database";


        this.service.get('/configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.sererNames = response.data.serversData;

            },
            (error) => this.errorMessage = <any>error
            );

    }
    saveNotesDatabase(dlg: wijmo.input.Popup) {      
        this.saveGridRow1('/configurator/save_notes_databases', dlg);
    }
    deleteNotesDatabase() {
        this.delteGridRow('/configurator/delete_notes_database/');
    }
  

}



