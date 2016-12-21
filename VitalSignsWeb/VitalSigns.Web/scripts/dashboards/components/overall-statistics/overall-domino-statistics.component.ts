import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/overall-statistics/overall-domino-statistics.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class DominoStatistics implements OnInit {
    @Input() settings: any;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    filterDate: string;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    constructor(private service: RESTService, private route: ActivatedRoute) { }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }
    ngOnInit() {
        this.service.get('/DashBoard/get_domino_statistics')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 50;
            },
            (error) => this.errorMessage = <any>error
        );
        var today = new Date();
        this.filterDate = today.toISOString().substr(0, 10);

    }

    filterStats() {
        //console.log(this.filterDate);
        var dt = new Date(this.filterDate);
        this.service.get('/DashBoard/get_domino_statistics?statdate=' + dt.toISOString().substr(0, 10))
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 50;
            },
            (error) => this.errorMessage = <any>error
            );
    }
    onItemsSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        // autosize first header row
        this.flex.autoSizeRow(0, true);

    }
}



