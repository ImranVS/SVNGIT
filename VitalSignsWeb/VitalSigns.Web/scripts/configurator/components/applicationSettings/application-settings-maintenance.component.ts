import {Component, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';


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
    selectedSettingValue: any;
    selectedDays: any;
    weekDays: any;
    repeat: any;
    repeat_monthly: any;
    end_date: any;
    day_of_the_month: any;
    splittedVlue: string;
   
    //currentDeviceType: string = ""
    
   
  keyUsers: string[] = []; 
    


    constructor(service: RESTService) {
        super(service);
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
            this.keyUsers.push(value);
        else {
            this.keyUsers.splice(this.keyUsers.indexOf(value), 1);
        }
    }

    ngOnInit() {
        this.initialGridBind('/Configurator/get_maintenance');
    }

    keyUsersGridBind() {
        this.service.get('/Dashboard/mobile_user_devices')
            .subscribe(
            (response) => {
                console.log("Key Users :" + this.keyUsers);
                var resultData: any = [];
                for (var item of response.data) {
                    if (this.keyUsers) {
                        var value = this.keyUsers.filter((record) => record==item.id);
                        console.log(item.id);
                        console.log(value);
                        if (value.length > 0) {
                            item.is_selected = true;
                        }
                    }
                    resultData.push(item);
                }

                console.log(resultData);
                this.dataMobileUsers = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(resultData));
                this.dataMobileUsers.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );
    }
    editMaintenance(dlg: wijmo.input.Popup) {
        this.editGridRow(dlg);

        var selectedWeekDays = this.currentEditItem.maintenance_days_list;
       // alert(selectedWeekDays);
        console.log(selectedWeekDays);
        this.splittedVlue = selectedWeekDays.split(":");
        //alert(this.splittedVlue);
        console.log(this.splittedVlue);
        this.selectedSetting = this.currentEditItem.maintain_type_value;
        console.log(this.selectedSetting);
       this.durationSetting = this.currentEditItem.duration_type;
       this.keyUsers = this.currentEditItem.key_users ;
       this.devices = this.currentEditItem.device_list;
       this.keyUsersGridBind();
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

                        console.log(selectedWeekDays)

                    }

                    else {

                        selectedWeekDays += "," + item.dayNumber + ":" + this.repeat;
                        console.log(selectedWeekDays)

                    }
                   
                  
                }
              
            
            }

       

        }

   
        if (this.selectedSetting == "4") {

            for (var item of this.weekDays) {

                if (item.isChecked == true) {
                    if (selectedWeekDays == "") {

                        selectedWeekDays = item.dayNumber + ":" + this.repeat_monthly;

                        console.log(selectedWeekDays)

                    }

                    else {

                        selectedWeekDays += "," + item.dayNumber + ":" + this.repeat_monthly;
                        console.log(selectedWeekDays)

                    }


                }


            }


            if (this.repeat_monthly == "specific_day") {

                selectedWeekDays = this.day_of_the_month + ":" + "specific_day";
                console.log(selectedWeekDays)
            }

        }

       
      
        this.currentEditItem.maintenance_days_list = selectedWeekDays;
        this.currentEditItem.maintain_type = this.selectedSetting;
        this.currentEditItem.duration_type = this.durationSetting;
        this.currentEditItem.key_users = this.keyUsers;
        this.currentEditItem.device_list = this.devices;
        console.log(this.devices)
        

        //if (this.durationSetting == 1) {

        //    this.currentEditItem.end_date = this.end_date;

        //}
        //else {

        //    this.currentEditItem.end_date = null;

        //}

        this.saveGridRow('/Configurator/save_maintenancedata',dlg);  
    }

    addMaintenance(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.name = "";
        this.currentEditItem.start_date = "";
        this.currentEditItem.end_date = "";
        this.currentEditItem.start_time = "";
        this.currentEditItem.end_time = "";
        this.currentEditItem.duration = "";
       

    }

    deleteMaintenance() {      
        this.delteGridRow('/Configurator/delete_maintenancedata/');  
    }

    changeInDevices(devices: string) {
        this.devices = devices;
    }
}



