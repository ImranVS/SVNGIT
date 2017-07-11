import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { FormsModule } from '@angular/forms';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';



@Component({
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-locations.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
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
    currentPageSize: any = 20;

    constructor(service: RESTService, appComponentService: AppComponentService,
        protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService
) {
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
    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("Location", this.authService.CurrentUser.email),
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

    savelocations(dlg: wijmo.input.Popup) {
        this.saveGridRow('/Configurator/save_locations',dlg);
    }


    ngOnInit() {
        this.initialGridBind('/Configurator/locations');
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("Location", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
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
        if (confirm("Are you sure want to delete this record?")) {
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
            (<wijmo.collections.CollectionView>this.flex.collectionView).remove(this.flex.collectionView.currentItem);
        }
    }

    addlocations(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.location_name = "";
        this.currentEditItem.country = "";
        this.currentEditItem.region = "";
        this.currentEditItem.city = "";
    }
}