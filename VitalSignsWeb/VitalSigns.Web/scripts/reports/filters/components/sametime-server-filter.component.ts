import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

@Component({
    selector: 'sametime-server-filter',
    templateUrl: '/app/reports/filters/components/sametime-server-filter.component.html',
})
export class SametimeServerFilter {
    //@ViewChildren('multisel1') multisel1: wijmo.input.MultiSelect;
    @Output() select: EventEmitter<string> = new EventEmitter<string>();
    selectedServers: any;
    serverType: string;
    statName: string;
    @Input() widgetName: string;
    @Input() widgetURL: string;
    @Input() hideStatDropdown: boolean;
    endDate: Date = new Date();
    startDate: Date = new Date(this.endDate.getFullYear() ,this.endDate.getMonth(),this.endDate.getDate()-7);
    currentDate: Date = new Date();
    deviceNameData: any;
    errorMessage: any;
    statisticDropdown: string;


    get gridUrl(): string {

        return this.widgetService.getProperty('gridUrl');

    }

    set gridUrl(id: string) {

        this.widgetService.setProperty('gridUrl', id);

        this.select.emit(this.widgetService.getProperty('gridUrl'));

    }
    
    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) { }
    ngOnInit() {

        this.route.queryParams.subscribe(params => {
            this.statName = params['statname'];
        });
        //this.service.get(`/services/server_list_dropdown?type=${this.deviceType}`)
        //    .subscribe(
        //    (response) => {
        //        this.deviceNameData = response.data.deviceNameData;
        //    },
        //    (error) => this.errorMessage = <any>error
        ////);
        if (this.hideStatDropdown == true) {
            var v = <HTMLDivElement>document.getElementById("dtServer");
            v.style.display = "none";
        }
        
    }
    applyFilters() {  
        //var v = multisel1.checkedItems;
       
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
        
        //var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `deviceId=` + selectedServers + `&startDate=` + newStartDate.toISOString() + `&endDate=` + newEndDate.toISOString();
        //if (this.showSingleDatePanel)
        //    URL += "&date=" + newCurrentDate.toISOString();
        //if (this.statName != "")
        //    URL += "&statName=" + this.statName;
        var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `startDate=` + newStartDate.toISOString() + `&endDate=` + newEndDate.toISOString();
        if (this.hideStatDropdown == false)
            URL += `&statName=` + this.statisticDropdown;
   
        //});
        //this.widgetService.refreshWidget('avgcpuutilchart', `/reports/summarystats_chart?statName=Platform.System.PctCombinedCpuUtil&deviceId=` + selectedServers + `&start=` + this.startDate.toISOString() + `&end=` + this.endDate.toISOString())
        //    .catch(error => console.log(error));
        //var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + "Temp";
        var arr = Array.from(document.getElementById(this.widgetName).childNodes)
        //arr.forEach(function (x) { console.log(x); } );
        //arr.forEach(function (x) { console.log(x.firstChild != null ? x.firstChild.localName.toString() : "false") })
        if (arr.length > 1 && arr.some(function (x) { return (x.firstChild != null ? x.firstChild.localName.toString().includes("wj-flex-grid") : false); })) {
            this.gridUrl = URL;
        } else {
            this.widgetService.refreshWidget(this.widgetName, URL)
                .catch(error => console.log(error));
        }
        

    }

}