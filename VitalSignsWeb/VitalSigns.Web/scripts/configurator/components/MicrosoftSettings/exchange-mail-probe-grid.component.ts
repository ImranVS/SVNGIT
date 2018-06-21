import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    templateUrl: './app/configurator/components/MicrosoftSettings/exchange-mail-probe-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        gridHelpers.CommonUtils
    ]
})
export class ExchangeMailProbeGrid implements WidgetComponent, OnInit {
    @Input() settings: any;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    deviceId: any;
    timer: any;
    serviceId: string;
    rowHeaders: String[] = [];
    source_servers: String[] = [];
    yellowthreshold: any;
    redthreshold: any;
    constructor(private service: RESTService, private widgetService: WidgetService, private route: ActivatedRoute, protected toolTip: helpers.GridTooltip,
        protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }
    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['service'])
                this.serviceId = params['service'];
            else {

                this.serviceId = this.widgetService.getProperty('serviceId');
            }
        });
        this.loaddata();
        this.timer = window.setInterval(() => {
            console.log(this.serviceId);
            this.loaddata();

        }, 30000);
       
    }
    ngOnDestroy() {
        clearInterval(this.timer);
    }
    itemsSourceChangedHandler() {
        this.flex.autoSizeColumn(0, true, 100);
    }
    
    getSourceServerobj(serverName, data) {

        let index = data.findIndex(s => s["source_server"] === serverName);
        return data[index];
    }
    loaddata() {

        var destination_servers = [];
        var grid_data = [];
        this.service.get(`/dashboard/exchange_mail_probe?deviceId=${this.serviceId}`)
            .subscribe(
            (data) => {
                for (let rec of data.data.latency_results) {
                    console.log(data.data.latency_results);
                    let sourceValue = rec["source_server"];
                    if (this.source_servers.indexOf(sourceValue) == -1) {
                        this.source_servers.push(rec["source_server"]);
                    }
                    let keys = Object.keys(rec);
                    for (let i = 1; i < keys.length; i++) {
                        if (destination_servers.indexOf(keys[i]) == -1) {
                            destination_servers.push(keys[i]);
                        }
                    }
                }
                destination_servers.sort();
                for (let s_server of this.source_servers) {
                    let obj = {};
                    let s_server_obj = this.getSourceServerobj(s_server, data.data.latency_results);
                    for (let d_server of destination_servers) {
                        let s = s_server_obj[d_server];
                        s = (s === 0 ? s + '' : s);
                        if (s) {
                            obj[d_server] = s_server_obj[d_server];
                        } else {
                            obj[d_server] = "";
                        }
                    }
    
                    grid_data.push(obj);
                }

                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(grid_data));
                //this.serviceId = this.data.currentItem.device_id;
                this.yellowthreshold = data.data.yellow_threshold;
                this.redthreshold = data.data.red_threshold;
            },
            (error) => this.errorMessage = <any>error
            );

    }

    getRowHeaders = (p, r, c, cell) => {
        if (p.cellType == wijmo.grid.CellType.RowHeader) {
            cell.textContent = this.source_servers[r];
        }
        else if (p.cellType !== wijmo.grid.CellType.ColumnHeader) {
            if (cell.textContent.trim().length > 0) {
                const value = parseInt(cell.textContent);
                //cell.style.fontclour = 'blue';
                if (value >= 0 && value <= this.yellowthreshold) {
                    cell.style.color = 'white';
                    cell.style.backgroundColor = '#5CB85C';
                }
                else if (value > this.yellowthreshold && value <= this.redthreshold) {
                    cell.style.color = 'black';
                    cell.style.backgroundColor = 'yellow';
                }
                else  {
                    cell.style.color = 'white';
                    cell.style.backgroundColor = 'red';
                }
                 
            } else {
                cell.style.backgroundColor = 'gray';
            }

        }

    }
}
 