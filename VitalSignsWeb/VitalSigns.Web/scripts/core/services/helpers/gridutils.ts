import { Injectable } from '@angular/core';

@Injectable()
export class CommonUtils {

    getGridPageName(gridName, userId) {
        return "GridPageSize_" + userId + "_" + gridName;
    }
}