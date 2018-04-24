import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {NotesDatabase} from '../models/notes-database';


@Component({
    templateUrl: './app/widgets/reports/components/notes-database-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class NotesDatabaseList implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;

    notesDatabase: any;

    constructor(private service: RESTService) { }

    loadData() {
        this.service.get('/reports/notes_database')
            .subscribe(
            data => this.notesDatabase = data.data,
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.loadData();
    }
}