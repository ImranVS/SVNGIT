import {Component, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';
import {ServersLocationService} from '../serverSettings/serverattributes-view.service';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    templateUrl: '/app/configurator/components/applicationsettings/application-settings-maintenance.component.html', 
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper,
        ServersLocationService
    ]
})
export class Maintenance extends GridBase implements OnInit  {  
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('flex1') flex1: wijmo.grid.FlexGrid;
    @ViewChild('wjStartTimeCtrl') wjStartTimeCtrl: wijmo.input.InputTime;

    errorMessage: string;
    dataMobileUsers: wijmo.collections.CollectionView;
    selectedSetting: string = "1";
    durationSetting: any;
    
    selectedSettingValue: any;
    selectedDays: any;
    weekDays: any;
    repeat: any;
    repeat_monthly: any;
    end_date: any;
    day_of_the_month: any;
    splittedVlue: string;

    devices: string[] = [];
    _deviceList: any;
    checkedDevices: any;
    selDeviceTypes: string = "Domino,Sametime,URL,WebSphere,IBM Connections";
    keyUsers: string[] = [];
    loading = false;

    formObject: any = {
        id: null,
        name: null,
        duration_type: null,
        start_date: null,
        end_date: null,
        start_time: null,
        end_time: null,
        duration: null,
        maintain_type: null,
        maintain_type_value: null,
        maintenance_days_list: null,
        repeat_every: null,
        repeat_monthly: null,
        specific_day: null,
        key_users: null,
        device_list: null
    };
   
