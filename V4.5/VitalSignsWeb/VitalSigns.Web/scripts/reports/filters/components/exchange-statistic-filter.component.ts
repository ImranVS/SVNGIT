import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

@Component({
    selector: 'exchange-statistic-filter',
    templateUrl: '/app/reports/filters/components/exchange-statistic-filter.component.html',
})
export class ExchangeStatisticFilter {
   
    statisticTypeData: any;
    //statisticTypeDropdown: any;
    statisticData: any;
    statisticDropdown: any;
    @Output() select: EventEmitter<string> = new EventEmitter<string>();
    @ViewChildren('multisel1') multisel1: wijmo.input.MultiSelect;
    @ViewChildren('multisel2') multisel2: wijmo.input.MultiSelect;
    selectedServers: any;
    selectedStatistic: any;
    serverType: string;
    statName: string;
    @Input() deviceType: any; 
    @Input() widgetName: string;
    @Input() widgetURL: string;
    endDate: Date = new Date();
    startDate: Date = new Date(this.endDate.getFullYear() ,this.endDate.getMonth(),this.endDate.getDate()-7);
    currentDate: Date = new Date();
    deviceNameData: any;
    deviceNameData1: any;
    errorMessage: any;
   

    get serviceId(): string {

        return this.widgetService.getProperty('serviceId');

    }

    set serviceId(id: string) {

        this.widgetService.setProperty('serviceId', id);

        this.select.emit(this.widgetService.getProperty('serviceId'));

    }

    get statisticId(): string {

        return this.widgetService.getProperty('serviceId');

    }

    set statisticId(id: string) {

        this.widgetService.setProperty('serviceId', id);

        this.select.emit(this.widgetService.getProperty('serviceId'));

    }
    get gridUrl(): string {

        return this.widgetService.getProperty('gridUrl');

    }

    set gridUrl(id: string) {

        this.widgetService.setProperty('gridUrl', id);

        this.select.emit(this.widgetService.getProperty('gridUrl'));

    }

    set widgetTitle(name: string) {

        this.widgetService.setProperty('widgetTitle', name);

        this.select.emit(this.widgetService.getProperty('widgetTitle'));

    }
    
    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) { }
    ngOnInit() {

        this.route.queryParams.subscribe(params => {
            this.statName = params['statname'];
        });
        this.service.get(`/services/server_list_dropdown?type=Exchange`)
            .subscribe(
            (response) => {
                this.deviceNameData = response.data.deviceNameData;
            },
            (error) => this.errorMessage = <any>error
            ); 

        this.deviceNameData1 = [
            { name: "All", id: "All" },
            { name: "Delivery Success Rate", id: "Mail_DeliverySuccessRate" },
            { name: "Delivery Count", id: "Mail_DeliverCount" },
            { name: "Received Count", id: "Mail_ReceivedCount" },
            { name: "Fail Count", id: "Mail_FailCount" },
            { name: "Received Size MB", id: "Mail_ReceivedSizeMB" },
            { name: "Sent Count", id: "Mail_SentCount" },
            { name: "Sent Size MB", id: "Mail_SentSizeMB" },
        ];
    }
    
   
    applyFilters(multisel1: wijmo.input.MultiSelect, multisel2: wijmo.input.MultiSelect) {
      
        var selectedServers = "";
        for (var item of multisel1.checkedItems) {
            if (selectedServers == "")
                selectedServers = item.id;
            else
                selectedServers += "," + item.id;
        }
        this.serviceId = selectedServers;


        var selectedStatistic = "";
        for (var item of multisel2.checkedItems) {
            if (selectedStatistic == "")
                selectedStatistic = item.id;
            else
                selectedStatistic += "," + item.id;
        }
        this.statisticId = selectedStatistic;



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
        if (this.statisticId === "All") {
            this.statisticId= "Mail_DeliverySuccessRate,Mail_DeliverCount,Mail_ReceivedCount,Mail_FailCount,Mail_ReceivedSizeMB,Mail_SentCount,Mail_SentSizeMB";
        }
        var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `startDate=` + newStartDate.toISOString() + `&endDate=` + newEndDate.toISOString() + `&statName=[` + this.statisticId +`]`;

        this.widgetService.refreshWidget(this.widgetName, URL)
            .catch(error => console.log(error));
    }
}