import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, REACTIVE_FORM_DIRECTIVES, REACTIVE_FORM_PROVIDERS, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {HTTP_PROVIDERS}    from '@angular/http';
import {RESTService} from '../../core/services';
import {AppNavigator} from '../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;


@Component({
    templateUrl: '/app/configurator/components/configurator-businesshours.component.html',
    directives: [REACTIVE_FORM_DIRECTIVES,
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem,
        AppNavigator
    ],
    viewProviders: [REACTIVE_FORM_PROVIDERS],
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class BusinessHours implements OnInit, AfterViewInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    //Columns in grid
    businessHoursForm: FormGroup;
    businessHourId: string;
    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }
    constructor(private service: RESTService, private formBuilder: FormBuilder) {

        this.businessHoursForm = this.formBuilder.group({
            'id': ['', Validators.required],

            'name': ['', Validators.required],
            'start_time': ['', Validators.required],
            'duration': ['', Validators.required],
            'sunday': [false],
            'monday': [false],
            'tuesday': [false],
            'wednesday': [false],
            'thursday': [false],
            'friday': [false],
            'saturday': [false]
        });
    }
    ngOnInit() { this.bindGrid();}
    bindGrid() {
        this.service.get('/Configurator/business_hours')
            .subscribe(
            response => {
            this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
            this.data.pageSize = 10;

            },
            error => this.errorMessage = <any>error
            );        

    }
    saveBusinessHours() {


    }

    ngAfterViewInit() {
        var self = this;
        this._updateView();
       
    }
    itemsSourceChangedHandler() {
        var flex = this.flex;
        if (!flex) {
            return;
        }

        // make columns 25% wider (for readability and to show how)
        for (var i = 0; i < flex.columns.length; i++) {
            flex.columns[i].width = flex.columns[i].renderSize * 1.25;
        }
      

        // set page size on the grid's internal collectionView
        if (flex.collectionView && this.pageSize) {
            (<wijmo.collections.IPagedCollectionView>flex.collectionView).pageSize = this.pageSize;
        }
    };

   
 
    private _updateView() {

        console.log(this.flex);
        if (!this.flex.collectionView) {
            return;
        }     

        this.businessHoursForm.setValue(this.flex.collectionView.currentItem);
        console.log(this.flex.collectionView);
        
    }

    onSubmit(businessHour: any): void {    
        console.log(businessHour);

        this.service.put(
            '/Configurator/save_business_hours',
            businessHour);    
        this.bindGrid();   
        this.flex.refresh();
    }
    addBusinessHour()
    {
        this.businessHoursForm.setValue(null);
    }
    editBusinessHour() {      
        this._updateView();      
    }

    delteBusinessHour() {
        this.businessHourId = this.flex.collectionView.currentItem.id;
        if (confirm("Are you sure want to delete record")){
          this.service.delete('/Configurator/'+this.businessHourId+'/delete_business_hours'); 
        }
        this.bindGrid();
        this.flex.refresh();
    }
}



