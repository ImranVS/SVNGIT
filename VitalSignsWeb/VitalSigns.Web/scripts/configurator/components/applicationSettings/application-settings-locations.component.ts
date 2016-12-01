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
    state: any;
    city: any;
    mode: string;
    setcountry: string;
    setregion: string;
    setcity: string;
    constructor(service: RESTService) {
        super(service);
        this.formName = "Locations";

        this.service.get('/Configurator/get_locations')
            .subscribe(
            (response) => {
                this.countries = response.data.countryData;

            },
            (error) => this.errorMessage = <any>error
            );

      
    }


    savelocations(dlg: wijmo.input.Popup) {
        this.saveGridRow('/Configurator/save_locations', dlg);
      
    }


    ngOnInit() {
        this.initialGridBind('/Configurator/locations');
       
        }
    editLocationsRow(dlg: wijmo.input.Popup) {

        this.editGridRow(dlg);
        this.mode = "edit"; 
        this.setcountry = this.currentEditItem.country;
        this.setregion = this.currentEditItem.region;
        this.setcity = this.currentEditItem.city; 
    }

    getstates() {
        
        this.service.get('/Configurator/get_locations?country=' + this.currentEditItem.country)

            .subscribe(
            (response) => {             
                this.states = response.data.stateData; 
                this.currentEditItem.country = this.setcountry; 
                this.currentEditItem.region = this.setregion;
                this.currentEditItem.city = this.setcity;             
                
            },
            (error) => this.errorMessage = <any>error
        );
       
    }
    getstate() {

        this.service.get('/Configurator/get_locations?country=' + this.currentEditItem.country)

            .subscribe(
            (response) => {
                this.state = response.data.stateData;
               // this.currentEditItem.country = this.setcountry;
               this.currentEditItem.region = this.setregion;
                this.currentEditItem.city = this.setcity;

            },
            (error) => this.errorMessage = <any>error
            );

    }

  
    getcities() {
       
        this.service.get('/Configurator/get_locations?country=' + this.currentEditItem.country + '&state=' + this.currentEditItem.region)
            .subscribe(
            (response) => {   
               //  this.currentEditItem.city = this.currentEditItem.city;
                this.cities = response.data.cityData;
                this.currentEditItem.country = this.setcountry;
                this.currentEditItem.region = this.setregion;
                this.currentEditItem.city = this.setcity;  
               // this.currentEditItem.city = this.currentEditItem.city;
            },
            (error) => this.errorMessage = <any>error
        );
        
    }
    getcity() {

        this.service.get('/Configurator/get_locations?country=' + this.currentEditItem.country + '&state=' + this.currentEditItem.region)
            .subscribe(
            (response) => {
                //  this.currentEditItem.city = this.currentEditItem.city;
                this.city = response.data.cityData;
                this.currentEditItem.country = this.setcountry;
             // this.currentEditItem.region = this.setregion;
                this.currentEditItem.city = this.setcity;
                // this.currentEditItem.city = this.currentEditItem.city;
            },
            (error) => this.errorMessage = <any>error
            );

    }
   
    deltelocations() {

        this.delteGridRow('/Configurator/delete_location/');

    }

    addlocations(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.mode = "add";
        this.currentEditItem.location_name = "";
        this.currentEditItem.country = "";
        this.currentEditItem.region = "";
        this.currentEditItem.city = "";
        console.log(this.mode);
        console.log(this.currentEditItem.country);
    }
}