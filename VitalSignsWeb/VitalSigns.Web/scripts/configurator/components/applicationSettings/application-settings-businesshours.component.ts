import {Component, OnInit, ViewChild} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';

import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjInputTime from 'wijmo/wijmo.angular2.input';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';

import {AppComponentService} from '../../../core/services';


@Component({
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-businesshours.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class BusinessHours extends GridBase implements OnInit {  
    displayDate: Date;
    errorMessage: string;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('wjTimeCtrl') wjTimeCtrl: wijmo.input.InputTime;
    formObject: any = {
        id: null,
        name: null,
        use_type: null,
        start_time: null,
        duration: null,
        sunday: null,
        monday: null,
        tuesday: null,
        wednesday: null,
        thursday: null,
        friday: null,
        saturday: null
    };

     constructor(service: RESTService, appComponentService: AppComponentService) {
         super(service, appComponentService);
         this.formName = "Business Hours";
     }  

    addBusinessHours(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.formObject.id = "";
        this.formObject.name = "";
        this.formObject.use_type = "2";
        this.formObject.start_time = "12:00 PM";
        this.formObject.duration = "0";
        this.formObject.sunday = false;
        this.formObject.monday = false;
        this.formObject.tuesday = false;
        this.formObject.wednesday = false;
        this.formObject.thursday = false;
        this.formObject.friday = false;
        this.formObject.saturday = false;
     }

    ngOnInit() {
        //this.initialGridBind('/Configurator/get_business_hours');
        this.service.get('/Configurator/get_business_hours')
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data[0]));
                    //this.data.pageSize = 10;
                } else {
                    this.appComponentService.showErrorMessage(response.message);
                }

            }, error => {
                var errorMessage = <any>error;
                this.appComponentService.showErrorMessage(errorMessage);
            });
    } 

    saveBusinessHour(dlg: wijmo.input.Popup) {  
        this.formObject.start_time = this.wjTimeCtrl.selectedValue;
        this.errorMessage = ""; 
        if (!this.formObject.sunday && !this.formObject.monday && !this.formObject.tuesday && !this.formObject.wednesday && !this.formObject.thursday
            && !this.formObject.friday && !this.formObject.saturday) {
            this.errorMessage = "No selection made. Please select at least one day.";
            this.appComponentService.showErrorMessage(this.errorMessage);
        }
        if (!this.errorMessage) {
            if (this.formObject.id == "") {
                this.service.put('/configurator/save_business_hours', this.formObject)
                    .subscribe(
                    response => {
                        if (response.status == "Success") {
                            this.data = response.data[0];
                            (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew();
                            dlg.hide();
                            this.appComponentService.showSuccessMessage(response.message);
                        }
                        else {
                            this.appComponentService.showErrorMessage(response.message);
                        }
                    });  
            }
            else {
                this.service.put('/configurator/save_business_hours', this.formObject)
                    .subscribe(
                    response => {
                        if (response.status == "Success") {
                            this.data = response.data[0];
                            (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit();
                            dlg.hide();
                            this.appComponentService.showSuccessMessage(response.message);
                        }
                        else {
                            this.appComponentService.showErrorMessage(response.message);
                        }
                    });
            }
        }
        //this.saveGridRow('/Configurator/save_business_hours', dlg);
    }
    
    delteBusinessHour() {      
        this.delteGridRow('/Configurator/delete_business_hours/');  
    }

    editBusinessHours(dlg: wijmo.input.Popup) {
        this.formTitle = "Edit " + this.formName;
        this.formObject.id = this.flex.collectionView.currentItem.id;
        this.formObject.name = this.flex.collectionView.currentItem.name;
        this.formObject.use_type = this.flex.collectionView.currentItem.use_type;
        this.convertTimeformat("24", this.flex.collectionView.currentItem.start_time);
        if (this.wjTimeCtrl != null) {
            this.wjTimeCtrl.value = this.formObject.start_time;
        }
        this.formObject.duration = this.flex.collectionView.currentItem.duration;
        this.formObject.sunday = this.flex.collectionView.currentItem.sunday;
        this.formObject.monday = this.flex.collectionView.currentItem.monday;
        this.formObject.tuesday = this.flex.collectionView.currentItem.tuesday;
        this.formObject.wednesday = this.flex.collectionView.currentItem.wednesday;
        this.formObject.thursday = this.flex.collectionView.currentItem.thursday;
        this.formObject.friday = this.flex.collectionView.currentItem.friday;
        this.formObject.saturday = this.flex.collectionView.currentItem.saturday;
        
        this.showDialog(dlg);
    }

    selectAllClick(index: any) {
        this.currentEditItem.sunday = true;
        this.currentEditItem.monday = true;
        this.currentEditItem.tuesday = true;
        this.currentEditItem.wednesday = true;
        this.currentEditItem.thursday = true;
        this.currentEditItem.friday = true;
        this.currentEditItem.saturday = true;
        
    }

    deselectAllClick(index: any) {

        this.currentEditItem.sunday = false;
        this.currentEditItem.monday = false;
        this.currentEditItem.tuesday = false;
        this.currentEditItem.wednesday = false;
        this.currentEditItem.thursday = false;
        this.currentEditItem.friday = false;
        this.currentEditItem.saturday = false;
        
    }

    convertTimeformat(format, str) {
        var time = str;
        var hours = Number(time.match(/^(\d+)/)[1]);
        var minutes = Number(time.match(/:(\d+)/)[1]);
        var AMPM = time.match(/\s(.*)$/)[1];
        if (AMPM == "PM" && hours < 12) hours = hours + 12;
        if (AMPM == "AM" && hours == 12) hours = hours - 12;
        var sHours = hours.toString();
        var sMinutes = minutes.toString();
        if (hours < 10) sHours = "0" + sHours;
        if (minutes < 10) sMinutes = "0" + sMinutes;
        this.formObject.start_time = sHours + ":" + sMinutes;
    }
}



