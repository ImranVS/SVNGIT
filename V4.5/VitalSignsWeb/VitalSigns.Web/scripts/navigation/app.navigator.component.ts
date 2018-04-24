import { Component, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { trigger, state, style, transition, animate, AnimationTransitionEvent } from '@angular/core';
import { HttpModule } from '@angular/http';

import { RESTService } from '../core/services/rest.service';

import { NodePanel } from './navigator/node-panel.component';

import * as helpers from '../core/services/helpers/helpers';

@Component({
    selector: 'app-navigator',
    template: `
<div class="dropdown">
    <span (click)='toggle()' class="glyphicon glyphicon-chevron-down dropdown-toggle" data-toggle="dropdown" id="openContextNavigation" aria-hidden="true"></span>
</div>
<div class='navigator-container'>
    <div class='navigator' [class.hidden]='!visible'>
    <div class='search-box'>
        <input type='text' placeholder='search' #search (keyup)="onSearch($event)" />
        <div class='search-close' (click)='closeSearch(search)'></div>
    </div>
    <div class='panels'>
        <node-panel [nodes]='menu'></node-panel>
        <div class='search-results' [@slideSearch]="searchState">
        <ul>
            <li *ngFor='let node of searchResults'>
            <a *ngIf="node.disabled != true" [routerLink]="this.urlHelpers.getUrlPath(node.url)" [queryParams]="this.urlHelpers.getUrlQueryParams(node.url)" [innerHtml]='node.highlight'></a>
            </li>
        </ul>
        </div>
    </div>
    </div>
</div>
  `,
    animations: [
        trigger('slideSearch', [
            state('in', style({
                transform: 'translateY(0)'
            })),
            state('out', style({
                transform: 'translateY(-100%)'
            })),
            transition('in <=> out', animate('200ms ease-in-out'))
        ]),
    ],
    providers: [
        HttpModule,
        RESTService,
        helpers.UrlHelperService
    ],
    host: {
        '(document:click)': 'onClick($event)',
    }
})
export class AppNavigator {

    @ViewChild(NodePanel)
    private rootNodePanel: NodePanel;

    private searchModel;

    private visible: boolean = false;

    private searchResults: any[] = [];

    private menu: any[];
    
    private searchState: string = 'out';

    private timeoutId;

    constructor(
        private service: RESTService,
        private router: Router,
        protected urlHelpers: helpers.UrlHelperService,
        private eltRef: ElementRef
    ) { }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/navigator')
            .subscribe(
            data => {
                this.menu = data.nodes;
                this.navigateTo(this.router.url);
            },
            error => console.log(error)
            );
            
    }

    onClick(event) {

        if (!this.eltRef.nativeElement.contains(event.target))
            this.visible = false;

    }

    onSearch(e) {

        if (this.timeoutId)
            clearTimeout(this.timeoutId);

        this.timeoutId = setTimeout(() => {

            let search = e.target.value;

            if (search)
                this.doSearch(e.target.value);
            else
                this.closeSearch();

        }, 500);

    }

    toggle() {

        this.visible = !this.visible;

    }

    navigateTo(url: string) {

        let path = this.lookup(url, this.menu);
        
        if (path.length >= 2)
            this.rootNodePanel.goTo(this.menu, path);

    }

    doSearch(term) {

        this.searchState = 'in';

        let matches: any[] = [];

        if (term)
            this.search(term, this.menu, matches);

        this.searchResults = matches;

        return false;

    }

    closeSearch(searchInput?: HTMLInputElement) {

        if (searchInput)
            searchInput.value = null;

        this.searchState = 'out';

    }

    search(term: string, nodes: any[], matches: any[]) {

        if (!nodes || nodes.length == 0)
            return;

        nodes
            .filter(node => node.title.toLowerCase().indexOf(term.toLowerCase()) > -1)
            .forEach(node => {

                node.highlight = node.title.replace(new RegExp(`(${term})`, 'gi'), '<b>$1</b>');

                matches.push(node);

            });

        nodes.forEach(node => this.search(term, node.nodes, matches));

    }

    lookup(url: string, nodes: any[], path: number[] = []): number[] {

        if (!nodes || nodes.length == 0)
            return path;

        let index = nodes.findIndex(node => node.url.toLowerCase() === url.toLowerCase());

        if (index == -1) {

            for (var i = 0; i < nodes.length; i++) {

                path.push(i);

                let depth = path.length;

                if (this.lookup(url, nodes[i].nodes, path).length > depth)
                    break;
                else
                    path.pop();

            }

        }
        else {
            nodes[index].active = true;
            path.push(index);
        }

        return path;

    }

    goToLink(e, link: string) {

        e.stopPropagation();

        alert(link);

    }

}