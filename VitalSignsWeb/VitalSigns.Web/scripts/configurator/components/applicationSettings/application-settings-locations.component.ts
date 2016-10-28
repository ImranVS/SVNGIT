import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {GridBase} from '../../../core/gridBase';



@Component({
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-locations.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Location extends GridBase implements OnInit  {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    selectedCountry: string;
    selectedState: string;
    selectedCity: string;
    errorMessage: string;
    locationsForm: FormGroup;
    locationsId: string;
    countries: any;
    states: any;
    cities: any;
    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService) {
        super(service, '/Configurator/locations');
        this.formName = "Maintain Locations";

        this.service.get('/Configurator/get_locations')
            .subscribe(
            (response) => {
                this.countries = response.data.countryData;
                //delete this.deviceTypes[0];
               
              //  this.countries.splice(0, 1);



            },
            (error) => this.errorMessage = <any>error
        );

      
    }


    savelocations(dlg: wijmo.input.Popup) {
        this.saveGridRow1('/Configurator/save_locations',dlg);
    }

    ngOnInit() {


    }

    getstates() {
      
        this.service.get('/Configurator/get_locations?country=' + this.currentEditItem.country)

            .subscribe(
            (response) => {
                this.states = response.data.stateData;
                //delete this.deviceTypes[0];
               
                this.states.splice(0, 1);


            },
            (error) => this.errorMessage = <any>error
            );
    }
    getcities() {
       
        this.service.get('/Configurator/get_locations?country=' + this.currentEditItem.country+'&state='+ this.currentEditItem.region)
            .subscribe(
            (response) => {
               
                this.cities = response.data.cityData 
             //   this.cities.splice(0, 1);

            },
            (error) => this.errorMessage = <any>error
            );
    }
    //getchanged() {
    //    alert("I am changed");
    //}

    deltelocations() {

        this.delteGridRow('/Configurator/delete_location/');

    }
}