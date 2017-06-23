import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppConfig } from './app.config';

import { AppComponent } from './app.component';
import { APP_ROUTES } from './app.routes';

import {SuccessErrorMessageComponent} from './core/components/success-error-message-component';
import {AlertService} from './core/services/alert.service';

import { DragulaModule } from 'ng2-dragula/ng2-dragula';

import { AuthenticationService } from './profiles/services/authentication.service';
import { AuthGuard } from './profiles/services/authgard.service';

import { LoginForm } from './profiles/components/login-form.component';

import * as dashboards from './app.routes';
import * as widgets from './app.widgets';
import * as tabs from './services/service-tab.collection';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjInputTime from 'wijmo/wijmo.angular2.input';

import { WidgetContainer } from './core/widgets';

import { ForwardPage } from './core/components/forward-page.component';

import {AppHeader} from './navigation/app.header.component';
import {AppMainMenu} from './navigation/app.main-menu.component';
import {AppNavigator} from './navigation/app.navigator.component';
import {NodePanel} from './navigation/navigator/node-panel.component';

import {SiteMapList} from './navigation/editor/components/sitemap-list.component';
import {SiteMapEditor} from './navigation/editor/components/sitemap-editor.component';
import {SiteMapNode} from './navigation/editor/components/sitemap-node.component';

import {SearchServerList} from './services/components/search-server-list.component';
import {ServersLocation} from './configurator/components/server-list-location.component';

import {SearchDeviceListPipe} from './services/components/search-server-list.pipe';
import { FilterDeviceAttributesPipe } from './configurator/components/server/filter-device-attributes.pipe';
import { FilterWidgetsPipe } from './services/components/filter-widgets.pipe';

import { IBMConnectionsDetails } from './dashboards/components/ibm-connections/ibm-connections-details.component';
import {IBMSametimeDetails} from './dashboards/components/ibm-sametime/ibm-sametime-details.component';
import {OverallDatabaseDetails} from './dashboards/components/key-metrics/overall-database-details.component';
import {FilterByDeviceTypePipe} from './configurator/components/serverSettings/device-settings-type.pipe';
import { DatabaseReplicationDetails } from './dashboards/components/key-metrics/database-replication-details.component';
import { Office365Details } from './dashboards/components/office365/office365-details.component';


import {SampleFiltersBar} from './reports/filters/components/sample-filters-bar.component'

import {ReportsBrowser} from './reports/components/reports-browser.component';
import {NoSelectedReport} from './reports/components/no-selected-report.component';
import {SampleReport} from './reports/components/sample-report.component';
import {DiskHealthReport} from './reports/components/disk/disk-health.component';
import { MailFileStatisticsReport } from './reports/components/mail/mail-file-statistics.component';
import { MailVolumeReport } from './reports/components/mail/mail-volume-report.component';
import { DiskAvailabilityTrendReport } from './reports/components/disk/disk-availability-trend.component';
import { CPUUtilizationReport } from './reports/components/disk/cpu-utilization.component';
import {ServerUtilizationReport} from './reports/components/financial/server-utilization.component';
import {AnyStatisticReport} from './reports/components/servers/any-statistic-report.component';
import {StatisticsReport} from './reports/components/statistics-report.component';
import {ResponseTimeReport} from './reports/components/servers/response-time-report.component';
import {ConsoleCommands} from './reports/components/ibm-domino/console-commands-report.component';
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
import { ServerFilter } from './reports/filters/components/server-filter.component';
import { UserFilter } from './reports/filters/components/user-filter.component';
import { TravelerFilter } from './reports/filters/components/traveler-filter.component';
import {ConnectionsServerFilter} from './reports/filters/components/connections-server-filter.component';
import {ConnectionsActivityReport} from './reports/components/ibm-connections/connections-activity-report.component';
import {ConnectionsBookmarkReport} from './reports/components/ibm-connections/connections-bookmark-report.component';
import {ConnectionsFilesReport} from './reports/components/ibm-connections/connections-files-report.component';
import {ConnectionsForumsReport} from './reports/components/ibm-connections/connections-forums-report.component';
import {ConnectionsProfilesReport} from './reports/components/ibm-connections/connections-profiles-report.component';
import {ConnectionsTagsReport} from './reports/components/ibm-connections/connections-tags-report.component';
import {ConnectionsWikiReport} from './reports/components/ibm-connections/connections-wiki-report.component';
import { ConnectionsCommunityActivityReport } from './reports/components/ibm-connections/connections-community-activity-report.component';
import { ConnectionsUserActivityReport } from './reports/components/ibm-connections/connections-user-activity-report.component';
import { ConnectionsUserActivityChartReport } from './reports/components/ibm-connections/connections-user-activity-chart-report.component';
import { ConnectionsUserActivityMonthlyChartReport } from './reports/components/ibm-connections/connections-user-activity-monthly-chart-report.component';
import { ConnectionsPopularContentReport } from './reports/components/ibm-connections/connections-popular-content-report.component';
import {ServerConfigurationFilter} from './reports/filters/components/server-configuration-filter.component';
import {FileUploadSample} from './configurator/components/serverImport/file-upload-sample.component';
import {SametimeServerFilter} from './reports/filters/components/sametime-server-filter.component';
import {AnyStatisticFilter} from './reports/filters/components/any-statistic-filter.component';
import {ServerAvailabilityFilter} from './reports/filters/components/server-availability-filter.component'
import {DiskSpaceConsumptionReport} from './reports/components/disk/disk-space-consumption.component';
import { TravelerHTTPSessionsReport } from './reports/components/ibm-traveler/traveler-http-sessions.component';
import { TravelerDeviceSyncReport } from './reports/components/ibm-traveler/traveler-device-syncs.component';
import { TravelerCPUUtilReport } from './reports/components/ibm-traveler/traveler-cpu-util.component';
import { WebSphereResponseTimes } from './reports/components/ibm-websphere/websphere-response-times.component';
import { WebSphereActiveThreads } from './reports/components/ibm-websphere/websphere-active-threads.component';
import { WebSphereCurrentHeapSize } from './reports/components/ibm-websphere/websphere-heap-size.component';
import { Office365UptimeReport } from './reports/components/office365/office365-uptime.component';
import { MobileDevicesSummaryOS } from './reports/components/mobile-users/mobile-users-os-summary.component';
import {RepeatableChart} from './reports/components/repeatable-chart.component';
import { MobileUsersReport } from './reports/components/mobile-users/mobile-users-report.component';


