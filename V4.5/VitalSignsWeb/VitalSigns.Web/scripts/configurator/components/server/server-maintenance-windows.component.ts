import {Component, OnInit, AfterViewInit, ViewChild, Output, EventEmitter} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {GridBase} from '../../../core/gridBase';
import {RESTService} from '../../../core/services';
import {DiskSttingsValue} from '../../models/server-disk-settings';

import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/server/server-maintenance-windows.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper
    ]
})
//export class ServerDiskSettings implements OnInit, AfterViewInit {
export class MaintenanceWindows implements OnInit {
    deviceId: any;
    from_date: any;
    to_date: any;
    from_time: any;
    to_time: any;
    fhour: string;
    thour: string;
    fhourInt: any;
    thourInt: any;
    fminute: any;
    tminute: any;
    fminuteInt: any;
    tminuteInt: any;
    maintenanceForm: FormGroup;
    module: any;
    _filter: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
     @ViewChild('flex') flex: wijmo.grid.FlexGrid;
     @ViewChild('filter') filter: wijmo.grid.filter.FlexGridFilter;
    constructor(
        private dataProvider: RESTService,
        private formBuilder: FormBuilder, private route: ActivatedRoute, private datetimeHelpers: helpers.DateTimeHelper) {

        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
           

        });

        this.maintenanceForm = this.formBuilder.group({
            'from_date': [''],
           

        });
       
        this.dataProvider.get('/Configurator/get_server_maintenancedata?id=' + this.deviceId)
            .subscribe(
            response => {
                this.datetimeHelpers.nameToFormat['StartDate'] = "date";
                this.datetimeHelpers.nameToFormat['EndDate'] = "date";
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                this.data.pageSize = 10;

            });

       
    }

    ngOnInit() {
        this.route.parent.params.subscribe(params => {

            this.module = params['module'];
        });
       
    }

   get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
   }

    //clearClick() {
    //    this.from_date = "";
    //    this.to_date = "";
    //    this.from_time = "";
    //    this.to_time = "";
    //    this.fillMaintenance();
        
       
    //}
    //dateChanged() {
    //  // this.filter.defaultFilterType = wijmo.grid.filter.FilterType.Condition;
    //    var col = this.flex.columns.getColumn("StartDate"),
    //       cf = this.filter.getColumnFilter(col);
    //    cf.filterType = wijmo.grid.filter.FilterType.Condition;
    //    cf.conditionFilter.condition1.operator = wijmo.grid.filter.Operator.GE;
    //    cf.conditionFilter.condition1.value = this.from_date;       
    //    cf.apply(this.from_date);
    //}
    //searchClick() {
    //    this.fillMaintenance();
    //}

    //fillMaintenance() {
       
    //    if (this.from_time == "undefined" || this.to_time == "undefined") {

    //        this.from_date = "";
    //        this.to_time = ""
    //    }

    //    if (this.from_date != "" && this.to_date != "") {
          
    //        if (this.from_date > this.to_date) {
    //            alert("From Date value should be less than To Date.");
    //        }
    //        else {
              
    //            if ((this.from_time != null && this.to_time != null) && (this.from_time != "" && this.to_time != "")) {                                  
    //                this.fhour = this.from_time.substring(0, this.from_time.indexOf(":"));
                   
    //                this.thour = this.to_time.substring(0, this.to_time.indexOf(":"));
    //                this.fhourInt = (this.fhour);
    //                this.thourInt = (this.thour);
    //                this.fminute = this.from_time.substring(3, 2);
    //                this.tminute = this.to_time.substring(3, 2);
    //                this.fminuteInt = (this.fminute);
    //                this.tminuteInt = (this.tminute);
                
    //                if (this.fhourInt >= 24 || this.thourInt >= 24 || this.fminuteInt >= 60 || this.tminuteInt >= 60) {
                      
    //                    alert("Invalid hour/minute entry.");
    //                }
    //                else {
                      
    //                    this.dataProvider.get('/Configurator/get_server_maintenancedata?id=' + this.deviceId + '&fromDate=' + this.from_date + '&toDate=' + this.to_date + '&fromTime=' + this.from_time + '&toTime=' + this.to_time)
    //                        .subscribe(
    //                        response => {
    //                            this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray((response.data)));
    //                            this.data.pageSize = 10;
    //                        });
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
         
    //        if ((this.from_time != null && this.to_time != null) && (this.from_time != "" && this.to_time != "")) {
    //            this.fhour = this.from_time.substring(0, this.from_time.indexOf(":"));
    //            this.thour = this.to_time.substring(0, this.to_time.IndexOf(":"));
    //            this.fhourInt = (this.fhour);
    //            this.thourInt = (this.thour);
    //            this.fminute = this.from_time.substring(3, 2);
    //            this.tminute = this.to_time.substring(3, 2);
    //            this.fminuteInt = (this.fminute);
    //            this.tminuteInt = (this.tminute);
    //            if (this.fhourInt >= 24 || this.thourInt >= 24 || this.fminuteInt >= 60 || this.tminuteInt >= 60) {
    //                alert("Invalid hour/minute entry.");
    //            }

    //            else {
    //                this.dataProvider.get('/Configurator/get_server_maintenancedata?id=' + this.deviceId + '&fromDate=' + this.from_date + '&toDate=' + this.to_date + '&fromTime=' + this.from_time + '&toTime=' + this.to_time)
    //                    .subscribe(
    //                    response => {
    //                        this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray((response.data)));
    //                        this.data.pageSize = 10;
    //                    });
    //            }
    //        }
         
    //        else {
    //            this.dataProvider.get('/Configurator/get_server_maintenancedata?id=' + this.deviceId + '&fromDate=' + this.from_date + '&toDate=' + this.to_date + '&fromTime=' + this.from_time + '&toTime=' + this.to_time)
    //                .subscribe(
    //                response => {
    //                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray((response.data)));
    //                    this.data.pageSize = 10;
    //                    });
    //        }
    //    }
    //}
}