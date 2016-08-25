import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component'
import { APP_ROUTES } from './app.routes';

import {
    OverallDashboard,
    IBMConnectionsDashboard,
    IBMDominoDashboard,
    IBMSametimeDashboard,
    IBMTravelerDashboard,
    IBMWebsphereDashboard,
    MSActiveDirectoryDashboard,
    MSExchangeDashboard,
    MSSharePointDashboard,
    Office365Dashboard,
    CloudServicesDashboard,
    FinancialDashboard,
    StatusMapDashboard,
    NoSelectedService,
    NotYetImplemented,
    ServiceDetails,
    ServicesView,
    ProfilesList,
    ProfilesForm
} from './app.routes';

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        APP_ROUTES
    ],
    declarations: [
        AppComponent,
        OverallDashboard,
        IBMConnectionsDashboard,
        IBMDominoDashboard,
        IBMSametimeDashboard,
        IBMTravelerDashboard,
        IBMWebsphereDashboard,
        MSActiveDirectoryDashboard,
        MSExchangeDashboard,
        MSSharePointDashboard,
        Office365Dashboard,
        CloudServicesDashboard,
        FinancialDashboard,
        StatusMapDashboard,
        NoSelectedService,
        NotYetImplemented,
        ServiceDetails,
        ServicesView,
        ProfilesList,
        ProfilesForm
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }