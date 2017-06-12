import {Injectable}     from '@angular/core';

@Injectable()
export class GridTooltip {

    getTooltip(grid, col1, col2, column_index? ) {
        //col1 is displayed in bold while col2 is normal text
        //column index is a column for which you want a custom tooltip
        return this.getGridTooltip(grid, col1, col2, column_index);
    }

    private getGridTooltip(grid, col1, col2, column_index) {
        var flex = grid;
        var tip = new wijmo.Tooltip(),
            rng = null;
        flex.hostElement.addEventListener('mousemove', function (evt) {
            var ht = flex.hitTest(evt);
            if (!ht.range.equals(rng)) {
                // new cell selected, show tooltip
                if (ht.cellType == wijmo.grid.CellType.Cell) {
                    if (typeof (column_index) === "undefined") {
                        rng = ht.range;
                        var cellElement = document.elementFromPoint(evt.clientX, evt.clientY),
                            cellBounds = wijmo.Rect.fromBoundingRect(cellElement.getBoundingClientRect()),
                            data = '<b>' + wijmo.escapeHtml(flex.getCellData(rng.row, col1, true)) + '</b>: ' + wijmo.escapeHtml(flex.getCellData(rng.row, col2, true)),
                            tipContent = data;
                        if (cellElement.className.indexOf('wj-cell') > -1) {
                            tip.show(flex.hostElement, tipContent, cellBounds);
                        } else {
                            tip.hide(); // cell must be behind scroll bar…
                        }
                    }
                    else {
                        if (ht.col === column_index) {
                            //special handling if the column index is a match
                            rng = ht.range;
                            var cellElement = document.elementFromPoint(evt.clientX, evt.clientY),
                                cellBounds = wijmo.Rect.fromBoundingRect(cellElement.getBoundingClientRect()),
                                data = '<b>' + wijmo.escapeHtml(flex.getCellData(rng.row, col1, true)) + '</b>: ' + wijmo.escapeHtml(flex.getCellData(rng.row, col2, true)),
                                tipContent = data;
                            if (cellElement.className.indexOf('wj-cell') > -1) {
                                tip.show(flex.hostElement, tipContent, cellBounds);
                            } else {
                                tip.hide(); // cell must be behind scroll bar…
                            }
                        }
                    }
                  
                }
            }
        });

        flex.hostElement.addEventListener('mouseout', function () {
            tip.hide();
            rng = null;
        });   
    }
}