@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        HttpModule,
        DragulaModule,
        APP_ROUTES
    ],
    providers: [
        AppConfig,
        {
            provide: APP_INITIALIZER,
            useFactory: (config: AppConfig) => () => config.load(),
            deps: [AppConfig],
            multi: true
        },
        AuthGuard,
        AuthenticationService
    ],
    declarations: [
        AppComponent,
        ForwardPage,
        LoginForm,
        SuccessErrorMessageComponent,
        WidgetContainer,
        AppHeader,
        AppMainMenu,
        AppNavigator,
        NodePanel,
        SiteMapList,
        SiteMapEditor,
        SiteMapNode,
        IBMConnectionsDetails,
        IBMSametimeDetails,
        Office365Details,
        OverallDatabaseDetails,
        DatabaseReplicationDetails,
        SearchServerList,
        ServersLocation,
        SearchDeviceListPipe,
        FilterByDeviceTypePipe,
        FilterDeviceAttributesPipe,
        FilterWidgetsPipe,
        ReportsBrowser,
        NoSelectedReport,
        SampleReport,
        DiskHealthReport,
        MailFileStatisticsReport,
        MailVolumeReport,
        DiskAvailabilityTrendReport,
        CPUUtilizationReport,
        ServerUtilizationReport,
        AnyStatisticReport,
        StatisticsReport,
        ResponseTimeReport,
        ConsoleCommands,
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
        UserFilter,
        ConnectionsServerFilter,
        ConnectionsActivityReport,
        ConnectionsBookmarkReport,
        ConnectionsFilesReport,
        ConnectionsForumsReport,
        ConnectionsProfilesReport,
        ConnectionsTagsReport,
        ConnectionsWikiReport,
        ConnectionsCommunityActivityReport,
        ConnectionsUserActivityReport,
        ConnectionsUserActivityChartReport,
        ConnectionsUserActivityMonthlyChartReport,
        ConnectionsPopularContentReport,
        ServerConfigurationFilter,
        FileUploadSample,
        SametimeServerFilter,
        AnyStatisticFilter,
        ServerAvailabilityFilter,
        DiskSpaceConsumptionReport,
        RepeatableChart,
        TravelerHTTPSessionsReport,
        TravelerDeviceSyncReport,
        TravelerCPUUtilReport,
        WebSphereResponseTimes,
        WebSphereActiveThreads,
        WebSphereCurrentHeapSize,
        Office365UptimeReport,
        MobileDevicesSummaryOS,
        MobileUsersReport,
        dashboards.SampleDashboard,
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
        dashboards.ServerDaysUp,
        dashboards.CloudServicesDashboard,
        dashboards.FinancialDashboard,
        dashboards.StatusMapDashboard,
        dashboards.DatabaseReplicationHealth,
        dashboards.NoSelectedService,
        dashboards.NotYetImplemented,
        dashboards.ServiceDetails,
        dashboards.ServicesView,
        dashboards.ProfilesList,
        dashboards.ProfilesForm,
        dashboards.Office365Dashboard,
        dashboards.ApplicationSettings,
        dashboards.Alerts,
        dashboards.LogsTabs,
        dashboards.AddLogFile,
        dashboards.Nodes,
        dashboards.IBMDomino,
        dashboards.MailDeliveryStatus,
        dashboards.ServerSettings,
        dashboards.ServerImports,
        dashboards.MobileUser,
        dashboards.Issues,
        dashboards.DominoStatistics,
        dashboards.SametimeStatistics,
        widgets.ServerListTypeReportGrid,
        widgets.ServerListLocationReportGrid,
        widgets.AppStatus,
        widgets.ChartComponent,
        widgets.BubbleChartComponent,
        widgets.DynamicGrid,
        widgets.GreetingsWidget,
        widgets.IBMConnectionsGrid,
        widgets.IBMConnectionsStatsGrid,
        widgets.IBMConnectionsCommunitiesDetailGrid,
        widgets.IBMDominoGrid,
        widgets.IBMSametimeGrid,
        widgets.IBMWebsphereGrid,
        widgets.IBMWebsphereNodeGrid,
        widgets.IBMWebsphereServerGrid,
        widgets.IBMTravelerGrid,
        widgets.Office365Grid,
        widgets.Office365PasswordsGrid,
        widgets.DatabaseReplicationGrid,
        widgets.DatabaseProblemsGrid,
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
        widgets.Services,
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
        widgets.TestsOptions,
        widgets.AlertHistory,
        widgets.Logs,
        widgets.ViewLogs,
        widgets.DominoMailDeliveryStatus,
        widgets.ExchangeMailDeliveryStatus,
        widgets.AlertDefinitionsDashboard,
        widgets.AlertSettings,
        widgets.AlertDefinitions,
        widgets.HoursDestinations,
        widgets.Escalation,
        widgets.Scripts,
        widgets.ServerTaskDefinition,
        widgets.NotesDatabaseReplica,
        widgets.NotesDatabases,
        widgets.CustomStatistics,
        widgets.DominoLogFiles,
        widgets.AddLogFile,
        widgets.ServerAttribute,
        widgets.MaintenanceWindows,
        widgets.ServerEvents,
        widgets.DominoServerDiskSettings,
        widgets.ServerAdvancedSettings,
        widgets.WebSphereServerSettings,
        widgets.AnyStatisticReportGrid,
        widgets.OverallStatusReportGrid,
        widgets.LogFileList,
        widgets.MailThresholdList,
        widgets.NotesDatabaseList,
        widgets.DominoServerTasksList,
        widgets.DominoServerImport,
        widgets.WebSphereServerImport,
        widgets.CommunityUsersList,
        widgets.FileUpload,
        widgets.SametimeStatisticGridReportGrid,
        widgets.DominoServerConfigurationReportGrid,
        widgets.ServerFilter,
        widgets.UserFilter,
        widgets.TravelerFilter,
        widgets.ServerConfigurationFilter,
        widgets.SametimeServerFilter,
        widgets.AnyStatisticFilter,
        widgets.ServerAvailabilityFilter,
        widgets.CommunityActivityList,
        widgets.UserActivityList,
        widgets.IBMConnectionsUserComparison,
        widgets.IBMConnectionsCommunityUser,
        widgets.NotesMailProbes,
        widgets.MobileUsersReportGrid,
        tabs.DominoHealthTab,
        tabs.IBMConnectionsCommunitiesTab,
        tabs.IBMConnectionsCommunitiesDetailTab,
        tabs.IBMConnectionsOverviewTab,
        tabs.IBMConnectionsProfilesTab,
        tabs.IBMConnectionsActivitiesTab,
        tabs.IBMConnectionsBlogsTab,
        tabs.IBMConnectionsBookmarksTab,
        tabs.NoSelectedService,
        tabs.ServiceOverallTab,
        tabs.ServiceNotesDatabaseOverallTab,
        tabs.IBMSametimeChatsTab,
        tabs.IBMSametimeConferencesTab,
        tabs.IBMSametimeMeetingsTab,
        tabs.IBMSametimeOverallTab,
        tabs.IBMConnectionsBlogsTab,
        tabs.IBMConnectionsCommunitiesDetailTab,
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
        tabs.ServiceDominoMailTab,
        tabs.ServiceExchangeMailTab,
        tabs.ServiceOutagesTab,
        tabs.ServiceEventsTab,
        tabs.URLHealthTab,
        tabs.WebSphereOverallTab,
        tabs.NotesMailProbeOverallTab,
        tabs.Office365OverallTab,
        tabs.Office365UserScenarioTestsTab,
        tabs.Office365MailStatsTab,
        tabs.Office365PasswordSettingsTab,
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
        wjFlexInput.WjMultiSelect,
        wjInputTime.WjInputTime
    ],
    entryComponents: [
        NodePanel,
        widgets.AppStatus,
        widgets.ChartComponent,
        widgets.BubbleChartComponent,
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
        widgets.Office365Grid,
        widgets.Office365PasswordsGrid,
        widgets.DatabaseReplicationGrid,
        widgets.DatabaseProblemsGrid,
        widgets.OverallDatabaseGrid,
        widgets.KeyMetricsStatisticsGrid,
        widgets.HardwareStatisticsGrid,
        widgets.DiskHealthGrid,
        widgets.MobileUsers,
        widgets.ConsoleCommands,
        widgets.MobileUsersGrid,
        widgets.MobileUsersKeyUserGrid,
        widgets.MailFileStatisticsList,
        widgets.IBMConnectionsCommunitiesDetailGrid,
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
        widgets.Services,
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
        widgets.TestsOptions,
        widgets.AlertHistory,
        widgets.Logs,
        widgets.ViewLogs,
        widgets.DominoMailDeliveryStatus,
        widgets.ExchangeMailDeliveryStatus,
        widgets.AlertDefinitionsDashboard,
        widgets.AlertSettings,
        widgets.AlertDefinitions,
        widgets.HoursDestinations,
        widgets.Escalation,
        widgets.Scripts,
        widgets.ServerTaskDefinition,
        widgets.NotesDatabaseReplica,
        widgets.NotesDatabases,
        widgets.CustomStatistics,
        widgets.DominoLogFiles,
        widgets.AddLogFile,
        widgets.MaintenanceWindows,
        widgets.ServerEvents,
        widgets.DominoServerDiskSettings,
        widgets.ServerAdvancedSettings,
        widgets.WebSphereServerSettings,
        widgets.AnyStatisticReportGrid,
        widgets.OverallStatusReportGrid,
        widgets.LogFileList,
        widgets.MailThresholdList,
        widgets.NotesDatabaseList,
        widgets.DominoServerTasksList,
        widgets.DominoServerImport,
        widgets.WebSphereServerImport,
        widgets.CommunityUsersList,
        widgets.FileUpload,
        widgets.SametimeStatisticGridReportGrid,
        widgets.DominoServerConfigurationReportGrid,
        widgets.ServerListTypeReportGrid,
        widgets.ServerListLocationReportGrid,
        widgets.ServerFilter,
        widgets.UserFilter,
        widgets.TravelerFilter,
        widgets.ServerConfigurationFilter,
        widgets.SametimeServerFilter,
        widgets.AnyStatisticFilter,
        widgets.ServerAvailabilityFilter,
        widgets.CommunityActivityList,
        widgets.UserActivityList,
        widgets.IBMConnectionsUserComparison,
        widgets.IBMConnectionsCommunityUser,
        widgets.NotesMailProbes,
        widgets.MobileUsersReportGrid,
        tabs.DominoHealthTab,
        tabs.IBMConnectionsCommunitiesTab,
        tabs.IBMConnectionsCommunitiesDetailTab,
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
        tabs.ServiceNotesDatabaseOverallTab,
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
        tabs.ServiceExchangeMailTab,
        tabs.ServiceDominoMailTab,
        tabs.ServiceEventsTab,
        tabs.URLHealthTab,
        tabs.WebSphereOverallTab,
        tabs.ServiceOutagesTab,
        tabs.NotesMailProbeOverallTab,
        tabs.Office365OverallTab,
        tabs.Office365MailStatsTab,
        tabs.Office365UserScenarioTestsTab,
        tabs.Office365PasswordSettingsTab
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }