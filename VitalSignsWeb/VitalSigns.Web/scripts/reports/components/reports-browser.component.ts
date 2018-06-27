import { Component, ViewChild, ElementRef, ContentChild, ContentChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router, NavigationStart, NavigationEnd, NavigationError, NavigationCancel, RoutesRecognized } from '@angular/router';
import { ChartComponent } from '../../widgets/charts/components/chart.component';
import { BubbleChartComponent } from '../../widgets/charts/components/bubblechart.component';
import { WidgetService } from '../../core/widgets/services/widget.service';

@Component({
    selector: 'reports-browser',
    templateUrl: '/app/reports/components/reports-browser.component.html',
    providers:[WidgetService]
})
export class ReportsBrowser {
    @ViewChild('exportButton') exportButton: ElementRef;
    @ContentChildren('exportButton') temp: ElementRef;
    hideButton: boolean = true;
    childComponent: any;
    constructor(private router: Router, private activatedRoute: ActivatedRoute, private widgetService: WidgetService) {
        //Creates a listener on the router and hides/dispalys export button
        router.events.subscribe(event => {
            //console.log(event)
            if (event instanceof NavigationStart) {
                this.hideButton = true;
            }
            if (event instanceof NavigationEnd) {
                this.hideButton = this.canExport() == "None";
            }
            //if (event instanceof NavigationCancel) {
            //    console.log(3);
            //}
            //if (event instanceof NavigationError) {
            //    console.log(4);
            //}
            //if (event instanceof RoutesRecognized) {
            //    console.log(5);
            //}
        });
    }

    printReport() {

        var doc = new wijmo.PrintDocument();

        var view = <HTMLElement>document.querySelector('#reportContainer');

        for (var i = 0; i < view.children.length; i++) {

            doc.append(view.children[i]);

        }
        
        doc.print();

    }

    ngAfterViewInit() {
        this.hideButton = this.canExport() == "None";
    }

    onActivate(componentRef) {
        this.childComponent = componentRef;
    }

    //Tests to see if you can export the report. Returns a integer for the given report type, or -1 if it cannot be exported
    canExport() {
        if (<HTMLTableElement>document.querySelector('#htmlTable'))
            return "HtmlTable";
        try {
            if (this.widgetService && this.childComponent && this.widgetService && this.widgetService.findWidget) {
                var childsChild = this.widgetService.findWidget(this.childComponent.widgets[0].id).component;
                if (childsChild) {
                    if (childsChild instanceof BubbleChartComponent)
                        return "BubbleChart";
                    if (childsChild instanceof ChartComponent)
                        return "Chart";
                }
            }
        } catch (ex) { console.warn(ex) }
        return "None";
    }

    //case statement which gets called when Export is clicked.
    export() {
        var exportType = this.canExport();
        switch (exportType) {
            case "HtmlTable":
                this.exportHtmlTable();
                break;

            case "BubbleChart":
                this.exportBubbleChart();
                break;

            case "Chart":
                this.exportChart();
                break;
        }
    }

    exportHtmlTable() {
        var htmlTable = <HTMLTableElement>document.querySelector('#htmlTable');
        
        //Goes through the HTML Table and constructs the CSV File
        var tableAsCsv = ""
        for (var i = 0; i < htmlTable.tHead.rows.length; i++) {
            for (var j = 0; j < htmlTable.tHead.rows[i].children.length; j++) {
                tableAsCsv += htmlTable.tHead.rows[i].children[j].innerHTML + ',';
            }
        }
        tableAsCsv = tableAsCsv.substring(0, tableAsCsv.length - 1) + '\r\n'

        for (var k = 0; k < htmlTable.tBodies.length; k++) {
            for (var i = 0; i < htmlTable.tBodies[k].rows.length; i++) {
                for (var j = 0; j < htmlTable.tBodies[k].rows[i].children.length; j++) {
                    tableAsCsv += htmlTable.tBodies[k].rows[i].children[j].innerHTML + ',';
                }
                tableAsCsv = tableAsCsv.substring(0, tableAsCsv.length - 1) + '\r\n'
            }
        }

        //Makes a download link and downloads the CSV file
        this.downloadFile(tableAsCsv)

    }

    exportBubbleChart() {
        try {
            var series = (this.widgetService.findWidget(this.childComponent.widgets[0].id).component as BubbleChartComponent).getSeries();// this.bubbleChartReference.getSeries();
            var csvData = ""
            csvData = "Series,X,Y,Value\r\n";
            for (var k = 0; k < series.length; k++) {
                for (var i = 0; i < series[k].data.length; i++) {
                    if (series[k].name && series[k].data[i].name.x && series[k].data[i].name.y && series[k].data[i].z)
                        csvData += series[k].name + "," + series[k].data[i].name.x + "," + series[k].data[i].name.y + "," + series[k].data[i].z + "\r\n"
                }
            }
            this.downloadFile(csvData);
        } catch (ex) { console.log(ex) }

    }

    exportChart() {
        try {
            var series = (this.widgetService.findWidget(this.childComponent.widgets[0].id).component as ChartComponent).getSeries();// this.bubbleChartReference.getSeries();
            var csvData = ""
            csvData = "Series,X,Y\r\n";
            for (var k = 0; k < series.length; k++) {
                for (var i = 0; i < series[k].data.length; i++) {
                    if (series[k].name && series[k].data[i].name && series[k].data[i].y !== undefined)
                        csvData += series[k].name + "," + series[k].data[i].name + "," + series[k].data[i].y + "\r\n"
                }
            }
            this.downloadFile(csvData);
        } catch (ex) { console.log(ex) }

    }



    downloadFile(csvData) {
        var reportTitle = document.querySelector('.list-group-item.selected').textContent;
        var csvContent = "data:text/csv;charset=utf-8,";
        csvContent += csvData;
        var encodedUri = encodeURI(csvContent);
        var link = document.createElement("a");
        link.setAttribute("href", encodedUri);
        link.setAttribute("download", reportTitle.replace(/\s+/g, '') + ".csv");
        link.style.display = 'none';
        document.body.appendChild(link);

        link.click();

        document.body.removeChild(link);
    }


}