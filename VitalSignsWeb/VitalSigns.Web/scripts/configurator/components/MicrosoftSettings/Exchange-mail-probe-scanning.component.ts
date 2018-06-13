import { Component, OnInit, ViewChild} from '@angular/core';
import { FormControl, NgModel, NgForm } from '@angular/forms';
import { RESTService } from '../../../core/services';
import { HttpModule } from '@angular/http';
import {GridBase} from '../../../core/gridBase';
import {Router} from '@angular/router';
import {ActivatedRoute} from '@angular/router';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
declare var injectSVG: any;



@Component({
    templateUrl: '/app/configurator/components/MicrosoftSettings/Exchange-mail-probe-scanning.component.html',

    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils

    ]
})
export class ExchnageMailProbe extends GridBase implements OnInit{
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('flex1') flex1: wijmo.grid.FlexGrid;

    @ViewChild('redThreshold')
    redFormControl: NgModel;

    @ViewChild('frmUser')
    userForm: NgForm;

    @ViewChild('yellowThreshold')
    yellowFormControl: NgModel;

    exchangedata: wijmo.collections.CollectionView;
    EventsName: any;
    errorMessage: string;
    currentPageSize: any = 20;
    checkedDevices: any;
    devices: string = "";

    
   
    constructor(service: RESTService, private route: ActivatedRoute,private router: Router, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, appComponentService);
        this.formName = "Exchange Mail Probe Scanning";
         
        this.service.get('/configurator/get_exchange_mail_probe ')
            .subscribe(
            response => {

                this.EventsName = response.data;

            },
            (error) => this.errorMessage = <any>error
            );

    }

    handleShownEvent(event) {
        this.redFormControl.control.valueChanges.subscribe((value) => {
            this.currentEditItem.mailprobe_red_threshold = parseInt('' + this.currentEditItem.mailprobe_red_threshold);
            if (parseInt(value) > this.currentEditItem.mailprobe_yellow_threshold) {
                this.yellowFormControl.control.setErrors(null);
            } else {
                this.yellowFormControl.control.setErrors({ 'invalid': true });
            }
        });

        this.yellowFormControl.control.valueChanges.subscribe((value) => {
            this.currentEditItem.mailprobe_red_threshold = parseInt('' + this.currentEditItem.mailprobe_red_threshold);
            if (parseInt(value) < this.currentEditItem.mailprobe_red_threshold) {
                this.redFormControl.control.setErrors(null);
            } else {
                this.redFormControl.control.setErrors({ 'invalid': true });
            }
        });
    }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("ExchnageMailProbe", this.authService.CurrentUser.email),
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
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("ExchangeMailProbe", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        this.initialGridBind('/configurator/get_exchange_mail_probe');
        injectSVG();
    }
    
  
    initialGridBind(dataURI: string) {
        this.service.get(dataURI)
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.mailprobes));
                    this.exchangedata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.exchangeservers));
                    this.data.pageSize = 10;
                } else {
                    this.appComponentService.showErrorMessage(response.message);
                }

            }, error => {
                var errorMessage = <any>error;
                this.appComponentService.showErrorMessage(errorMessage);
            });

    }
    showAddForm(dlg: wijmo.input.Popup) {

        this.addGridRow(dlg);
        this.currentEditItem.id = "";
        this.currentEditItem.name = "";
        this.currentEditItem.scan_interval = "";
        this.currentEditItem.off_hours_scan_interval = "";
        this.currentEditItem.mailprobe_red_threshold = "";
        this.currentEditItem.mailprobe_yellow_threshold = "";
         this.exchangedata.items.forEach(function (x) {
         x.is_selected = false;
        });
        
         this.exchangedata.refresh();
         this.showDialog(dlg);
    }
    saveExchangeMailProbes(dlg: wijmo.input.Popup) {

        var selectedExchangeServers: any[] = this.flex1.collectionView.items.filter(function (x) {
            return x.is_selected;
        }).map(function (x) {
            return { device_name: x.device_name, id: x.id, is_selected: x.is_selected };
            });

        if (selectedExchangeServers.length < 2) {
            this.errorMessage = "Please select at least two servers";
        }
        else {
                var postdata = {
                    exchange_mail_probe: this.currentEditItem,
                    exchange_servers: selectedExchangeServers
                }  
               this.saveGridRow('/configurator/save_exchange_probes', dlg, postdata);
                        
        }
       
    } 

    saveGridRow(saveUrl: any, dlg?: wijmo.input.Popup, postdata?: any) {

        if (this.currentEditItem.id == "") {

            this.service.put(saveUrl, postdata ? postdata : this.currentEditItem)

                .subscribe(
                response => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.mailprobes));
                    this.exchangedata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.exchangeservers));
                    if (response.status == "Success") {
                        this.currentEditItem.id = response.data;
                        (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew();

                        dlg.hide();
                        this.initialGridBind('/configurator/get_exchange_mail_probe');
                        this.appComponentService.showSuccessMessage(response.message);

                    } else {

                        this.appComponentService.showErrorMessage(response.message);
                    }

                }, error => {
                    var errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(errorMessage);
                });
        }
        else {
            this.flex.collectionView.currentItem = this.currentEditItem;

            this.service.put(saveUrl, postdata ? postdata : this.currentEditItem)
                .subscribe(
                response => {

                    if (response.status == "Success") {
                        (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit()

                        dlg.hide();
                        this.appComponentService.showSuccessMessage(response.message);
                    } else {
                        this.appComponentService.showErrorMessage(response.message);
                    }

                }, error => {
                    var errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(errorMessage);
                });

        }

    }

    deleteExchangeMailProbes() {
        this.deleteGridRow('/configurator/delete_exchange_mail_probe/');
    }
    onItemsSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        this.flex.autoSizeRow(0, true);

    }
    editGridRow(dlg: wijmo.input.Popup) {

        console.log("Inside Edit Popup");
        console.log(this.redFormControl);

            this.formTitle = "Edit " + this.formName;
            var mainThis = this;
            
                for(var i = 0; i < this.exchangedata.items.length; i++) {
                var exchangeServer = this.exchangedata.items[i];
                var exchangeMailProbeSelectedServers = mainThis.flex.collectionView.currentItem.selected_exchange_servers;

                if (exchangeMailProbeSelectedServers.indexOf(exchangeServer.id) > -1)
                    exchangeServer.is_selected = true;
                else
                    exchangeServer.is_selected = false;
            }

            (<wijmo.collections.CollectionView>this.flex.collectionView).editItem(this.flex.collectionView.currentItem);
            this.currentEditItem = this.flex.collectionView.currentItem;
            this.showDialog(dlg);
       
    }
}

