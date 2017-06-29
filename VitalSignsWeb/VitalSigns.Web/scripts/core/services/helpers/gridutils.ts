import { Injectable } from '@angular/core';

@Injectable()
export class CommonUtils {

    getGridPageName(gridName, userId) {
        return "GridePageSize_" + userId + "_" + gridName;
    }
}