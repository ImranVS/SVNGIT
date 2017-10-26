import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import { AppNavigator } from '../../../navigation/app.navigator.component';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/overall-statistics/overall-domino-statistics.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class DominoStatistics implements OnInit {
    @Input() settings: any;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    filterDate: string;
    currentPageSize: any = 20;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    loading = false;
    constructor(private service: RESTService, private route: ActivatedRoute, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();

            var obj = {
                name: this.gridHelpers.getGridPageName("DominoStatistics", this.authService.CurrentUser.email),
                value: value
            };

            this.service.put(`/services/set_name_value`, obj)
                .subscribe(
                (data) => {

                },
                (error) => console.log(error)
                );
        }
    }
    ngOnInit() {
        this.service.get('/DashBoard/get_domino_statistics')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
        );
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("DominoStatistics", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        var today = new Date();
        this.filterDate = today.toISOString().substr(0, 10);

    }
    ngAfterViewChecked() {
        injectSVG();
    }

    filterStats() {
        //console.log(this.filterDate);
        this.loading = true;
        var dt = new Date(this.filterDate);
        this.service.get('/DashBoard/get_domino_statistics?statdate=' + dt.toISOString().substr(0, 10))
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = this.currentPageSize;
                this.loading = false;
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



