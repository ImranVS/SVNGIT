import { Routes, RouterModule } from '@angular/router';

import { OverallDashboard } from './dashboards/components/overall-dashboard.component';

import { IBMDominoDashboard } from './dashboards/components/ibm-domino/ibm-domino-dashboard.component';
import { IBMConnectionsDashboard } from './dashboards/components/ibm-connections/ibm-connections-dashboard.component';
import { IBMSametimeDashboard } from './dashboards/components/ibm-sametime/ibm-sametime-dashboard.component';
import { MobileUsersDashboard } from './dashboards/components/mobile-users/mobile-users-dashboard.component';
import { IBMWebsphereDashboard } from './dashboards/components/ibm-websphere/ibm-websphere-dashboard.component';
import { IBMTravelerDashboard } from './dashboards/components/ibm-traveler/ibm-traveler-dashboard.component';

import {KeyMetricsDashboard} from './dashboards/components/key-metrics/key-metrics-dashboard.component'; 
import {OverallDatabaseDashboard} from './dashboards/components/key-metrics/overall-database-dashboard.component'; 
import {HardwareStatsDashboard} from './dashboards/components/key-metrics/hardware-stats-dashboard.component'; 
import {UsersDashboard} from './dashboards/components/key-metrics/users-dashboard.component'; 

import { MSActiveDirectoryDashboard } from './dashboards/components/ms-ad/ms-ad-dashboard.component';
import { MSExchangeDashboard } from './dashboards/components/ms-exchange/ms-exchange-dashboard.component';
import { MSSharePointDashboard } from './dashboards/components/ms-sharepoint/ms-sharepoint-dashboard.component';

import { Office365Dashboard } from './dashboards/components/office365/office365-dashboard.component';
import { Office365Overall } from './dashboards/components/office365/office365-overall.component';
import { OfficeMailStatistics } from './dashboards/components/office365/office365-mail-statistics.component';
import { Office365PasswordSettings } from './dashboards/components/office365/office365-password-settings.component';

import { CloudServicesDashboard } from './dashboards/components/cloud-services-dashboard.component';

import { FinancialDashboard } from './dashboards/components/financial-dashboard.component';

import { StatusMapDashboard } from './dashboards/components/status-map-dashboard.component';

import {SiteMapList} from './navigation/editor/components/sitemap-list.component';
import {SiteMapEditor} from './navigation/editor/components/sitemap-editor.component';

import { ServicesView } from './services/components/services-view.component';
import { ServiceDetails } from './services/components/service-details.component';
import { NoSelectedService } from './services/components/no-selected-service.component';

import { ProfilesList } from './profiles/components/profiles-list.component';
import { ProfilesForm } from './profiles/components/profiles-form.component';

import { ReportsBrowser } from './reports/components/reports-browser.component';
import { NoSelectedReport } from './reports/components/no-selected-report.component';
import { SampleReport } from './reports/components/sample-report.component';
import { DiskHealthReport } from './reports/components/disk-health.component';
import { MailFileStatisticsReport } from './reports/components/mail-file-statistics.component';
import { DiskAvailabilityTrendReport } from './reports/components/disk-availability-trend.component';
import { ServerUtilizationReport } from './reports/components/server-utilization.component';
import { AnyStatisticReport } from './reports/components/servers/any-statistic-report.component';
import {AverageResponseTimeReport} from './reports/components/servers/average-response-time-report.component';
import {ResponseTimeReport} from './reports/components/servers/response-time-report.component';

import { NotYetImplemented } from './not-yet-implemented.component';
import { ApplicationSettings } from './configurator/components/applicationSettings/application-settings-tabs.component';
import { ServerSettings } from './configurator/components/serverSettings/server-settings-tabs.component';
import { Alerts } from './configurator/components/alert/alert-tabs-component';
import { IBMDomino } from './configurator/components/ibmDomino/ibmdomino-tabs-component';

export * from './dashboards/components/overall-dashboard.component';

export * from './dashboards/components/ibm-domino/ibm-domino-dashboard.component';
export * from './dashboards/components/ibm-connections/ibm-connections-dashboard.component';
export * from './dashboards/components/ibm-sametime/ibm-sametime-dashboard.component';
export * from './dashboards/components/mobile-users/mobile-users-dashboard.component';
export * from './dashboards/components/ibm-websphere/ibm-websphere-dashboard.component';
export * from './dashboards/components/ibm-traveler/ibm-traveler-dashboard.component';

export * from './dashboards/components/key-metrics/key-metrics-dashboard.component'
export * from './dashboards/components/key-metrics/overall-database-dashboard.component'
export * from './dashboards/components/key-metrics/hardware-stats-dashboard.component'; 
export * from './dashboards/components/key-metrics/users-dashboard.component'

export * from './dashboards/components/ms-ad/ms-ad-dashboard.component';
export * from './dashboards/components/ms-exchange/ms-exchange-dashboard.component';
export * from './dashboards/components/ms-sharepoint/ms-sharepoint-dashboard.component';

