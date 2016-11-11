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
    selector: 'vs-hours-destinations',
    templateUrl: '/app/configurator/components/alert/hours-destinations.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class HoursDestinations extends GridBase implements OnInit  {    
    devices: string;
    errorMessage: string;
    sendtodata: any;
    escalatetodata: any;
    eventsdata: any;
    businesshours: any;
    sendvia: any;
    isSMS: boolean;
    isSNMP: boolean;
    isEmail: boolean;
    isScript: boolean;
    isWinLog: boolean;

    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService) {
        super(service);
        this.formName = "Hours and Destinations";
        this.service.get('/configurator/get_business_hours?nameonly=true')
            .subscribe(
            (response) => {
                this.businesshours = response.data;
            },
            (error) => this.errorMessage = <any>error
        );
        this.sendvia = ["E-mail", "Script", "SMS", "SNMP Trap", "Windows Log"];
        this.isEmail = true;
    }

    ngOnInit() {
        this.service.get('/configurator/notifications_list')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[1]));
            },
            (error) => this.errorMessage = <any>error
        );
    }
 
    eventCheck(value, event) {
        if (event.target.checked) {
            this.eventsdata.push(value);
        }
        else {
            this.eventsdata.splice(this.eventsdata.indexOf(value), 1);
        }
    }

    saveHoursDestinations(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_hours_destinations', dlg);
    }

    setDefaults(src: wijmo.input.ComboBox) {
        this.getSendVia(src);
    }

    getSendVia(src: wijmo.input.ComboBox) {
        //console.log(src);
        if (src._oldText == "E-mail") {
            this.isEmail = true;
            this.isScript = false;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
        }
        else if (src._oldText == "Script") {
            this.isEmail = false;
            this.isScript = true;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
        }
        else if (src._oldText == "SMS") {
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = true;
            this.isSNMP = false;
            this.isWinLog = false;
        }
        else if (src._oldText == "SNMP Trap") {
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
        }
        else if (src._oldText == "Windows Log") {
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
        }
    }
}