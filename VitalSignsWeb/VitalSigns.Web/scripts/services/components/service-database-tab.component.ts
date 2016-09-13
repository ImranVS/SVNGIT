import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/services/components/service-database-tab.component.html',
    directives: [WidgetContainer, AppNavigator],
    providers: [WidgetService]
})
export class ServiceDatabaseTab extends WidgetController implements OnInit {
    deviceId: any;
    widgets: WidgetContract[]; 

    constructor(protected resolver: ComponentResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
      
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.widgets = [
            {
                id: 'dynamicGrid',
                title: 'All Databases',
                path: '/app/widgets/grid/components/dynamic-grid.component',
                name: 'DynamicGrid',
                css: 'col-xs-12',
                settings: {
                    url: '/DashBoard/' + this.deviceId +'/database',
                    //url: '/DashBoard/57af28415c6c6c02d4fce747/database',
                    columns: [
                        { header: "Title", binding: "title", name: "title", width: "*" },
                        { header: "Status", binding: "status", name: "status", width: "*" },
                        { header: "DeviceName", binding: "device_name", name: "device_name", width: "*" },
                        { header: "Folder", binding: "folder", name: "folder", width: "*" },
                        { header: "FolderCount", binding: "folder_count", name: "folder_count", width: "*" },
                        { header: "Details", binding: "details", name: "details", width: "*" },
                        { header: "FileName", binding: "file_name", name: "file_name", width: "*" },
                        { header: "DesignTemplateName", binding: "design_template_name", name: "design_template_name", width: "*" },
                        { header: "FileSize", binding: "file_size", name: "file_size", width: "*" },
                        { header: "Quota", binding: "quota", name: "quota", width: "*" },
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