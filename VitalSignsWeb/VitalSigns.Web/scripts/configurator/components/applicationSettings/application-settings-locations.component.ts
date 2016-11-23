import {Component, OnInit} from '@angular/core';
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
    selectedCountry: string;
    selectedState: string;
    selectedCity: string;
    errorMessage: string;
    locationsForm: FormGroup;
    locationsId: string;
    countries: any;
    states: any;
    cities: any;
    data: wijmo.collections.CollectionView;
    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService) {
        super(service);
        this.formName = "Maintain Locations";

        this.service.get('/Configurator/get_locations')
            .subscribe(
            (response) => {
                this.countries = response.data.countryData;
            },
            (error) => this.errorMessage = <any>error
        );

      
    }


    savelocations(dlg: wijmo.input.Popup) {
        this.saveGridRow('/Configurator/save_locations',dlg);
    }


    ngOnInit() {
        this.initialGridBind('/Configurator/locations');
        }
 

    getstates(currentregion) {
      
        this.service.get('/Configurator/get_locations?country=' + currentregion)

            .subscribe(
            (response) => {
                this.states = response.data.stateData;
               // this.states.splice(0, 1);
            },
            (error) => this.errorMessage = <any>error
            );
    }
    getcities(currentitem) {
       
        this.service.get('/Configurator/get_locations?country=' + this.currentEditItem.country + '&state=' + currentitem)
            .subscribe(
            (response) => {               
                this.cities = response.data.cityData 
            },
            (error) => this.errorMessage = <any>error
            );
    }
   
    deltelocations() {

        this.delteGridRow('/Configurator/delete_location/');

    }

    addlocations(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.location_name = "";
        this.currentEditItem.country = "";
        this.currentEditItem.region = "";
        this.currentEditItem.city = "";
    }
}