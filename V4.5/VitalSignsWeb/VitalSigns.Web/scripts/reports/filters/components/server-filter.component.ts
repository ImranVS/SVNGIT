import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

@Component({
    selector: 'server-filter',
    templateUrl: '/app/reports/filters/components/server-filter.component.html',
})
export class ServerFilter {
    @Output() select: EventEmitter<string> = new EventEmitter<string>();
    @ViewChildren('multisel1') multisel1: wijmo.input.MultiSelect;
    selectedServers: any;
    serverType: string;
    statName: string;
    dropdownvalue: string;
    @Input() deviceType: any; 
    @Input() widgetName: string;
    @Input() widgetURL: string;
    @Input() hideDatePanel: boolean;
    @Input() showSingleDatePanel: boolean;
    @Input() hideServerControl: boolean;
    @Input() showdisksdropdown: boolean;
    @Input() HideSingleDTControl: boolean;
   
    endDate: Date = new Date();
    startDate: Date = new Date(this.endDate.getFullYear() ,this.endDate.getMonth(),this.endDate.getDate()-7);
    currentDate: Date = new Date();
    deviceNameData: any;
    errorMessage: any;
    
    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) { }

    get serviceId(): string {

        return this.widgetService.getProperty('serviceId');

    }

    set serviceId(id: string) {

        this.widgetService.setProperty('serviceId', id);

        this.select.emit(this.widgetService.getProperty('serviceId'));

    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            this.statName = params['statname'];
        });
        this.service.get(`/services/server_list_dropdown?type=${this.deviceType}`)
            .subscribe(
            (response) => {
                this.deviceNameData = response.data.deviceNameData;
            },
            (error) => this.errorMessage = <any>error
        );
        if (this.hideDatePanel == true) {
            var v = <HTMLDivElement>document.getElementById("dtPanel");
            v.style.display = "none";
            var v1 = <HTMLDivElement>document.getElementById("dtPanel2");
            v1.style.display = "none";
        }
        
        if (this.hideServerControl == true) {
            var v1 = <HTMLDivElement>document.getElementById("dtServer");
            v1.style.display = "none";
        }
        //if (this.hideServerControl == true && this.hideServerControl == true) {
        //    var v2 = <HTMLDivElement>document.getElementById("dtFilter");
        //    v2.style.display = "none";
        //}
        if (this.showSingleDatePanel == true) {
            var v = <HTMLDivElement>document.getElementById("dtPanel3");
            v.style.display = "inline-block";
        }

        if (this.showdisksdropdown == true) {
            var v = <HTMLDivElement>document.getElementById("dtSort");
            v.style.display = "inline-block";
        }
        if (this.HideSingleDTControl == true) {
            var v = <HTMLDivElement>document.getElementById("dtDate");
            v.style.display = "none";
        }
        //Set a selected value of the Status drop down box to the passed query parameter or -All- if no parameter is available
        //this.deviceStatus = paramstatus;
    }
    onChange(value) {

        console.log(value)
        this.dropdownvalue = value;

    }
    applyFilters(multisel1: wijmo.input.MultiSelect) {
        //var v = multisel1.checkedItems;
        var selectedServers = "";
        for (var item of multisel1.checkedItems) {
            if (selectedServers == "") 
                selectedServers = item.id;
            else 
                selectedServers += "," + item.id;
        }
        this.serviceId = selectedServers;
        var selStartDate = (this.startDate.getDate()).toString();
        var selStartMonth = (this.startDate.getMonth()+1).toString();
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
        
        var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `deviceId=` + selectedServers + `&startDate=` + newStartDate.toISOString() + `&endDate=` + newEndDate.toISOString();
        if (this.showSingleDatePanel)
            URL += "&date=" + newCurrentDate.toISOString();
        if (this.showdisksdropdown)
            URL += "&ismonitored=" + this.dropdownvalue;
        this.widgetService.refreshWidget(this.widgetName, URL )
            .catch(error => console.log(error));

    }

}