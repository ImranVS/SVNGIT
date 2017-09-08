import { Component, ComponentFactoryResolver, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import 'rxjs/Rx';

import { WidgetController, WidgetContainer, WidgetContract } from '../../core/widgets';
import { WidgetService } from '../../core/widgets/services/widget.service';
import { AppNavigator } from '../../navigation/app.navigator.component';

declare var injectSVG: any;


@Component({
    selector: 'ahjiaehjah',
    templateUrl: '/app/dashboards/components/executive-summary.component.html',
    providers: [WidgetService]
})
export class ExecutiveSummary extends WidgetController implements OnInit {
    
    errorMessage: string;


    widgets: WidgetContract[] = [
        {
            id: 'executiveSummaryWidget',
            title: 'Executive Summary',
            name: 'ExecutiveSummaryWidget',
            css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',
            settings: {
                deviceType : "Office365"
            }
        }
    ];
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {

        super(resolver, widgetService);

    }
    
    ngOnInit() {

        injectSVG();

    }
    
}