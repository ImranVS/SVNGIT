import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/configurator/components/serverImport/file-upload-sample.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class FileUploadSample extends WidgetController {
    widgets: WidgetContract[];

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.widgets = [
            {
                id: 'fileUpload',
                title: 'Upload File',
                name: 'FileUpload',
                settings: {}
            }
        ];
    }
}