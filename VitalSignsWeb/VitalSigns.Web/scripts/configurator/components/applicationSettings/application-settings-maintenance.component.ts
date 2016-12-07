import {Component, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';



@Component({
    templateUrl: '/app/configurator/components/applicationsettings/application-settings-maintenance.component.html', 
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Maintenance extends GridBase implements OnInit  {  
    devices: string;
    
   errorMessage: string;
   dataMobileUsers: wijmo.collections.CollectionView;
   selectedSetting: string="1";
   durationSetting: any;
   durationSetting1: any;
   durationSetting2: any;
    selectedSettingValue: any;
    selectedDays: any;
    weekDays: any;
    repeat: any;
    repeat_monthly: any;
    end_date: any;
    day_of_the_month: any;
    splittedVlue: string;
    checkedDevices: any;
   
  keyUsers: string[] = []; 
    


  constructor(service: RESTService, appComponentService: AppComponentService) {
      super(service, appComponentService);
        this.formName = "Maintenance";
        this.weekDays = [
            { weekday: "Sunday", dayNumber: "7", isChecked:false},
            { weekday: "Monday", dayNumber: "1", isChecked: false },
            { weekday: "Tuesday", dayNumber: "2", isChecked: false },
            { weekday: "Wednesday", dayNumber: "3", isChecked: false },
            { weekday: "Thursday", dayNumber: "4", isChecked: false },
            { weekday: "Friday", dayNumber: "5", isChecked: false },
            { weekday: "Saturday", dayNumber: "6", isChecked: false},
        ];
      
    } 

    keyUsersCheck(value, event) {

        if (event.target.checked)
        {
            this.keyUsers.push(value);
            this.flex1.collectionView.currentItem.is_selected = true;
        }
        else {
            this.keyUsers.splice(this.keyUsers.indexOf(value), 1);
            this.flex1.collectionView.currentItem.is_selected = false;
        }
    }

    ngOnInit() {
        this.initialGridBind('/Configurator/get_maintenance');
        this.keyUsersGridBind();
        
    }

    handleClickMonthly(index: any) 
    {
        if (this.currentEditItem.maintain_type_value != "4") {
            for (var i = 0; i < 7; i++) {
                this.weekDays[i].isChecked = false;
            }
        }
        else if (this.currentEditItem.maintain_type_value == "4") {
            this.monthlyEditBinding();
        }

    }

   

    handleClickWeekly(index: any) {

        if (this.currentEditItem.maintain_type_value != "3") {
            for (var i = 0; i < 7; i++) {
                this.weekDays[i].isChecked = false;
            }
        }
        else if (this.currentEditItem.maintain_type_value == "3") {
            this.weeklyEditBinding();
        }
    }

    keyUsersGridBind() {
        this.service.get('/Dashboard/mobile_user_devices')
            .subscribe(
            (response) => {
                var resultData: any = [];
                for (var item of response.data) {
                    if (this.keyUsers) {
                        var value = this.keyUsers.filter((record) => record==item.id);
                        if (value.length > 0) {
                            item.is_selected = true;
                        }
                    }
                    resultData.push(item);
                }
                
                this.dataMobileUsers = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(resultData));
                this.dataMobileUsers.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );
    }
    editMaintenance(dlg: wijmo.input.Popup) {
        this.editGridRow(dlg);
        this.repeat = "";
        this.repeat_monthly = "";
        this.day_of_the_month = "";
        this.splittedVlue = "";

        for (var item of this.weekDays) {
            this.weekDays[this.weekDays.indexOf(item)].isChecked = false;
        }
        
        if (this.currentEditItem.maintain_type_value == "3") {
            this.weeklyEditBinding(); 
        }

        if (this.currentEditItem.maintain_type_value == "4") {
            this.monthlyEditBinding();
        }

  
        this.selectedSetting = this.currentEditItem.maintain_type_value;
        this.durationSetting = this.currentEditItem.duration_type;
        this.keyUsers = this.currentEditItem.key_users;
        console.log(this.currentEditItem.device_list + "serverslist");
        this.checkedDevices = this.currentEditItem.device_list;
        this.keyUsersGridBind();
    }
    weeklyEditBinding()
    {
        var selectedWeekDays = this.currentEditItem.maintenance_days_list;
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
        var selectedWeekDays = this.currentEditItem.maintenance_days_list;
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
        var selectedWeekDays = "";
        this.currentEditItem.maintain_type = this.selectedSetting;
        if (this.selectedSetting == "3") {
            for (var item of this.weekDays) {
               if (item.isChecked == true )
                {
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
        
        this.currentEditItem.maintenance_days_list = selectedWeekDays;
        this.currentEditItem.maintain_type = this.selectedSetting;
        this.currentEditItem.duration_type = this.durationSetting;
        this.currentEditItem.key_users = this.keyUsers;
        this.currentEditItem.device_list = this.devices;
        console.log(this.currentEditItem.key_users.count +"count")
        if (this.keyUsers.length > 0 || this.devices.length > 0) {

            this.saveGridRow('/Configurator/save_maintenancedata', dlg);
        }
        else {

            alert("Please select at least one Key User or one Server");
        }
        if (this.selectedSetting == "1") {
            this.currentEditItem.maintain_type = "One Time";
        }
        else if (this.selectedSetting == "2")
        {
            this.currentEditItem.maintain_type = "Daily";
        }
        else if (this.selectedSetting == "3")
        {
            this.currentEditItem.maintain_type = "Weekly";
        }
        else {
            this.currentEditItem.maintain_type = "Monthly";
        }
    }

    addMaintenance(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
      
        this.selectedSetting = "1";
        this.durationSetting = "1";

        this.devices = "";
        this.keyUsers = [];
       
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
        this.devices = devices;
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
}



