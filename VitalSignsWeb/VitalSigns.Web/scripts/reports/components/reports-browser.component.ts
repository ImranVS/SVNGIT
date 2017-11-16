import { Component, ViewChild, ElementRef, ContentChild, } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router, NavigationStart, NavigationEnd, NavigationError, NavigationCancel, RoutesRecognized } from '@angular/router';

@Component({
    selector: 'reports-browser',
    templateUrl: '/app/reports/components/reports-browser.component.html',
})
export class ReportsBrowser {
    @ViewChild('exportButton') exportButton: ElementRef;
    hideButton: boolean = true
    constructor(private router: Router, private activatedRoute: ActivatedRoute) {
        //Creates a listener on the router and hides/dispalys export button
        router.events.subscribe(event => {
            //console.log(event)
            this.fileName = activatedRoute.snapshot.firstChild.url[0].path;
            if (event instanceof NavigationStart) {
                this.hideButton = true;
            }
            if (event instanceof NavigationEnd) {
                this.hideButton = this.canExport() == -1;
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
        this.hideButton = this.canExport() == -1;
    }

    //Tests to see if you can export the report. Returns a integer for the given report type, or -1 if it cannot be exported
    canExport() {
        if (<HTMLTableElement>document.querySelector('#htmlTable'))
            return 1;
        return -1;
    }

    //case statement which gets called when Export is clicked.
    export() {
        var exportNumber = this.canExport();
        switch(exportNumber) {
            case 1:
                this.exportHtmlTable();
                break;
        }
    }

    exportHtmlTable() {
        var htmlTable = <HTMLTableElement>document.querySelector('#htmlTable');
        var reportTitle = document.querySelector('.list-group-item.selected').textContent;
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
        var csvContent = "data:text/csv;charset=utf-8,";
        csvContent += tableAsCsv
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