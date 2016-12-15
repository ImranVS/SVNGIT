import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent, WidgetService} from '../../../core/widgets';

@Component({
    selector: 'server-configuration-filter',
    templateUrl: '/app/reports/filters/components/server-configuration-filter.component.html',
})
export class ServerConfigurationFilter {
    @ViewChildren('multisel1') multisel1: wijmo.input.MultiSelect;

    @Output() select: EventEmitter<string> = new EventEmitter<string>();

    url: any;


    selectedServers: any;
    serverType: string;
    statName: string;

    @Input() docField: string;
    @Input() widgetName: string;
    @Input() widgetURL: string;
    @Input() hideDatePanel: boolean;
    
    filterData: any;
    errorMessage: any;

    get gridUrl(): string {

        return this.widgetService.getProperty('gridUrl');

    }

    set gridUrl(id: string) {

        this.widgetService.setProperty('gridUrl', id);

        this.select.emit(this.widgetService.getProperty('gridUrl'));

    }
    
    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) { }
    ngOnInit() {
        this.service.get(`/reports/server_configuration_dropdown?docfield=${this.docField}`)
            .subscribe(
            (response) => {
                this.filterData = response.data;
            },
            (error) => this.errorMessage = <any>error
            );
    }


    applyFilters(multisel1: wijmo.input.MultiSelect) {
     
        //var v = multisel1.checkedItems;
        var selectedValues = "";
        for (var item of multisel1.checkedItems) {
            if (selectedValues == "") 
                selectedValues = item.id;
            else 
                selectedValues += "," + item.id;
        }

        var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + this.docField + `=` + selectedValues;
    
        //this.widgetService.refreshWidget(this.widgetName, URL )
        //    .catch(error => console.log(error));

        this.gridUrl = URL;

    }

}