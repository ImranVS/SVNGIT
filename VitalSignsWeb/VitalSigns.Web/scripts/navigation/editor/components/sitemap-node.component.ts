import {Component, Input, Output, EventEmitter, ViewChild, ViewContainerRef} from '@angular/core';
import {SiteMapTreeService} from '../services/sitemap-tree.service';

@Component({
    selector: 'sitemap-node',
    template: `
<ul [dragula]="'node-bag'" #container>
    <li class="siteMapNode" *ngFor="let node of nodes">
        <div [class.selected]="node.selected">
            <span class="aDragger"></span>
            <span class="aCollapse" (click)="toggleNode(node)" *ngIf="node.nodes && node.nodes.length > 0">
                <b *ngIf="!node.collapsed"> - </b>
                <b *ngIf="node.collapsed"> + </b>
            </span>
            <span class="aTitle" (click)="selectNode(node)">{{node.title}}</span>
            <span class="aToggle" (click)="toggleActive(node)">
                <b class="vsToggle" [class.checked]="!node.hasOwnProperty('active') || node.active">
                    <span> </span>
                </b>
            </span>
        </div>
        <sitemap-node [nodes]="node.nodes" (select)="selectNode($event)" [class.hideItems]="node.collapsed"></sitemap-node>
    </li>
</ul>
`,
})
export class SiteMapNode {

    @ViewChild('container', { read: ViewContainerRef }) container: ViewContainerRef;

    private _nodes: any[];

    get nodes(): any[] {
    
        return this._nodes;

    }

    @Input() set nodes(nodes: any[]) {

        this._nodes = nodes;

        this.siteMapTreeService.registerNode(this.container.element.nativeElement, this._nodes);

    }

    @Output() select: EventEmitter<any> = new EventEmitter<any>();

    constructor(private siteMapTreeService: SiteMapTreeService) { }

    ngAfterViewInit() {

        //this.siteMapTreeService.registerNode(this.container.element.nativeElement, this.nodes);

    }

    ngOnDestroy() {

        this.siteMapTreeService.removeNode(this.container.element.nativeElement);

    }

    private toggleNode(node: any) {
    
        node.collapsed = !node.collapsed;
        
    }

    private selectNode(node: any) {

        this.select.emit(node);

    }

    private toggleActive(node: any) {

        if (!node.hasOwnProperty('active'))
            node.active = true;

        node.active = !node.active;

    }

}