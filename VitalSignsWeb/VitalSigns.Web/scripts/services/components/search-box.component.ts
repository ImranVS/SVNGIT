import {Component, Output, EventEmitter}  from '@angular/core';

@Component({
    selector: 'search-box',
    template: '<div> <input #searchBox type= "text" (input) = "update.emit(searchBox.value)" /> </div>'
})
export class SearchBox {
    @Output() update = new EventEmitter();

    ngOnInit() {
        this.update.emit('');
    }
}
