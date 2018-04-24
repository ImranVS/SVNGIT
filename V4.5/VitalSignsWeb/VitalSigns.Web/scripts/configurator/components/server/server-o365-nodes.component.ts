import {Component, OnInit, AfterViewInit, ViewChild, Output, EventEmitter} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {GridBase} from '../../../core/gridBase';
import {RESTService} from '../../../core/services';
import {DiskSttingsValue} from '../../models/server-disk-settings';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {AppComponentService} from '../../../core/services';
import {ServersLocationService} from '../serverSettings/serverattributes-view.service';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/server/server-o365-nodes.component.html',
    providers: [
        HttpModule,
        RESTService,
        ServersLocationService
    ]
})
//export class ServerDiskSettings implements OnInit, AfterViewInit {
export class Office365Nodes implements OnInit {
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: any;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;

    constructor(
        private dataProvider: RESTService,
        private formBuilder: FormBuilder,

        private route: ActivatedRoute,
        private appComponentService: AppComponentService) {
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.dataProvider.get(`/Configurator/get_office365_nodes?deviceId=${this.deviceId}`)
            .subscribe(
            response => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 10;

            });
        
    }
    

    applySetting(nameValue: any) {

        let postData = [];
            
        for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {

            var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];

            if (item.is_selected) {


                var obj = {
                    is_selected : item.is_selected,
                    disk_name : item.disk_name,
                    freespace_threshold : item.freespace_threshold,
                    threshold_type: item.threshold_type,
                    node_id : item.node_id
                }
                // alert(item.is_selected)
                postData.push(obj);
            }
        }

    
        this.dataProvider.put(`/Configurator/save_office365_nodes?deviceId=${this.deviceId}`, postData)
            .subscribe(
            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
    }

    itemsSourceChangedHandler() {
        this.flex.autoSizeColumns();
    }

}