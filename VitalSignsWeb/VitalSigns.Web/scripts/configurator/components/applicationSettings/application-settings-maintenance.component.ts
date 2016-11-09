import {Component, OnInit} from '@angular/core';
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
    i: any;
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

        this.service.get('/Dashboard/mobile_user_devices')
            .subscribe(
            (response) => {
                this.dataMobileUsers = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.dataMobileUsers.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );

      

    }
    saveMaintenance(dlg: wijmo.input.Popup) {
        var selectedWeekDays = "";

       
       
        //this.weekDays[0] == true
        //alert(this.weekDays.weekday)
        this.currentEditItem.maintain_type = this.selectedSetting;
      

        //if (this.selectedSetting == "2" || "3" || "4") {

        //    if (this.durationSetting == "1")
        //        this.currentEditItem.end_date = this.end_date;
        //    else
        //        this.currentEditItem.end_date = "";
           
        //}

       
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
    deleteMaintenance() {      
        this.delteGridRow('/Configurator/delete_maintenancedata/');  
    }

    changeInDevices(devices: string) {
        this.devices = devices;
    }
}



