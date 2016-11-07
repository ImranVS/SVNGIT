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
    sunday: any;
    monday: any;
    tuesday: any;
    wednesday: any;
    thursday: any;
    friday: any;
    saturday: any;
    repeat: any;
    repeat_monthly: any;
    end_date: any;
    i: any;


    constructor(service: RESTService) {
        super(service);
        this.formName = "Maintenance";
      
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
       

        this.currentEditItem.maintain_type = this.selectedSetting;
      

        if (this.selectedSetting == "2" || "3" || "4") {

            if (this.durationSetting == "1")
                this.currentEditItem.end_date = this.end_date;
            else
                this.currentEditItem.end_date = "";
           
        }

       
        if (this.selectedSetting == "3") {
            //if (this.sunday == true)
            //    this.sunday = "7";
            //alert(this.sunday);
            //this.currentEditItem.maintenance_days_list = this.sunday
            //this.currentEditItem.maintenance_days_list += ":" + this.repeat;
            //alert(this.currentEditItem.maintenance_days_list)

            if (this.sunday == true || this.monday == true || this.tuesday == true || this.wednesday == true || this.thursday == true || this.friday == true) {

                if (this.sunday == true)
                {
                    this.sunday = "7";
                    this.currentEditItem.maintenance_days_list = this.sunday + ":" + this.repeat;
                    alert(this.currentEditItem.maintenance_days_list)
                }

                if (this.monday == true) {
                    this.monday = "1";
                    this.currentEditItem.maintenance_days_list = this.monday + ":" + this.repeat;
                    alert(this.currentEditItem.maintenance_days_list)
                }

                if (this.tuesday == true) {
                    this.tuesday = "2";
                    this.currentEditItem.maintenance_days_list = this.tuesday + ":" + this.repeat;
                    alert(this.currentEditItem.maintenance_days_list)
                }

                if (this.wednesday == true) {
                    this.wednesday = "3";
                    this.currentEditItem.maintenance_days_list = this.wednesday + ":" + this.repeat;
                    alert(this.currentEditItem.maintenance_days_list)
                }

                if (this.thursday == true) {
                    this.thursday = "4";
                    this.currentEditItem.maintenance_days_list = this.thursday + ":" + this.repeat;
                    alert(this.currentEditItem.maintenance_days_list)
                }

                if (this.friday == true) {
                    this.friday = "5";
                    this.currentEditItem.maintenance_days_list = this.friday + ":" + this.repeat;
                    alert(this.currentEditItem.maintenance_days_list)
                }
                if (this.saturday == true) {
                    this.saturday = "6";
                    this.currentEditItem.maintenance_days_list = this.saturday + ":" + this.repeat;
                    alert(this.currentEditItem.maintenance_days_list)
                }

                 if (this.sunday == true && this.monday == true) 
                    this.sunday = "7";
                    this.monday = "1";
                    this.currentEditItem.maintenance_days_list = this.sunday + ":" + this.repeat;
                    this.currentEditItem.maintenance_days_list += "," + this.monday + ":" + this.repeat;
                    alert(this.currentEditItem.maintenance_days_list)
                
                if (this.sunday == true && this.monday == true && this.tuesday == true) 
                    this.sunday = "7";
                    this.monday = "1";
                    this.tuesday = "2";
                    this.currentEditItem.maintenance_days_list = this.sunday + ":" + this.repeat;
                    this.currentEditItem.maintenance_days_list += "," + this.monday + ":" + this.repeat;
                    this.currentEditItem.maintenance_days_list += "," + this.tuesday + ":" + this.repeat;
                    alert(this.currentEditItem.maintenance_days_list)
                
                    if (this.sunday == true && this.monday == true && this.tuesday == true && this.wednesday == true) 
                        this.sunday = "7";
                    this.monday = "1";
                    this.tuesday = "2";
                    this.wednesday = "3";
                    this.currentEditItem.maintenance_days_list = this.sunday + ":" + this.repeat;
                    this.currentEditItem.maintenance_days_list += "," + this.monday + ":" + this.repeat;
                    this.currentEditItem.maintenance_days_list += "," + this.tuesday + ":" + this.repeat;
                   this.currentEditItem.maintenance_days_list += "," + this.wednesday + ":" + this.repeat;
                 alert(this.currentEditItem.maintenance_days_list)
                
                 if (this.sunday == true && this.monday == true && this.tuesday == true && this.wednesday == true && this.thursday == true) 
                     this.sunday = "7";
                     this.monday = "1";
                     this.tuesday = "2";
                     this.wednesday = "3";
                     this.thursday = "4";
                     this.currentEditItem.maintenance_days_list = this.sunday + ":" + this.repeat;
                     this.currentEditItem.maintenance_days_list += "," + this.monday + ":" + this.repeat;
                     this.currentEditItem.maintenance_days_list += "," + this.tuesday + ":" + this.repeat;
                     this.currentEditItem.maintenance_days_list += "," + this.wednesday + ":" + this.repeat;
                     this.currentEditItem.maintenance_days_list += "," + this.thursday + ":" + this.repeat;
                 alert(this.currentEditItem.maintenance_days_list)
                
                 if (this.sunday == true && this.monday == true && this.tuesday == true && this.wednesday == true && this.thursday == true && this.friday == true) 
                     this.sunday = "7";
                 this.monday = "1";
                 this.tuesday = "2";
                 this.wednesday = "3";
                 this.thursday = "4";
                 this.friday = "5";

                 this.currentEditItem.maintenance_days_list = this.sunday + ":" + this.repeat;
                 this.currentEditItem.maintenance_days_list += "," + this.monday + ":" + this.repeat;
                 this.currentEditItem.maintenance_days_list += "," + this.tuesday + ":" + this.repeat;
                 this.currentEditItem.maintenance_days_list += "," + this.wednesday + ":" + this.repeat;
                 this.currentEditItem.maintenance_days_list += "," + this.thursday + ":" + this.repeat;
                 this.currentEditItem.maintenance_days_list += "," + this.friday + ":" + this.repeat;
                 alert(this.currentEditItem.maintenance_days_list)
                
                 if (this.sunday == true && this.monday == true && this.tuesday == true && this.wednesday == true && this.thursday == true && this.friday == true && this.saturday == true) 
                     this.sunday = "7";
                     this.monday = "1";
                     this.tuesday = "2";
                     this.wednesday = "3";
                     this.thursday = "4";
                     this.friday = "5";
                     this.saturday = "6";
                     this.currentEditItem.maintenance_days_list = this.sunday + ":" + this.repeat;
                     this.currentEditItem.maintenance_days_list += "," + this.monday + ":" + this.repeat;
                     this.currentEditItem.maintenance_days_list += "," + this.tuesday + ":" + this.repeat;
                     this.currentEditItem.maintenance_days_list += "," + this.wednesday + ":" + this.repeat;
                     this.currentEditItem.maintenance_days_list += "," + this.thursday + ":" + this.repeat;
                     this.currentEditItem.maintenance_days_list += "," + this.friday + ":" + this.repeat;
                     this.currentEditItem.maintenance_days_list += "," + this.saturday + ":" + this.repeat;
                    alert(this.currentEditItem.maintenance_days_list)
                

               
            }

           //if (this.sunday == true && this.monday == true && this.tuesday == true && this.wednesday == true && this.thursday == true && this.friday == true)
            
           //     alert("sowjiiiiii");
           //     this.sunday = "7"
           //     this.monday = "1";
           //     this.tuesday = "2";
           //     this.wednesday = "3";
           //     this.thursday = "4";
           //     this.friday = "5";
           //     this.saturday = "6";

           //     this.currentEditItem.maintenance_days_list += this.sunday + ":" + this.repeat;
           //     this.currentEditItem.maintenance_days_list += "," + this.monday + ":" + this.repeat;
           //     this.currentEditItem.maintenance_days_list += "," + this.tuesday + ":" + this.repeat;
           //     this.currentEditItem.maintenance_days_list += "," + this.wednesday + ":" + this.repeat;
           //     this.currentEditItem.maintenance_days_list += "," + this.thursday + ":" + this.repeat;
           //     this.currentEditItem.maintenance_days_list += "," + this.friday + ":" + this.repeat;
           //     this.currentEditItem.maintenance_days_list += "," + this.saturday + ":" + this.repeat;
           //     alert(this.currentEditItem.maintenance_days_list)
            

        }

        else
        {
            this.currentEditItem.maintenance_days_list = "";
        }


        if (this.selectedSetting == "4") {
            if (this.sunday == true)
                this.sunday = "7";
            alert(this.sunday);
            this.currentEditItem.maintenance_days_list = this.sunday
            this.currentEditItem.maintenance_days_list += ":" + this.repeat_monthly;
            alert(this.currentEditItem.maintenance_days_list)

            if (this.sunday == true && this.monday == true && this.tuesday == true && this.wednesday == true && this.thursday == true && this.friday == true)
                this.sunday = "7"
            this.monday = "1";
            this.tuesday = "2";
            this.wednesday = "3";
            this.thursday = "4";
            this.friday = "5";
            this.saturday = "6";

            alert(this.monday);
            this.currentEditItem.maintenance_days_list = this.sunday
            this.currentEditItem.maintenance_days_list += ":" + this.repeat_monthly;
            this.currentEditItem.maintenance_days_list += "," + this.monday + ":" + this.repeat_monthly;
            this.currentEditItem.maintenance_days_list += "," + this.tuesday + ":" + this.repeat_monthly;
            this.currentEditItem.maintenance_days_list += "," + this.wednesday + ":" + this.repeat_monthly;
            this.currentEditItem.maintenance_days_list += "," + this.thursday + ":" + this.repeat_monthly;
            this.currentEditItem.maintenance_days_list += "," + this.friday + ":" + this.repeat_monthly;
            this.currentEditItem.maintenance_days_list += "," + this.saturday + ":" + this.repeat_monthly;
            alert(this.currentEditItem.maintenance_days_list)
        }

       
      
       
        this.saveGridRow('/Configurator/save_maintenancedata',dlg);  
    }
    deleteMaintenance() {      
        this.delteGridRow('/Configurator/delete_maintenancedata/');  
    }

    changeInDevices(devices: string) {
        this.devices = devices;
    }
}



