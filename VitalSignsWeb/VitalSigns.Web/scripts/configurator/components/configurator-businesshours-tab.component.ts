import {Component, ComponentFactoryResolver, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';


//import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/configurator/components/configurator-businesshours-tab.component.html',
    providers: [WidgetService]
})
export class BusinessHoursTab extends WidgetController implements OnInit {
    deviceId: any;
    widgets: WidgetContract[];
    @ViewChild('flex') flexInline: wijmo.grid.FlexGrid;
    editIndex: number;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.widgets = [
            {
                id: 'BusinessHoursGrid',
                title: '',
                name: 'BusinessHoursGrid',
                css: 'col-xs-12',
            }

        ]
        injectSVG();
        bootstrapNavigator();

    }


    editRow(rowIdx: number) {
        this.editIndex = rowIdx;
        this.flexInline.invalidate();
    }

    deleteRow(rowIdx: number) {
        var ecv = <wijmo.collections.CollectionView>this.flexInline.collectionView;
        ecv.removeAt(rowIdx);
    }

    commitRow(rowIdx: number) {
        // save changes
        var flex = this.flexInline;
        //flex.setCellData(rowIdx, 'Name', $("#Name").val());
        //flex.setCellData(rowIdx, 'product', $("#theProduct").val());

        // done editing
        this.cancelRow(rowIdx);
    }

    cancelRow(rowIdx: number) {
        this.editIndex = -1;
        this.flexInline.invalidate();
    }   


  

}


