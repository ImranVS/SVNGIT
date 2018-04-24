import { Component, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';


@Component({
    templateUrl: '/app/reports/components/schedule-reports/schedule-reports.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class ScheduleReports extends GridBase {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('flex3') flex3: wijmo.grid.FlexGrid;
    scheduledata: wijmo.collections.CollectionView;
    currentPageSize: any = 20;
    errorMessage: string;
    weekDays: any;
    selectedSetting: string = "Daily";
    selectedWeekDays: string;
    selectedSettingValue: any;
    selected_reports: string[] = [];
    repeat: any;
    splittedVlue: string;
    formObject: any = {
        id: null,
        report_name: null,
        report_frequency: null,
        send_to: null,
        copy_to: null,
        blind_copy_to: null,
        file_format: null,
        requency_days_list: null,
        repeat: null,
       
 
    };
    refreshCheckedReports() {
        if (this.flex3.collectionView) {
            if (this.flex3.collectionView.items.length > 0) {
                for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
                    
                    if (item.is_selected) {
                        this.formObject.selected_reports.push(item.report_title + '~' + item.report_category);
                    }
                        else { item.is_selected == false; }
                   
                }
            }
        }
    }

    constructor(service: RESTService, AppComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, AppComponentService);
        this.formName = " Scheduled Reports";
        this.weekDays = [
            { weekday: "Sunday", dayNumber: "7", isChecked: false },
            { weekday: "Monday", dayNumber: "1", isChecked: false },
            { weekday: "Tuesday", dayNumber: "2", isChecked: false },
            { weekday: "Wednesday", dayNumber: "3", isChecked: false },
            { weekday: "Thursday", dayNumber: "4", isChecked: false },
            { weekday: "Friday", dayNumber: "5", isChecked: false },
            { weekday: "Saturday", dayNumber: "6", isChecked: false },
        ];
    }
    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("ScheduledReports", this.authService.CurrentUser.email),
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

       
        this.service.get(`/configurator/get_scheduled_reports`)
            .subscribe(
            response => {
                  this.binddata(response);

            }, error => {
                var errorMessage = <any>error;
                this.appComponentService.showErrorMessage(errorMessage);
            });

    }

    editMaintenance(dlg: wijmo.input.Popup) {
        this.formTitle = "Edit " + this.formName;
        this.formObject.id = this.flex.collectionView.currentItem.id;
        this.formObject.send_to = this.flex.collectionView.currentItem.send_to;
        this.formObject.copy_to = this.flex.collectionView.currentItem.copy_to;
        this.formObject.blind_copy_to = this.flex.collectionView.currentItem.blind_copy_to;
        this.formObject.file_format = this.flex.collectionView.currentItem.file_format;
        this.formObject.report_subject = this.flex.collectionView.currentItem.report_subject;
        this.formObject.report_name = this.flex.collectionView.currentItem.report_name;
        this.formObject.report_body = this.flex.collectionView.currentItem.report_body;
        this.selectedWeekDays = this.flex.collectionView.currentItem.frequency_days_list;
        this.formObject.selected_reports = this.flex.collectionView.currentItem.selected_reports;
        this.formObject.frequency_days_list = this.flex.collectionView.currentItem.frequency_days_list;
        this.selectedSetting = this.flex.collectionView.currentItem.report_frequency;
        this.formObject.report_frequency = this.flex.collectionView.currentItem.report_frequency;
        this.formObject.repeat = this.flex.collectionView.currentItem.repeat;
        this.splittedVlue = "";
        for (var item of this.weekDays) {
            this.weekDays[this.weekDays.indexOf(item)].isChecked = false;
        }
        console.log(this.flex.collectionView.currentItem.report_frequency)
        if (this.flex.collectionView.currentItem.report_frequency == "Weekly") {
            console.log("before")
            this.weeklyEditBinding();
            console.log("after")

        }
        if (this.flex3.collectionView) {
            if (this.flex3.collectionView.items.length > 0) {
                for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
                    if (this.formObject.selected_reports)
                        item.is_selected = this.formObject.selected_reports.indexOf(item.report_title + '~' + item.report_category ) != -1;
                    else { item.is_selected == false; }
                }
            }
        }
        this.formObject.is_selected = this.flex.collectionView.currentItem.is_select;
       
        this.showDialog(dlg);
    }

    handleClickWeekly(index: any) {

        if (this.formObject.selectedSetting != "Weekly") {
            for (var i = 0; i < 7; i++) {
                this.weekDays[i].isChecked = false;
            }
        }
        else if (this.formObject.selectedSetting == "Weekly") {
            this.weeklyEditBinding();
        }
    }

    weeklyEditBinding() {
        var selectedWeekDays = this.flex.collectionView.currentItem.frequency_days_list;
        var checkbox;
        var index;
        for ( index = 0; index < this.weekDays.length; index++) this.weekDays[index].isChecked = false;
        for (index = 0; index < selectedWeekDays.length; index++) {
            checkbox = this.weekDays.filter(x => x.weekday == selectedWeekDays[index]);
            console.log(checkbox)
            if (checkbox.length > 0) {
                checkbox[0].isChecked = true;
                console.log("Set : " + checkbox[0] + " to true")
            }
        }
    }

    selectAllClick(index: any) {
        for (var i = 0; i < 7; i++) {
            this.weekDays[i].isChecked = true;
        }
    }

    deselectAllClick(index: any) {
        for (var i = 0; i < 7; i++) {
            this.weekDays[i].isChecked = false;
        }
    }

    saveServerTaskDefinition(dlg: wijmo.input.Popup) {
       
        this.errorMessage = null
       this.formObject.selected_reports = [];
        this.refreshCheckedReports();
        var selectedWeekDays = [];
        if (this.formObject.selected_reports == false) {
            this.errorMessage = "No selection made. Please select at least one Report";
        }
        //console.log(this.formObject.selected_reports.length)
        if (this.selectedSetting == "Weekly") {
            for (var item of this.weekDays) {
                if (item.isChecked == true) {
                selectedWeekDays.push(item.weekday);
           }
            }
        }
        this.formObject.frequency_days_list = selectedWeekDays;
        this.formObject.report_frequency = this.selectedSetting;
        if (!this.errorMessage) {
            this.service.put('/configurator/save_scheduled_reports', this.formObject)
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        this.binddata(response);
                        this.appComponentService.showSuccessMessage(response.message);
                    }
                    else {
                        this.appComponentService.showErrorMessage(response.message);
                    }
                });
                    
            if (this.formObject.id == "") {
                (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew();
            } else {
                (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit();
            }
            this.flex.refresh();
            dlg.hide();
        }
    }

    binddata(response)
    {
        if (response.status == "Success") {
            this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data[0]));
            this.scheduledata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data[1]));
            var groupDesc = new wijmo.collections.PropertyGroupDescription('report_title');
            this.scheduledata.groupDescriptions.push(groupDesc);
            this.data.pageSize = 10;
        } else {
            this.appComponentService.showErrorMessage(response.message);
        }
    }

    delteServerTaskDefinition() {
        this.deleteGridRow('/configurator/delete_scheduled_reports/');
    }

    addServerTask(dlg: wijmo.input.Popup) {
        this.formTitle = "Add " + this.formName;
        this.formObject.id = "";
        this.formObject.report_name = "";
        this.formObject.report_frequency = "";
        this.formObject.repeat = "";
        this.formObject.frequency_days_list = "";
        this.formObject.send_to = "";
        this.formObject.copy_to = "";
        this.formObject.report_subject = "";
        this.formObject.report_body = "";
        this.formObject.blind_copy_to = "";
        this.formObject.file_format = "";
        this.selectedSetting = "";
        this.selectedWeekDays = "";
        this.showDialog(dlg);
        if (this.flex3.collectionView) {
             if (this.flex3.collectionView.items.length > 0) {
                          for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
                    item.is_selected = false;
                }
            }
        }
       
    }

    selectAll() {
        for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
            item.is_selected = true;
         }
        this.flex3.refresh();
    }

    deselectAll() {
        for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
            item.is_selected = false;
          }
        this.flex3.refresh();
    }

  }



