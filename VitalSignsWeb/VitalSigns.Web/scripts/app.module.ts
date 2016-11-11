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

import {SampleFiltersBar} from './reports/filters/components/sample-filters-bar.component'

import {ReportsBrowser} from './reports/components/reports-browser.component';
import {NoSelectedReport} from './reports/components/no-selected-report.component';
import {SampleReport} from './reports/components/sample-report.component';
import {DiskHealthReport} from './reports/components/disk/disk-health.component';
import {MailFileStatisticsReport} from './reports/components/mail/mail-file-statistics.component';
import {DiskAvailabilityTrendReport} from './reports/components/disk/disk-availability-trend.component';
import {ServerUtilizationReport} from './reports/components/financial/server-utilization.component';
import {AnyStatisticReport} from './reports/components/servers/any-statistic-report.component';
import {StatisticsReport} from './reports/components/statistics-report.component';
import {ResponseTimeReport} from './reports/components/servers/response-time-report.component';
import {ConsoleCommands} from './reports/components/ibm-domino/console-commands-report.component';
import {AvgCPUUtil} from './reports/components/ibm-domino/avg-cpu-util.component';
import {DailyServerTrans} from './reports/components/ibm-domino/daily-server-trans.component';
import {ClusterSecQueue} from './reports/components/ibm-domino/cluster-sec-queue.component';
import {DominoResponseTimes} from './reports/components/ibm-domino/domino-response-times.component';
import {CostPerUserChartReport} from './reports/components/financial/cost-per-user-chart.component';
import {CostPerUserGridReport} from './reports/components/financial/cost-per-user-grid.component';
import {TravelerAllocatedMemoryReport} from './reports/components/ibm-traveler/traveler-allocated-memory.component';
import { TravelerStatsReport } from './reports/components/ibm-traveler/traveler-stats.component';
import {OverallStatusReport} from './reports/components/servers/overall-status-report.component';
import {DatabaseInventoryReport} from './reports/components/ibm-domino/database-inventory-report.component';
import {LogFileReport} from './reports/components/ibm-domino/log-file-report.component';
import {MailThresholdReport} from './reports/components/ibm-domino/mail-threshold-report.component';
import {NotesDatabaseReport} from './reports/components/ibm-domino/notes-database-report.component';
import {DominoServerTasksReport} from './reports/components/ibm-domino/domino-server-tasks-report.component';
import {ServerAccessBrowserReport} from './reports/components/ibm-domino/server-access-browser-report.component';
import {ServerAvailabilityIndexReport} from './reports/components/ibm-domino/server-availability-index-report.component';
import {CommunityUsersReport} from './reports/components/ibm-connections/community-users-report.component';
import {HourlyStatisticsReport} from './reports/components/hourly-statistics-report.component';
import {ServerAvailabilityReport} from './reports/components/servers/server-availability-report.component';
import {SametimeStatisticsChartReport} from './reports/components/ibm-sametime/sametime-statistics-chart-report.component';
import {SametimeStatisticsGridReport} from './reports/components/ibm-sametime/sametime-statistics-grid-report.component';
import {DominoServerConfigurationReport} from './reports/components/configuration/domino-server-configuration-report.component';
import {ServerListTypeReport} from './reports/components/configuration/server-list-type-report.component';
import {ServerListLocationReport} from './reports/components/configuration/server-list-location-report.component';
import {ServerFilter} from './reports/filters/components/server-filter.component';

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
        StatisticsReport,
        ResponseTimeReport,
        ConsoleCommands,
        AvgCPUUtil,
        DailyServerTrans,
        ClusterSecQueue,
        DominoResponseTimes,
        CostPerUserChartReport,
        CostPerUserGridReport,
        TravelerAllocatedMemoryReport,
        SampleFiltersBar,
        OverallStatusReport,
        TravelerStatsReport,
        DatabaseInventoryReport,
        LogFileReport,
        MailThresholdReport,
        NotesDatabaseReport,
        DominoServerTasksReport,
        ServerAccessBrowserReport,
        ServerAvailabilityIndexReport,
        CommunityUsersReport,
        HourlyStatisticsReport,
        ServerAvailabilityReport,
        SametimeStatisticsChartReport,
        SametimeStatisticsGridReport,
        DominoServerConfigurationReport,
        ServerListTypeReport,
        ServerListLocationReport,
        ServerFilter,
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
        dashboards.AddLogFile,
        dashboards.Nodes,
        dashboards.IBMDomino,
        dashboards.MailDeliveryStatus,
        dashboards.ServerSettings,
        dashboards.ServerImports,
        dashboards.MobileUser,
        dashboards.Issues,

        widgets.ServerListTypeReportGrid,
        widgets.ServerListLocationReportGrid,

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
        widgets.DatabaseInventoryList,
        widgets.CostPerUserPivotGrid,
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
        widgets.DominoMailDeliveryStatus,
        widgets.ExchangeMailDeliveryStatus,
        widgets.AlertDefinitionsDashboard,
        widgets.AlertSettings,
        widgets.AlertDefinitions,
        widgets.HoursDestinations,
        widgets.Escalation,
        widgets.ServerTaskDefinition,
        widgets.NotesDatabaseReplica,
        widgets.NotesDatabases,
        widgets.CustomStatistics,
        widgets.DominoLogFiles,
        widgets.AddLogFile,
        widgets.ServerAttribute,
        widgets.MaintenanceWindows,
        widgets.DominoServerDiskSettings,
        widgets.ServerAdvancedSettings,
        widgets.AnyStatisticReportGrid,
        widgets.OverallStatusReportGrid,
        widgets.LogFileList,
        widgets.MailThresholdList,
        widgets.NotesDatabaseList,
        widgets.DominoServerTasksList,
        widgets.DominoServerImport,
        widgets.WebSphereServerImport,
        widgets.OverallDominoStat,
        widgets.CommunityUsersList,
        widgets.SametimeStatisticGridReportGrid,
        widgets.DominoServerConfigurationReportGrid,
        widgets.ServerFilter,
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
        wjFlexInput.WjComboBox,
        wjFlexInput.WjCalendar,
        wjFlexInput.WjInputDate,
        wjFlexInput.WjListBox,
        wjFlexInput.WjMultiSelect
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
        widgets.DatabaseInventoryList,
        widgets.CostPerUserPivotGrid,
        widgets.NotYetImplemented,
        widgets.OnPremisesApps,
        widgets.UserSessions,
        widgets.SampleGrid,
        widgets.StatusSummary,
        widgets.ServiceDatabaseGrid,
        widgets.BusinessHours,
        widgets.DominoServerTasks,
        widgets.OverallDominoStat,
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
        widgets.DominoMailDeliveryStatus,
        widgets.ExchangeMailDeliveryStatus,
        widgets.AlertDefinitionsDashboard,
        widgets.AlertSettings,
        widgets.AlertDefinitions,
        widgets.HoursDestinations,
        widgets.Escalation,
        widgets.ServerTaskDefinition,
        widgets.NotesDatabaseReplica,
        widgets.NotesDatabases,
        widgets.CustomStatistics,
        widgets.DominoLogFiles,
        widgets.AddLogFile,
        widgets.MaintenanceWindows,
        widgets.DominoServerDiskSettings,
        widgets.ServerAdvancedSettings,
        widgets.AnyStatisticReportGrid,
        widgets.OverallStatusReportGrid,
        widgets.LogFileList,
        widgets.MailThresholdList,
        widgets.NotesDatabaseList,
        widgets.DominoServerTasksList,
        widgets.DominoServerImport,
        widgets.WebSphereServerImport,
        widgets.CommunityUsersList,
        widgets.SametimeStatisticGridReportGrid,
        widgets.DominoServerConfigurationReportGrid,
        widgets.ServerListTypeReportGrid,
        widgets.ServerListLocationReportGrid,
        widgets.ServerFilter,
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