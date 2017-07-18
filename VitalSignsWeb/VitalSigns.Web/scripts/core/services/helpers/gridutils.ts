import { Injectable } from '@angular/core';

@Injectable()
export class CommonUtils {

    getGridPageName(gridName, userId) {
        return "GridPageSize_" + userId + "_" + gridName;
    }
    ExportExcel(flex, filename) {
        let pageSize = flex.itemsSource.pageSize;
        let pageIndex = flex.itemsSource.pageIndex;
        let currentItem = flex.itemsSource.currentItem;

        flex.itemsSource.pageSize = 0;

        wijmo.grid.xlsx.FlexGridXlsxConverter.save(flex, { includeColumnHeaders: true, includeCellStyles: false }, filename);

        flex.itemsSource.pageSize = pageSize;
        flex.itemsSource.pageIndex = pageIndex;
        flex.itemsSource.currentItem = currentItem;
    }


}