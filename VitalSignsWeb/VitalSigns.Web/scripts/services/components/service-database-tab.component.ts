import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/services/components/service-database-tab.component.html',
    directives: [ROUTER_DIRECTIVES, WidgetContainer, AppNavigator]
})
export class ServiceDatabaseTab extends WidgetController implements OnInit {

    widgets: WidgetContract[]; 

    constructor(protected resolver: ComponentResolver) {
        super(resolver);
    }

    ngOnInit() {
      


        //this.route.params.subscribe(params => {
        //    this.serviceId = params['service'];

        //});
        this.widgets = [
            {
                id: 'dynamicGrid',
                title: 'Health Assessment',
                path: '/app/widgets/grid/components/dynamic-grid.component',
                name: 'DynamicGrid',
                css: 'col-xs-12',
                settings: {
                    // url: '/DashBoard/'+this.serviceId+'/health-assessment',
                    url: '/DashBoard/57af28415c6c6c02d4fce747/database',
                    columns: [
                        { header: "Title", binding: "title", name: "title", width: "*" },
                        { header: "Status", binding: "status", name: "status", width: "*" },
                        { header: "ServerName", binding: "server_name", name: "server_name", width: "*" },
                        { header: "Test Name", binding: "test_name", name: "test_name", width: "*" },
                        { header: "Folder", binding: "folder", name: "folder", width: "*" },
                        { header: "FolderCount", binding: "folder_count", name: "folder_count", width: "*" },
                        { header: "Test Name", binding: "details", name: "details", width: "*" },
                        { header: "Result", binding: "result", name: "result", width: "*" },
                        { header: "Details", binding: "details", name: "details", width: "*" },
                        { header: "DesignTemplateName", binding: "file_name", name: "file_name", width: "*" },
                        { header: "Result", binding: "design_template_name", name: "design_template_name", width: "*" },
                        { header: "FileSize", binding: "file_size", name: "file_size", width: "*" },
                        { header: "InboxDocCount", binding: "inbox_doc_count", name: "inbox_doc_count", width: "*" },
                        { header: "ScanDateTime", binding: "scan_date", name: "scan_date", width: "*" },
                        { header: "ReplicaId", binding: "replica_idss", name: "replica_idss", width: "*" },
                        { header: "DocumentCount", binding: "document_count", name: "document_count", width: "*" },
                        { header: "Categories", binding: "categories", name: "categories", width: "*" }
                    ]

                }
            }

        ]
        injectSVG();
        bootstrapNavigator();

    }

}