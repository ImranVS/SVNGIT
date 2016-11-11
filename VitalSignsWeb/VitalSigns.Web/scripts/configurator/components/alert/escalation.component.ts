import {Component, OnInit} from '@angular/core';
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

@Component({
    selector: 'vs-escalation',
    templateUrl: '/app/configurator/components/alert/escalation.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Escalation extends GridBase implements OnInit  {    
    devices: string;
    errorMessage: string;
    sendvia: any;
    isSMS: boolean;
    isEmail: boolean;
    isScript: boolean;

    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService) {
        super(service);
        this.formName = "Escalation";
        this.sendvia = ["E-mail", "Script", "SMS"];
        this.isEmail = true;
    }

    ngOnInit() {
        this.service.get('/configurator/notifications_list')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[2]));
            },
            (error) => this.errorMessage = <any>error
        );
    }

    changeInDevices(devices: string) {
        this.devices = devices;
    }

    saveEscalation(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_escalation', dlg);
    }

    setDefaults(src: wijmo.input.ComboBox) {
        this.getSendVia(src);
    }

    getSendVia(src: wijmo.input.ComboBox) {
        console.log(src);
        if (src._oldText == "E-mail") {
            this.isEmail = true;
            this.isScript = false;
            this.isSMS = false;
        }
        else if (src._oldText == "Script") {
            this.isEmail = false;
            this.isScript = true;
            this.isSMS = false;
        }
        else if (src._oldText == "SMS") {
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = true;
        }
    }
}