export * from './dashboards/components/office365/office365-dashboard.component';
export * from './dashboards/components/office365/office365-overall.component';
export * from './dashboards/components/office365/office365-mail-statistics.component';
export * from './dashboards/components/office365/office365-password-settings.component';

export * from './dashboards/components/cloud-services-dashboard.component';
export * from './dashboards/components/financial-dashboard.component';
export * from './dashboards/components/status-map-dashboard.component';

export * from './services/components/services-view.component';
export * from './services/components/service-details.component';
export * from './services/components/no-selected-service.component';

export * from './profiles/components/profiles-list.component';
export * from './profiles/components/profiles-form.component';

export * from './not-yet-implemented.component';

export * from './configurator/components/applicationSettings/application-settings-tabs.component';
export * from './configurator/components/serverSettings/server-settings-tabs.component';
export *from './configurator/components/alert/alert-tabs-component';
export * from './configurator/components/ibmDomino/ibmdomino-tabs-component';

const appRoutes: Routes = [
    {
        path: '',
        component: OverallDashboard
    },
    {
        path: 'dashboard',
        component: OverallDashboard
    },
    {
        path: 'dashboard/ibm/domino',
        component: IBMDominoDashboard
    },
    {
        path: 'dashboard/ibm/connections',
        component: IBMConnectionsDashboard
    },
    {
        path: 'dashboard/ibm/sametime',
        component: IBMSametimeDashboard
    },
    {
        path: 'dashboard/mobileusers',
        component: MobileUsersDashboard
    },
    {
        path: 'dashboard/ibm/websphere',
        component: IBMWebsphereDashboard
    },
    {
        path: 'dashboard/ibm/traveler',
        component: IBMTravelerDashboard
    },
    {
        path: 'dashboard/microsoft/active-directory',
        component: MSActiveDirectoryDashboard
    },
    {
        path: 'dashboard/microsoft/exchange',
        component: MSExchangeDashboard
    },
    {
        path: 'dashboard/microsoft/sharepoint',
        component: MSSharePointDashboard
    },
    {
        path: 'dashboard/office365',
        component: Office365Dashboard,
        children: [
            {
                path: '',
                component: Office365Overall
            },
            {
                path: 'mail-statistics',
                component: OfficeMailStatistics
            },
            {
                path: 'password-settings',
                component: Office365PasswordSettings
            }
        ]
    },
    {
        path: 'dashboard/cloud',
        component: CloudServicesDashboard
    },
    {
        path: 'dashboard/keymetrics',
        component: KeyMetricsDashboard
    },
    {
        path: 'dashboard/dominodatabases',
        component: OverallDatabaseDashboard
    },
    {
        path: 'dashboard/hardwarestats',
        component: HardwareStatsDashboard
    },
    {
        path: 'dashboard/users',
        component: UsersDashboard
    },
    {
        path: 'dashboard/financial',
        component: FinancialDashboard
    },
    //{
    //    path: 'dashboard/status-map',
    //    component: IssuesDashboard
    //},
    {
        path: 'services/:module',
        component: ServicesView,
        children: [
            {
                path: '',
                component: NoSelectedService
            },
            {
                path: ':service',
                component: ServiceDetails
            },
        ]
    },
    {
        path: 'reports',
        component: ReportsBrowser,
        children: [
            {
                path: '',
                component: NoSelectedReport
            },
            {
                path: 'sample',
                component: SampleReport
            },
            {
                path: 'diskhealth',
                component: DiskHealthReport
            },
            {
                path: 'mailfilestats',
                component: MailFileStatisticsReport
            },
            {
                path: 'diskavailabilitytrend',
                component: DiskAvailabilityTrendReport
            },
            {
                path: 'serverutilization',
                component: ServerUtilizationReport
            },
            {
                path: 'anystatistic',
                component: AnyStatisticReport
            },
            {
                path: 'averageresponsetime',
                component: AverageResponseTimeReport
            },
            {
                path: 'responsetime',
                component: ResponseTimeReport
            }
        ]
    },
    {
        path: 'profiles',
        component: ProfilesList
    },
    {
        path: 'profiles/create',
        component: ProfilesForm
    },
    {
        path: 'profiles/:email',
        component: ProfilesForm
    },
    {
        path: 'configurator/applicationsettings',
        component: ApplicationSettings
    },
    {
        path: 'configurator/serversettings',
        component: ServerSettings
    },
    {
        path: 'configurator/alert',
        component: Alerts
    },
    {
        path: 'sitemaps',
        component: SiteMapList
    },
    {
        path: 'sitemaps/:sitemap',
        component: SiteMapEditor
    },
    {
        path: 'configurator/ibmDomino',
        component: IBMDomino
    }
];

export const APP_ROUTES = RouterModule.forRoot(appRoutes);