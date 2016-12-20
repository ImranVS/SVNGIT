import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent, WidgetService} from '../../../core/widgets';

@Component({
    selector: 'traveler-filter',
    templateUrl: '/app/reports/filters/components/traveler-filter.component.html',
})
export class TravelerFilter {
    @ViewChildren('interval_selector') interval_selector: wijmo.input.ComboBox;
    @ViewChildren('server_selector') server_selector: wijmo.input.ComboBox;
    selectedServers: any;
    serverType: string;
    statName: string;
    @Input() widgetName: string;
    @Input() widgetURL: string;
    @Input() hideDatePanel: boolean;
    @Input() hideSingleDatePanel: boolean;
    @Input() hideIntervalControl: boolean;
    @Input() hideMailServerControl: boolean;
    @Input() hideServerControl: boolean;
    @Input() hideAllServerControl: boolean;
    endDate: Date = new Date();
    startDate: Date = new Date(this.endDate.getFullYear(), this.endDate.getMonth(), this.endDate.getDate() - 7);
    currentDate: Date = new Date();
    servers: any;
    mail_servers: any;
    all_servers: any;
    errorMessage: any;
    intervals: any;

    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) {
        this.intervals = ["000-001", "001-002", "002-005", "005-010", "010-030", "030-060", "060-120", "120-Inf"];
    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            this.statName = params['statname'];
        });
        this.service.get(`/services/status_list?type=Traveler`)
            .subscribe(
            (response) => {
                this.servers = response.data;
            },
            (error) => this.errorMessage = <any>error
        );
        this.service.get(`/services/server_list_dropdown?type=Domino`)
            .subscribe(
            (response) => {
                this.mail_servers = response.data.deviceNameData;
                this.all_servers = response.data.deviceNameData;
            },
            (error) => this.errorMessage = <any>error
        );
        if (this.hideDatePanel == true) {
            var v1 = <HTMLDivElement>document.getElementById("dtStart");
            v1.style.display = "none";
            v1 = <HTMLDivElement>document.getElementById("dtEnd");
            v1.style.display = "none";
        }
        if (this.hideSingleDatePanel == true) {
            var v1 = <HTMLDivElement>document.getElementById("dtDate");
            v1.style.display = "none";
        }
        if (this.hideServerControl == true) {
            var v1 = <HTMLDivElement>document.getElementById("dtServer");
            v1.style.display = "none";
        }
        if (this.hideMailServerControl == true) {
            var v1 = <HTMLDivElement>document.getElementById("dtMailServer");
            v1.style.display = "none";
        }
        if (this.hideAllServerControl == true) {
            var v1 = <HTMLDivElement>document.getElementById("dtAllServer");
            v1.style.display = "none";
        }
        if (this.hideIntervalControl == true) {
            var v1 = <HTMLDivElement>document.getElementById("selInterval");
            v1.style.display = "none";
        }
       
    }
    applyFilters(server_sel: wijmo.input.ComboBox, interval_sel: wijmo.input.ComboBox, mail_server_sel: wijmo.input.ComboBox,
        all_server_sel: wijmo.input.ComboBox) {
        if (this.hideDatePanel == true && this.hideSingleDatePanel == true) {
            if (this.hideIntervalControl == false) {
                var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `deviceId=` + server_sel.selectedValue + `&paramvalue=` + interval_sel.selectedValue;          
                this.widgetService.refreshWidget(this.widgetName, URL)
                    .catch(error => console.log(error));
            }
            else if (this.hideMailServerControl == false && this.hideServerControl == false) {
                var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `deviceId=` + server_sel.selectedValue + `&paramvalue=` + mail_server_sel.selectedValue;
              
                this.widgetService.refreshWidget(this.widgetName, URL)
                    .catch(error => console.log(error));
            }
            else if (this.hideAllServerControl == false) {
                var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `deviceId=` + all_server_sel.selectedValue;
              
                this.widgetService.refreshWidget(this.widgetName, URL)
                    .catch(error => console.log(error));
            }
        }
        else {
            var selStartDate = (this.startDate.getDate()).toString();
            var selStartMonth = (this.startDate.getMonth() + 1).toString();
            if (selStartDate.length == 1)
                selStartDate = '0' + selStartDate;
            if (selStartMonth.length == 1)
                selStartMonth = '0' + selStartMonth;

            var selEndDate = (this.endDate.getDate()).toString();
            var selEndMonth = (this.endDate.getMonth() + 1).toString();
            if (selEndDate.length == 1)
                selEndDate = '0' + selEndDate;
            if (selEndMonth.length == 1)
                selEndMonth = '0' + selEndMonth;

            var currentStartDate = (this.currentDate.getDate()).toString();
            var currentStartMonth = (this.currentDate.getMonth() + 1).toString();
            if (currentStartDate.length == 1)
                currentStartDate = '0' + currentStartDate;
            if (currentStartMonth.length == 1)
                currentStartMonth = '0' + currentStartMonth;


            var startFinalDate = this.startDate.getFullYear().toString() + '-' + selStartMonth + '-' + selStartDate;
            var endFinalDate = this.endDate.getFullYear().toString() + '-' + selEndMonth + '-' + selEndDate;

            var newStartDate = new Date(this.startDate.getFullYear(), this.startDate.getMonth(), this.startDate.getDate());
            var newEndDate = new Date(this.endDate.getFullYear(), this.endDate.getMonth(), this.endDate.getDate());
            var newCurrentDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(), this.currentDate.getDate());

            if (this.hideSingleDatePanel == true) {
                if (this.hideServerControl == false){
                    var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `deviceId=` + server_sel.selectedValue + `&startDate=` + newStartDate.toISOString() + `&endDate=` + newEndDate.toISOString();

                    this.widgetService.refreshWidget(this.widgetName, URL)
                        .catch(error => console.log(error));
                }
                else {
                    var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `deviceId=` + all_server_sel.selectedValue + `&startDate=` + newStartDate.toISOString() + `&endDate=` + newEndDate.toISOString();

                    this.widgetService.refreshWidget(this.widgetName, URL)
                        .catch(error => console.log(error));
                }
            }
            else {
                var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `deviceId=` + all_server_sel.selectedValue + `&year=` + newCurrentDate.getFullYear();
             
                this.widgetService.refreshWidget(this.widgetName, URL)
                    .catch(error => console.log(error));
            }
            
        }

    }

}