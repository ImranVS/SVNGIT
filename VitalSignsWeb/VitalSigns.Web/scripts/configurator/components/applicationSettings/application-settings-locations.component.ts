import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { FormsModule } from '@angular/forms';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';



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

    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);

        this.formName = "Location";


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


       // this.delteGridRow('/Configurator/delete_location/');
        this.service.delete('/Configurator/delete_location/' + this.flex.collectionView.currentItem.id)
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.appComponentService.showSuccessMessage(response.message);
                } else {
                    this.appComponentService.showErrorMessage(response.message);
                }

            }, error => {
                var errorMessage = <any>error;
                this.appComponentService.showErrorMessage(errorMessage);
            });

    }

    addlocations(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.location_name = "";
        this.currentEditItem.country = "";
        this.currentEditItem.region = "";
        this.currentEditItem.city = "";
    }
}