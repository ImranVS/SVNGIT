import {Component, OnInit, ViewChild, AfterViewInit, Output, EventEmitter} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HTTP_PROVIDERS}    from '@angular/http';
import {RESTService} from '../../core/services';
import {GridBase} from '../../core/gridBase';

import {AppNavigator} from '../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;


@Component({
    selector: 'server-location-list',
    templateUrl: '/app/configurator/components/server-list-location.component.html',
    directives: [
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem,
        AppNavigator
    ],
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class ServersLocation implements OnInit, AfterViewInit {
   
    @Output() checkedDevices = new EventEmitter();   
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    devices: string[]=[];
    constructor(private service: RESTService) {
        this.flex.formatItem.addHandler(function (sender, args) {

            console.log(sender);
            console.log(args);

        });
    } 
    serverCheck(value,event) {
     
        if (event.target.checked)
            this.devices.push(value);
        else {          
            this.devices.splice(this.devices.indexOf(value), 1);
        }
        this.checkedDevices.emit(this.devices);
    }

    ngOnInit() {       
        this.service.get("/Configurator/device_list")
            .subscribe(
            response => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription("location_name"));
                this.data.pageSize = 10;
            });        
    }
    ngAfterViewInit() {
      
    }
    
}



