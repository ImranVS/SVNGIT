import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component'
import { APP_ROUTES } from './app.routes';

import { DragulaModule } from 'ng2-dragula/ng2-dragula';

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
import {SiteMapList} from './navigation/editor/components/sitemap-list.component';
import {SiteMapEditor} from './navigation/editor/components/sitemap-editor.component';
import {SiteMapNode} from './navigation/editor/components/sitemap-node.component';

import {SearchServerList} from './services/components/search-server-list.component';
import {ServersLocation} from './configurator/components/server-list-location.component';

import {SearchDeviceListPipe} from './services/components/search-server-list.pipe';
import {FilterDeviceAttributesPipe} from './configurator/components/server/filter-device-attributes.pipe';

import {IBMConnectionsDetails} from './dashboards/components/ibm-connections/ibm-connections-details.component';
import {IBMSametimeDetails} from './dashboards/components/ibm-sametime/ibm-sametime-details.component';
import {OverallDatabaseDetails} from './dashboards/components/key-metrics/overall-database-details.component';
import {FilterByDeviceTypePipe} from './configurator/components/serverSettings/device-settings-type.pipe';

import {ReportsBrowser} from './reports/components/reports-browser.component';
import {NoSelectedReport} from './reports/components/no-selected-report.component';
import {SampleReport} from './reports/components/sample-report.component';
import {DiskHealthReport} from './reports/components/disk-health.component';
import {MailFileStatisticsReport} from './reports/components/mail-file-statistics.component';
import {DiskAvailabilityTrendReport} from './reports/components/disk-availability-trend.component';
import {ServerUtilizationReport} from './reports/components/server-utilization.component';
import {AnyStatisticReport} from './reports/components/servers/any-statistic-report.component';
import {AverageResponseTimeReport} from './reports/components/servers/average-response-time-report.component';
import {ResponseTimeReport} from './reports/components/servers/response-time-report.component';
import {ConsoleCommands} from './reports/components/consoleCommands-report.component';
import {AvgCPUUtil} from './reports/components/domino/avg-cpu-util.component';
import {DailyServerTrans} from './reports/components/domino/daily-server-trans.component';
import {ClusterSecQueue} from './reports/components/domino/cluster-sec-queue.component';
import {DominoResponseTimes} from './reports/components/domino/domino-response-times.component';

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        HttpModule,
        DragulaModule,
        APP_ROUTES
    ],
    declarations: [
        AppComponent,
        WidgetContainer,
        AppHeader,
        AppMainMenu,
        AppNavigator,
        SiteMapList,
        SiteMapEditor,
        SiteMapNode,
        IBMConnectionsDetails,
        IBMSametimeDetails,
        OverallDatabaseDetails,
        SearchServerList,
        ServersLocation,
        SearchDeviceListPipe,
        FilterByDeviceTypePipe,
        FilterDeviceAttributesPipe,
        ReportsBrowser,
        NoSelectedReport,
        SampleReport,
        DiskHealthReport,
        MailFileStatisticsReport,
        DiskAvailabilityTrendReport,
        ServerUtilizationReport,
        AnyStatisticReport,
        AverageResponseTimeReport,
        ResponseTimeReport,
        ConsoleCommands,
        AvgCPUUtil,
        DailyServerTrans,
        ClusterSecQueue,
        DominoResponseTimes,
        dashboards.OverallDashboard,
        dashboards.IBMConnectionsDashboard,
        dashboards.IBMDominoDashboard,
        dashboards.IBMSametimeDashboard,
        dashboards.MobileUsersDashboard,
        dashboards.IBMWebsphereDashboard,
        dashboards.IBMTravelerDashboard,
        dashboards.MSActiveDirectoryDashboard,
        dashboards.MSExchangeDashboard,
        dashboards.MSSharePointDashboard,
        dashboards.Office365Dashboard,
        dashboards.KeyMetricsDashboard,
        dashboards.OverallDatabaseDashboard,
        dashboards.HardwareStatsDashboard,
        dashboards.UsersDashboard,
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
        dashboards.ApplicationSettings,
        dashboards.Alerts,
        dashboards.IBMDomino,
        dashboards.ServerSettings,
        widgets.AppStatus,
        widgets.ChartComponent,
        widgets.DynamicGrid,
        widgets.GreetingsWidget,
        widgets.IBMConnectionsGrid,
        widgets.IBMConnectionsStatsGrid,
        widgets.IBMDominoGrid,
        widgets.IBMSametimeGrid,
        widgets.IBMWebsphereGrid,
        widgets.IBMWebsphereNodeGrid,
        widgets.IBMWebsphereServerGrid,
        widgets.IBMTravelerGrid,
        widgets.OverallDatabaseGrid,
        widgets.KeyMetricsStatisticsGrid,
        widgets.HardwareStatisticsGrid,
        widgets.DiskHealthGrid,
        widgets.MobileUsers,
        widgets.ConsoleCommands,
        widgets.MobileUsersGrid,
        widgets.MobileUsersKeyUserGrid,
        widgets.MailFileStatisticsList,
        widgets.NotYetImplemented,
        widgets.OnPremisesApps,
        widgets.UserSessions,
        widgets.SampleGrid,
        widgets.StatusSummary,
        widgets.ServiceDatabaseGrid,
        widgets.BusinessHours,
        widgets.DominoServerTasks,
        widgets.ServerCredentials,
        widgets.Location,
        widgets.MaintainUser,
        widgets.TravelerDataStore,
        widgets.PreferencesForm,
        widgets.IbmDominoSettingsForm,
        widgets.ServerLocations,
        widgets.ServerDiskSettings,
        widgets.DeviceAttributes,
        widgets.WindowsServices,
        widgets.Maintenance,
        widgets.ServerTasks,
        widgets.ServiceClusterHealthGrid,
        widgets.ServiceDatabaseGrid,
        widgets.ServiceMainHealthGrid,
        widgets.ServiceOutagesGrid,
        widgets.ServiceServerTasksGrid,
        widgets.ServiceTravelerHealthGrid,
        widgets.ServiceTravelerMailServersGrid,
        widgets.ServiceNMServerTasksGrid,
        widgets.ServiceEventsGrid,
        widgets.SimulationTests,
        widgets.AlertHistory,
        widgets.AlertSettings,
        widgets.ServerTaskDefinition,
        widgets.NotesDatabaseReplica,
        widgets.NotesDatabases,
        widgets.ServerAttribute,
        widgets.MaintenanceWindows,
        widgets.DominoServerDiskSettings,
        widgets.ServerAdvancedSettings,
        widgets.AnyStatisticReportGrid,
        tabs.DominoHealthTab,
        tabs.IBMConnectionsCommunitiesTab,
        tabs.IBMConnectionsOverviewTab,
        tabs.IBMConnectionsProfilesTab,
        tabs.IBMConnectionsActivitiesTab,
        tabs.IBMConnectionsBlogsTab,
        tabs.IBMConnectionsBookmarksTab,
        tabs.NoSelectedService,
        tabs.ServiceOverallTab,
        tabs.IBMSametimeChatsTab,
        tabs.IBMSametimeConferencesTab,
        tabs.IBMSametimeMeetingsTab,
        tabs.IBMSametimeOverallTab,
        tabs.IBMConnectionsBlogsTab,
        tabs.IBMConnectionsFilesTab,
        tabs.IBMConnectionsForumsTab,
        tabs.IBMConnectionsWikisTab,
        tabs.IBMConnectionsLibrariesTab,
        tabs.OverallDatabaseAllTab,
        tabs.OverallDatabaseProblemsTab,
        tabs.OverallDatabaseByTemplateTab,
        tabs.NotYetImplemented,
        tabs.ServiceMainHealthTab,
        tabs.ServiceTravelerHealthTab,
        tabs.ServiceDatabaseTab,
        tabs.ServiceTasksTab,
        tabs.ServiceClusterHealthTab,
        tabs.ServiceOutagesTab,
        tabs.ServiceEventsTab,
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem,
        wjFlexInput.WjMenuItemTemplateDir,
        wjFlexInput.WjPopup,
        wjFlexInput.WjComboBox
    ],
    entryComponents: [
        widgets.AppStatus,
        widgets.ChartComponent,
        widgets.DynamicGrid,
        widgets.GreetingsWidget,
        widgets.IBMConnectionsGrid,
        widgets.IBMConnectionsStatsGrid,
        widgets.IBMDominoGrid,
        widgets.IBMSametimeGrid,
        widgets.IBMWebsphereGrid,
        widgets.IBMWebsphereNodeGrid,
        widgets.IBMWebsphereServerGrid,
        widgets.IBMTravelerGrid,
        widgets.OverallDatabaseGrid,
        widgets.KeyMetricsStatisticsGrid,
        widgets.HardwareStatisticsGrid,
        widgets.DiskHealthGrid,
        widgets.MobileUsers,
        widgets.ConsoleCommands,
        widgets.MobileUsersGrid,
        widgets.MobileUsersKeyUserGrid,
        widgets.MailFileStatisticsList,
        widgets.NotYetImplemented,
        widgets.OnPremisesApps,
        widgets.UserSessions,
        widgets.SampleGrid,
        widgets.StatusSummary,
        widgets.ServiceDatabaseGrid,
        widgets.BusinessHours,
        widgets.DominoServerTasks,
        widgets.ServerCredentials,
        widgets.Location,
        widgets.MaintainUser,
        widgets.TravelerDataStore,
        widgets.PreferencesForm,
        widgets.IbmDominoSettingsForm,
        widgets.ServerLocations,
        widgets.ServerDiskSettings,
        widgets.DeviceAttributes,
        widgets.WindowsServices,
        widgets.Maintenance,
        widgets.ServerTasks,
        widgets.ServiceClusterHealthGrid,
        widgets.ServiceDatabaseGrid,
        widgets.ServiceMainHealthGrid,
        widgets.ServiceOutagesGrid,
        widgets.ServiceServerTasksGrid,
        widgets.ServiceTravelerHealthGrid,
        widgets.ServiceTravelerMailServersGrid,
        widgets.ServiceNMServerTasksGrid,
        widgets.ServiceEventsGrid,
        widgets.ServerAttribute,
        widgets.SimulationTests,
        widgets.AlertHistory,
        widgets.AlertSettings,
        widgets.ServerTaskDefinition,
        widgets.NotesDatabaseReplica,
        widgets.NotesDatabases,
        widgets.MaintenanceWindows,
        widgets.DominoServerDiskSettings,
        widgets.ServerAdvancedSettings,
        widgets.AnyStatisticReportGrid,
        tabs.DominoHealthTab,
        tabs.IBMConnectionsCommunitiesTab,
        tabs.IBMConnectionsOverviewTab,
        tabs.IBMConnectionsProfilesTab,
        tabs.IBMConnectionsActivitiesTab,
        tabs.IBMConnectionsBlogsTab,
        tabs.IBMConnectionsBookmarksTab,
        tabs.IBMConnectionsFilesTab,
        tabs.IBMConnectionsForumsTab,
        tabs.IBMConnectionsWikisTab,
        tabs.IBMConnectionsLibrariesTab,
        tabs.NoSelectedService,
        tabs.ServiceOverallTab,
        tabs.IBMSametimeChatsTab,
        tabs.IBMSametimeConferencesTab,
        tabs.IBMSametimeMeetingsTab,
        tabs.IBMSametimeOverallTab,
        tabs.OverallDatabaseAllTab,
        tabs.OverallDatabaseProblemsTab,
        tabs.OverallDatabaseByTemplateTab,
        tabs.NotYetImplemented,
        tabs.ServiceMainHealthTab,
        tabs.ServiceTravelerHealthTab,
        tabs.ServiceDatabaseTab,
        tabs.ServiceTasksTab,
        tabs.ServiceClusterHealthTab,
        tabs.ServiceEventsTab,
        tabs.ServiceOutagesTab
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }