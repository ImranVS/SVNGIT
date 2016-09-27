import { Routes, RouterModule } from '@angular/router';

import { OverallDashboard } from './dashboards/components/overall-dashboard.component';

import { IBMDominoDashboard } from './dashboards/components/ibm-domino/ibm-domino-dashboard.component';
import { IBMConnectionsDashboard } from './dashboards/components/ibm-connections/ibm-connections-dashboard.component';
import { IBMSametimeDashboard } from './dashboards/components/ibm-sametime/ibm-sametime-dashboard.component';
import { IBMTravelerDashboard } from './dashboards/components/ibm-traveler/ibm-traveler-dashboard.component';
import { IBMWebsphereDashboard } from './dashboards/components/ibm-websphere/ibm-websphere-dashboard.component';

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

import { ServicesView } from './services/components/services-view.component';
import { ServiceDetails } from './services/components/service-details.component';
import { NoSelectedService } from './services/components/no-selected-service.component';

import { ProfilesList } from './profiles/components/profiles-list.component';
import { ProfilesForm } from './profiles/components/profiles-form.component';

import { NotYetImplemented } from './not-yet-implemented.component';

export * from './dashboards/components/overall-dashboard.component';

export * from './dashboards/components/ibm-domino/ibm-domino-dashboard.component';
export * from './dashboards/components/ibm-connections/ibm-connections-dashboard.component';
export * from './dashboards/components/ibm-sametime/ibm-sametime-dashboard.component';
export * from './dashboards/components/ibm-traveler/ibm-traveler-dashboard.component';
export * from './dashboards/components/ibm-websphere/ibm-websphere-dashboard.component';

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
        path: 'dashboard/ibm/traveler',
        component: IBMTravelerDashboard
    },
    {
        path: 'dashboard/ibm/websphere',
        component: IBMWebsphereDashboard
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
        path: 'dashboard/financial',
        component: FinancialDashboard
    },
    {
        path: 'dashboard/status-map',
        component: StatusMapDashboard
    },
    {
        path: 'services',
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
        component: NotYetImplemented
    },
    {
        path: 'configurator',
        component: NotYetImplemented
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
    }
];

export const APP_ROUTES = RouterModule.forRoot(appRoutes);