    constructor(service: RESTService, appComponentService: AppComponentService, private datetimeHelpers: helpers.DateTimeHelper) {
        super(service, appComponentService);
        this.formName = "Maintenance";
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

    keyUsersCheck(value, event) {

        if (event.target.checked) {
            this.keyUsers.push(value);
            this.flex1.collectionView.currentItem.is_selected = true;
        }
        else {
            this.keyUsers.splice(this.keyUsers.indexOf(value), 1);
            this.flex1.collectionView.currentItem.is_selected = false;
        }
    }

    ngOnInit() {

        this.service.get('/Configurator/get_maintenance')
            .subscribe(
            response => {
                if (response.status == "Success") {

                    this.datetimeHelpers.nameToFormat['start_date'] = "date";
                    this.datetimeHelpers.nameToFormat['start_time'] = "time";
                    this.datetimeHelpers.nameToFormat['end_date'] = "date";
                    this.datetimeHelpers.nameToFormat['end_time'] = "time";

                    response.data.forEach(function (entry) {
                        var dt = new Date(entry.start_date);
                        if (dt.getUTCHours() == 0 && dt.getUTCMinutes() == 0 && dt.getUTCSeconds() == 0 && dt.getUTCMilliseconds() == 0) {
                            var dt2 = new Date(dt.getUTCFullYear(), dt.getUTCMonth(), dt.getUTCDate());
                            entry.start_date = dt2.toISOString();
                        }

                        dt = new Date(entry.end_date);
                        if (dt.getUTCHours() == 0 && dt.getUTCMinutes() == 0 && dt.getUTCSeconds() == 0 && dt.getUTCMilliseconds() == 0) {
                            var dt2 = new Date(dt.getUTCFullYear(), dt.getUTCMonth(), dt.getUTCDate());
                            entry.end_date = dt2.toISOString();
                        }
                    });

                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocal(response.data)));
                    //this.data.pageSize = 10;
                } else {
                    this.appComponentService.showErrorMessage(response.message);
                }

            }, error => {
                var errorMessage = <any>error;
                this.appComponentService.showErrorMessage(errorMessage);
            });
        //this.initialGridBind('/Configurator/get_maintenance');
        this.keyUsersGridBind();

    }

    handleClickMonthly(index: any) {
        if (this.formObject.maintain_type_value != "4") {
            for (var i = 0; i < 7; i++) {
                this.weekDays[i].isChecked = false;
            }
        }
        else if (this.formObject.maintain_type_value == "4") {
            this.monthlyEditBinding();
        }

    }

    handleClickWeekly(index: any) {

        if (this.formObject.maintain_type_value != "3") {
            for (var i = 0; i < 7; i++) {
                this.weekDays[i].isChecked = false;
            }
        }
        else if (this.formObject.maintain_type_value == "3") {
            this.weeklyEditBinding();
        }
    }

    keyUsersGridBind() {
        this.service.get('/Dashboard/mobile_user_devices?isKey=true')
            .subscribe(
            (response) => {
                var resultData: any = [];
                for (var item of response.data) {
                    if (this.keyUsers) {
                        var value = this.keyUsers.filter((record) => record == item.id);
                        if (value.length > 0) {
                            item.is_selected = true;
                        }
                    }
                    resultData.push(item);
                }

                this.dataMobileUsers = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(resultData));
                //this.dataMobileUsers.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );
    }

    editMaintenance(dlg: wijmo.input.Popup) {
        this.devices = [];
        this.keyUsers = [];
        this.formTitle = "Edit " + this.formName;
        this.formObject.id = this.flex.collectionView.currentItem.id;
        this.formObject.name = this.flex.collectionView.currentItem.name;
        this.durationSetting = this.flex.collectionView.currentItem.duration_type;
        this.formObject.duration_type = this.flex.collectionView.currentItem.duration_type;
        this.formObject.start_date = this.flex.collectionView.currentItem.start_date;
        this.formObject.end_date = this.flex.collectionView.currentItem.end_date;
        this.formObject.start_time = this.convertTimeformat("24", this.flex.collectionView.currentItem.start_time);
        if (this.wjStartTimeCtrl != null) {
            this.wjStartTimeCtrl.value = this.formObject.start_time;
        }
        if (this.flex.collectionView.currentItem.end_time != null) {
            this.formObject.end_time = this.flex.collectionView.currentItem.end_time;
        }
        else {
            var result = new Date(new Date().toDateString() + ' ' + this.flex.collectionView.currentItem.start_time);
            var endTime = new Date(result.getTime() + this.flex.collectionView.currentItem.duration * 60000);
            var sHours = endTime.getHours().toString();
            var sMinutes = endTime.getMinutes().toString();
            if (endTime.getHours() < 10) sHours = "0" + sHours;
            if (endTime.getMinutes() < 10) sMinutes = "0" + sMinutes;
            this.formObject.end_time = sHours + ':' + sMinutes;
        }
        this.formObject.duration = this.flex.collectionView.currentItem.duration;
        this.formObject.maintain_type = this.flex.collectionView.currentItem.maintain_type;
        this.formObject.maintain_type_value = this.flex.collectionView.currentItem.maintain_type_value;
        this.selectedSetting = this.flex.collectionView.currentItem.maintain_type_value;

        //this.editGridRow(dlg);
        //this.repeat = "";
        //this.repeat_monthly = "";
        //this.day_of_the_month = "";
        this.splittedVlue = "";

        for (var item of this.weekDays) {
            this.weekDays[this.weekDays.indexOf(item)].isChecked = false;
        }

        if (this.flex.collectionView.currentItem.maintain_type_value == "3") {
            this.weeklyEditBinding();
        }

        if (this.flex.collectionView.currentItem.maintain_type_value == "4") {
            this.monthlyEditBinding();
        }


        //this.selectedSetting = this.currentEditItem.maintain_type_value;
        //this.durationSetting = this.currentEditItem.duration_type;
        //this.keyUsers = this.currentEditItem.key_users;
      
        //this.checkedDevices = this.currentEditItem.device_list;
        //this.keyUsersGridBind();

        this.formObject.device_list = this.flex.collectionView.currentItem.device_list;
        for (var _i = 0; _i < this.flex.collectionView.currentItem.device_list.length; _i++) {
            var item = this.formObject.device_list[_i];
            if (item) {
                this.devices.push(item);
            }
            else {
            }
        }
        this._deviceList = this.devices;
        this.checkedDevices = this._deviceList;

        this.formObject.key_users = this.flex.collectionView.currentItem.key_users;
        for (var _i = 0; _i < this.flex.collectionView.currentItem.key_users.length; _i++) {
            var item = this.formObject.key_users[_i];
            if (item) {
                this.keyUsers.push(item);
            }
            else {
            }
        }
        this.keyUsersGridBind();
        this.showDialog(dlg);
    }

    weeklyEditBinding() {
        var selectedWeekDays = this.flex.collectionView.currentItem.maintenance_days_list;
        if (selectedWeekDays.indexOf(',') > 0) {

            var a = selectedWeekDays.split(",");

            for (var x of a) {

                this.splittedVlue = x.split(":");
                this.repeat = this.splittedVlue[1];
                for (var item of this.weekDays) {
                    if (item.dayNumber == this.splittedVlue[0]) {
                        this.weekDays[this.weekDays.indexOf(item)].isChecked = true;
                    }
                }
            }
        }
        else if (selectedWeekDays.indexOf(':') > 0) {
            this.splittedVlue = selectedWeekDays.split(":")
            this.repeat = this.splittedVlue[1];
            for (var item of this.weekDays) {
                if (item.dayNumber == this.splittedVlue[0]) {
                    this.weekDays[this.weekDays.indexOf(item)].isChecked = true;
                }
            }
        }
    }
    monthlyEditBinding() {
        var selectedWeekDays = this.flex.collectionView.currentItem.maintenance_days_list;
        if (selectedWeekDays.indexOf(',') > 0) {
            var a = selectedWeekDays.split(",");
            for (var x of a) {
                this.splittedVlue = x.split(":");
                this.repeat_monthly = this.splittedVlue[1];
                if (this.splittedVlue[1] == "specific_day") {
                    this.day_of_the_month = this.splittedVlue[0]
                }
                for (var item of this.weekDays) {
                    if (item.dayNumber == this.splittedVlue[0]) {
                        this.weekDays[this.weekDays.indexOf(item)].isChecked = true;
                    }
                }
            }
        }
        else if (selectedWeekDays.indexOf(':') > 0) {
            this.splittedVlue = selectedWeekDays.split(":")
            this.repeat_monthly = this.splittedVlue[1];
            if (this.splittedVlue[1] == "specific_day") {
                this.day_of_the_month = this.splittedVlue[0]
            }
            for (var item of this.weekDays) {
                if (item.dayNumber == this.splittedVlue[0]) {
                    this.weekDays[this.weekDays.indexOf(item)].isChecked = true;
                }
            }
        }
    }

    saveMaintenance(dlg: wijmo.input.Popup) {
        this.loading = true;
        this.keyUsers = [];
        this.refreshCheckedUsers();
        var selectedWeekDays = "";
        if (this.selectedSetting == "3") {
            for (var item of this.weekDays) {
                if (item.isChecked == true) {
                    if (selectedWeekDays == "") {
                        selectedWeekDays = item.dayNumber + ":" + this.repeat;
                    }
                    else {
                        selectedWeekDays += "," + item.dayNumber + ":" + this.repeat;
                    }
                }
            }
        }

        if (this.selectedSetting == "4") {
            for (var item of this.weekDays) {
                if (item.isChecked == true) {
                    if (selectedWeekDays == "") {
                        selectedWeekDays = item.dayNumber + ":" + this.repeat_monthly;
                    }
                    else {
                        selectedWeekDays += "," + item.dayNumber + ":" + this.repeat_monthly;
                    }
                }
            }

            if (this.repeat_monthly == "specific_day") {
                selectedWeekDays = this.day_of_the_month + ":" + "specific_day";
            }
        }
        this.formObject.start_time = this.wjStartTimeCtrl.selectedValue;
        var result = new Date(new Date().toDateString() + ' ' + this.formObject.start_time);
        var endTime = new Date(result.getTime() + this.formObject.duration * 60000);
        var sHours = endTime.getHours().toString();
        var sMinutes = endTime.getMinutes().toString();
        if (endTime.getHours() < 10) sHours = "0" + sHours;
        if (endTime.getMinutes() < 10) sMinutes = "0" + sMinutes;
        this.formObject.end_time = sHours + ':' + sMinutes;
        this.formObject.maintenance_days_list = selectedWeekDays;
        this.formObject.maintain_type = this.selectedSetting;
        this.formObject.duration_type = this.durationSetting;
        if (this.durationSetting == '2') {
            var d = new Date();        
            d.setFullYear(2999, 0, 1);       
            this.formObject.end_date = d; 
        }
        this.formObject.key_users = this.keyUsers;
        this.formObject.device_list = this.checkedDevices;

        if (this.keyUsers.length > 0 || this.checkedDevices.length > 0) {
            if (this.formObject.id == "") {
                this.formObject.start_date = new Date(Date.UTC(this.formObject.start_date.getFullYear(), this.formObject.start_date.getMonth(), this.formObject.start_date.getDate()));
                this.formObject.end_date = new Date(Date.UTC(this.formObject.end_date.getFullYear(), this.formObject.end_date.getMonth(), this.formObject.end_date.getDate()));

                this.service.put('/configurator/save_maintenancedata', this.formObject)
                    .subscribe(
                    response => {
                        if (response.status == "Success") {

                            this.datetimeHelpers.nameToFormat['start_date'] = "date";
                            this.datetimeHelpers.nameToFormat['start_time'] = "time";
                            this.datetimeHelpers.nameToFormat['end_date'] = "date";
                            this.datetimeHelpers.nameToFormat['end_time'] = "time";

                            response.data.forEach(function (entry) {
                                var dt = new Date(entry.start_date);
                                if (dt.getUTCHours() == 0 && dt.getUTCMinutes() == 0 && dt.getUTCSeconds() == 0 && dt.getUTCMilliseconds() == 0) {
                                    var dt2 = new Date(dt.getUTCFullYear(), dt.getUTCMonth(), dt.getUTCDate());
                                    entry.start_date = dt2.toISOString();
                                }

                                dt = new Date(entry.end_date);
                                if (dt.getUTCHours() == 0 && dt.getUTCMinutes() == 0 && dt.getUTCSeconds() == 0 && dt.getUTCMilliseconds() == 0) {
                                    var dt2 = new Date(dt.getUTCFullYear(), dt.getUTCMonth(), dt.getUTCDate());
                                    entry.end_date = dt2.toISOString();
                                }
                            });
                            this.data = this.datetimeHelpers.toLocal(response.data);
                            //this.data.pageSize = 10;
                            (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew();
                            //(<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection).moveToFirstPage();    
                            dlg.hide();
                            this.appComponentService.showSuccessMessage(response.message);
                            this.loading = false;
                        }
                        else {
                            this.appComponentService.showErrorMessage(response.message);
                            this.loading = false;
                        }
                    });
            }
            else {
                this.formObject.start_date = new Date(Date.UTC(this.formObject.start_date.getFullYear(), this.formObject.start_date.getMonth(), this.formObject.start_date.getDate()));
                this.formObject.end_date = new Date(Date.UTC(this.formObject.end_date.getFullYear(), this.formObject.end_date.getMonth(), this.formObject.end_date.getDate()));

                this.service.put('/configurator/save_maintenancedata', this.formObject)
                    .subscribe(
                    response => {
                        if (response.status == "Success") {

                            this.datetimeHelpers.nameToFormat['start_date'] = "date";
                            this.datetimeHelpers.nameToFormat['start_time'] = "time";
                            this.datetimeHelpers.nameToFormat['end_date'] = "date";
                            this.datetimeHelpers.nameToFormat['end_time'] = "time";

                            response.data.forEach(function (entry) {
                                var dt = new Date(entry.start_date);
                                if (dt.getUTCHours() == 0 && dt.getUTCMinutes() == 0 && dt.getUTCSeconds() == 0 && dt.getUTCMilliseconds() == 0) {
                                    var dt2 = new Date(dt.getUTCFullYear(), dt.getUTCMonth(), dt.getUTCDate());
                                    entry.start_date = dt2.toISOString();
                                }

                                dt = new Date(entry.end_date);
                                if (dt.getUTCHours() == 0 && dt.getUTCMinutes() == 0 && dt.getUTCSeconds() == 0 && dt.getUTCMilliseconds() == 0) {
                                    var dt2 = new Date(dt.getUTCFullYear(), dt.getUTCMonth(), dt.getUTCDate());
                                    entry.end_date = dt2.toISOString();
                                }
                            });

                            this.data = this.datetimeHelpers.toLocal(response.data);
                            //this.data.pageSize = 10;
                            (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit();
                            //(<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection).moveToFirstPage();    
                            dlg.hide();
                            this.appComponentService.showSuccessMessage(response.message);
                            this.loading = false;
                        }
                        else {
                            this.appComponentService.showErrorMessage(response.message);
                            this.loading = false;
                        }
                    });
            }
        }
        else {
            this.appComponentService.showErrorMessage("Please select at least one Key User or one Server");
            this.loading = false;
        }
    }

    addMaintenance(dlg: wijmo.input.Popup) {
        this.formTitle = "Add " + this.formName;
        this.formObject.id = "";
        this.formObject.name = "";
        this.formObject.start_date = new Date();
        this.formObject.start_time = "12:00 AM";
        if (this.wjStartTimeCtrl != null) {
            this.wjStartTimeCtrl.value = this.formObject.start_time;
        }
        this.formObject.end_date = new Date();
        this.formObject.end_time = "12:00";
        this.formObject.duration = "0";
        this.formObject.key_users = [];
        this.selectedSetting = "1";
        this.durationSetting = "1";
        this.devices = [];
        this._deviceList = this.devices;
        this.checkedDevices = this._deviceList;
        this.keyUsers = [];
        this.keyUsersGridBind();
        this.showDialog(dlg);
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

    deleteMaintenance() {
        this.delteGridRow('/Configurator/delete_maintenancedata/');
    }

    changeInDevices(devices: string) {
        this.checkedDevices = devices;
    }

    get dataMobileUserspageSize(): number {
        return this.dataMobileUsers.pageSize;
    }

    set dataMobileUserspageSize(value: number) {

        if (this.flex1) {
            if (this.dataMobileUsers.pageSize != value) {
                this.dataMobileUsers.pageSize = value;
                if (this.flex1) {
                    (<wijmo.collections.IPagedCollectionView>this.flex1.collectionView).pageSize = value;
                }
            }
        }

    }

    convertTimeformat(format, str): string {
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
        return sHours + ":" + sMinutes;
    }

    refreshCheckedUsers() {
        if (this.flex1.collectionView) {
            if (this.flex1.collectionView.items.length > 0) {
                //(<wijmo.collections.CollectionView>this.flex1.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex1.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex1.collectionView.sourceCollection)[_i];
                    var val = this.keyUsers.filter((record) => record == item.id);
                    if (item.is_selected && val.length == 0) {
                        this.keyUsers.push(item.id);
                    }
                }
            }
        }
    }
}



