import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component'
import { APP_ROUTES } from './app.routes';

import * as dashboards from './app.routes';
import * as widgets from './app.widgets';
import * as tabs from './services/service-tab.collection';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

import {WidgetContainer} from './core/widgets';

import {AppHeader} from './navigation/app.header.component';
import {AppMainMenu} from './navigation/app.main-menu.component';
import {AppNavigator} from './navigation/app.navigator.component';

import {SearchServerList} from './services/components/search-server-list.component';
import {SearchDeviceListPipe} from './services/components/search-server-list.pipe';

import {IBMConnectionsDetails} from './dashboards/components/ibm-connections/ibm-connections-details.component';
import {IBMSametimeDetails} from './dashboards/components/ibm-sametime/ibm-sametime-details.component';

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        HttpModule,
        APP_ROUTES
    ],
    declarations: [
        AppComponent,
        WidgetContainer,
        AppHeader,
        AppMainMenu,
        AppNavigator,
        IBMConnectionsDetails,
        IBMSametimeDetails,
        SearchServerList,
        SearchDeviceListPipe,
        dashboards.OverallDashboard,
        dashboards.IBMConnectionsDashboard,
        dashboards.IBMDominoDashboard,
        dashboards.IBMSametimeDashboard,
        dashboards.IBMTravelerDashboard,
        dashboards.IBMWebsphereDashboard,
        dashboards.MSActiveDirectoryDashboard,
        dashboards.MSExchangeDashboard,
        dashboards.MSSharePointDashboard,
        dashboards.Office365Dashboard,
        dashboards.CloudServicesDashboard,
        dashboards.FinancialDashboard,
        dashboards.StatusMapDashboard,
        dashboards.NoSelectedService,
        dashboards.NotYetImplemented,
        dashboards.ServiceDetails,
        dashboards.ServicesView,
        dashboards.ProfilesList,
        dashboards.ProfilesForm,
        dashboards.Office365Overall,
        dashboards.Office365PasswordSettings,
        dashboards.OfficeMailStatistics,
        widgets.AppStatus,
        widgets.ChartComponent,
        widgets.DynamicGrid,
        widgets.GreetingsWidget,
        widgets.IBMConnectionsGrid,
        widgets.IBMDominoGrid,
        widgets.IBMSametimeGrid,
        widgets.IBMWebsphereGrid,
        widgets.KeyMetricsAlphabeticalGrid,
        widgets.KeyMetricsStatisticsGrid,
        widgets.MobileUsers,
        widgets.MobileUsersGrid,
        widgets.NotYetImplemented,
        widgets.OnPremisesApps,
        widgets.SampleGrid,
        widgets.StatusSummary,
        widgets.ServiceDatabaseGrid,
        widgets.BusinessHours,
        widgets.ServerCredentials,
        widgets.ServiceClusterHealthGrid,
        widgets.ServiceDatabaseGrid,
        widgets.ServiceMainHealthGrid,
        widgets.ServiceOutagesGrid,
        widgets.ServiceServerTasksGrid,
        widgets.ServiceTravelerHealthGrid,
        widgets.ServiceTravelerMailServersGrid,
        widgets.ServiceNMServerTasksGrid,
        tabs.DominoHealthTab,
        tabs.IBMConnectionsCommunitiesTab,
        tabs.IBMConnectionsOverviewTab,
        tabs.IBMConnectionsProfilesTab,
        tabs.NoSelectedService,
        tabs.ServiceOverallTab,
        tabs.IBMSametimeChatsTab,
        tabs.IBMSametimeConferencesTab,
        tabs.IBMSametimeMeetingsTab,
        tabs.IBMSametimeOverallTab,
        tabs.NotYetImplemented,
        tabs.ServiceMainHealthTab,
        tabs.ServiceTravelerHealthTab,
        tabs.ServiceDatabaseTab,
        tabs.ServiceTasksTab,
        tabs.ServiceClusterHealthTab,
        tabs.ServiceOutagesTab,
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem,
        wjFlexInput.WjMenuItemTemplateDir
    ],
    entryComponents: [
        widgets.AppStatus,
        widgets.ChartComponent,
        widgets.DynamicGrid,
        widgets.GreetingsWidget,
        widgets.IBMConnectionsGrid,
        widgets.IBMDominoGrid,
        widgets.IBMSametimeGrid,
        widgets.IBMWebsphereGrid,
        widgets.KeyMetricsAlphabeticalGrid,
        widgets.KeyMetricsStatisticsGrid,
        widgets.MobileUsers,
        widgets.MobileUsersGrid,
        widgets.NotYetImplemented,
        widgets.OnPremisesApps,
        widgets.SampleGrid,
        widgets.StatusSummary,
        widgets.ServiceDatabaseGrid,
        widgets.BusinessHours,
        widgets.ServerCredentials,
        widgets.ServiceClusterHealthGrid,
        widgets.ServiceDatabaseGrid,
        widgets.ServiceMainHealthGrid,
        widgets.ServiceOutagesGrid,
        widgets.ServiceServerTasksGrid,
        widgets.ServiceTravelerHealthGrid,
        widgets.ServiceTravelerMailServersGrid,
        widgets.ServiceNMServerTasksGrid,
        tabs.DominoHealthTab,
        tabs.IBMConnectionsCommunitiesTab,
        tabs.IBMConnectionsOverviewTab,
        tabs.IBMConnectionsProfilesTab,
        tabs.NoSelectedService,
        tabs.ServiceOverallTab,
        tabs.IBMSametimeChatsTab,
        tabs.IBMSametimeConferencesTab,
        tabs.IBMSametimeMeetingsTab,
        tabs.IBMSametimeOverallTab,
        tabs.NotYetImplemented,
        tabs.ServiceMainHealthTab,
        tabs.ServiceTravelerHealthTab,
        tabs.ServiceDatabaseTab,
        tabs.ServiceTasksTab,
        tabs.ServiceClusterHealthTab,
        tabs.ServiceOutagesTab
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }