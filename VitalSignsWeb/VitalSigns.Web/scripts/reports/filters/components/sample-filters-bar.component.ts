import {Component} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';

@Component({
    selector: 'sample-filters-bar',
    templateUrl: '/app/reports/filters/components/sample-filters-bar.component.html',
})
export class SampleFiltersBar {

    startDate: Date = new Date();
    endDate: Date = new Date();

    constructor(private router: Router, private route: ActivatedRoute) { }

    applyFilters() {
    
        this.router.navigate([/[^\?]*/.exec(this.router.url)[0]], {
            queryParams: {
                start: this.startDate.toISOString(),
                end: this.endDate.toISOString()
            }
        });

    }

}