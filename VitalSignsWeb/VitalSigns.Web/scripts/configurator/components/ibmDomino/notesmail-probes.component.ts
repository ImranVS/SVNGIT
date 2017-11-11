import {Component, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, FormsModule, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import { AppNavigator } from '../../../navigation/app.navigator.component';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';


@Component({
    selector: 'vs-notesmail-probes',
    templateUrl: 'app/configurator/components/ibmDomino/notesmail-probes.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class NotesMailProbes extends GridBase implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    serverNames: any;
    categories: any;
    errorMessage: string;
    currentPageSize: any = 20;

    
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("NotesMailProbes", this.authService.CurrentUser.email),
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

    constructor(service: RESTService, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService
) {
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
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("NotesMailProbes", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
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
        this.deleteGridRow('/configurator/delete_server/');
    }

    onItemsSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        // autosize first header row
        this.flex.autoSizeRow(0, true);

    }
}