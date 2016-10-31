import {Component} from '@angular/core';

@Component({
    selector: 'reports-browser',
    templateUrl: '/app/reports/components/reports-browser.component.html',
})
export class ReportsBrowser {

    printReport() {

        var doc = new wijmo.PrintDocument();

        var view = <HTMLElement>document.querySelector('#reportContainer');

        for (var i = 0; i < view.children.length; i++) {

            doc.append(view.children[i]);

        }
        
        doc.print();

    }


}