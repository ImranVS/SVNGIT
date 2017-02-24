import {Component, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, FormsModule, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';


@Component({
    selector: 'vs-notesmail-probes',
    templateUrl: '/app/configurator/components/mail/notesmail-probes.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class NotesMailProbes extends GridBase implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    serverNames: any;
    categories: any;
    errorMessage: string;
    
    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "NotesMail Probe";
        this.categories = ["Domino", "Inbound", "International", "Internet Round Trip", "Domestic", "Between Hubs"];
        this.service.get('/Configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.serverNames = response.data.serversData;
            },
            (error) => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.initialGridBind('/configurator/notesmail_probes_list');
    }

    saveNotesMailProbes(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_notesmail_probes', dlg);
    }

    showAddForm(dlg: wijmo.input.Popup) {

        this.addGridRow(dlg);
        this.currentEditItem.id = "";
        this.currentEditItem.name = "";
        this.currentEditItem.scan_interval = "";
        this.currentEditItem.off_hours_interval = "";
        this.currentEditItem.retry_interval = "";
        this.currentEditItem.threshold = "";
        this.currentEditItem.send_to = "";
        this.currentEditItem.reply_to = "";
        this.currentEditItem.destination_database = "";
    }

    deleteNotesMailProbes() {
        this.delteGridRow('/configurator/delete_notesmail_probes/');
    }

    onItemsSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        // autosize first header row
        this.flex.autoSizeRow(0, true);

    }
}