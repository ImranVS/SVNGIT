import { Component, Input } from '@angular/core';
import { ComponentFactoryResolver, ViewContainerRef, ComponentRef } from '@angular/core';
import { trigger, state, style, transition, animate, AnimationTransitionEvent } from '@angular/core';

import * as helpers from '../../core/services/helpers/helpers';

@Component({
    selector: 'node-panel',
    template: `
    <div class='nodePanel' [@slidePanel]="position" (@slidePanel.done)="moveDone($event)">
      <div class='roll-up' *ngIf='parentPanel' (click)='rollUp()'>
        {{parentTitle}}
      </div>
      <ul>
        <li *ngFor='let node of nodes' (click)='drillDown(node)' [class.hasChild]='node.nodes'>
          <a [routerLink]="this.urlHelpers.getUrlPath(node.url)" [queryParams]="this.urlHelpers.getUrlQueryParams(node.url)" [class.link-active]='node.active'>{{node.title}}</a>
          <div *ngIf='node.nodes && node.nodes.length > 0' class='drill-down'></div>
        </li>
      </ul>
    </div>
  `,
    animations: [
        trigger('slidePanel', [
            state('left', style({
                transform: 'translateX(-100%)'
            })),
            state('middle', style({
                transform: 'translateX(0)'
            })),
            state('right', style({
                transform: 'translateX(100%)'
            })),
            transition('middle <=> left, middle <=> right', animate('200ms ease-in-out'))
        ]),
    ],
    providers: [
        helpers.UrlHelperService
    ]
})
export class NodePanel {

    @Input() nodes: any[];

    private position: string = 'middle';

    private childPanelRef: ComponentRef<{}>;

    private childPanel: NodePanel;
    private parentPanel: NodePanel;
    private parentTitle: string;

    constructor(
        private viewContainerRef: ViewContainerRef,
        private factoryResolver: ComponentFactoryResolver,
        protected urlHelpers: helpers.UrlHelperService
    ) { }

    rollUp() {

        if (this.parentPanel) {
            this.position = 'right';
            this.parentPanel.position = 'middle';
        }

    }

    goTo(menu: any[], path: number[]) {

        if (!(menu && path))
            return;

        this.cleanUpPath(this);

        let factory = this.factoryResolver.resolveComponentFactory(NodePanel);

        // Skip top level, already created

        let node: any = menu[path.shift()];
        let parentTitle: string = node.title;
        let nodes: any[] = node.nodes;

        let levelParentPanel: NodePanel = this;

        path.forEach(index => {

            let levelPanelRef = this.viewContainerRef.createComponent(factory);
            let levelPanel = <NodePanel>(levelPanelRef.instance);

            levelPanel.parentPanel = levelParentPanel;
            levelPanel.parentTitle = parentTitle;
            levelPanel.nodes = nodes;

            levelParentPanel.position = 'left';
            levelParentPanel.childPanel = levelPanel;
            levelParentPanel.childPanelRef = levelPanelRef;

            // Drill-down hierarchy
            let childNode = nodes[index];

            if (childNode)
                parentTitle = childNode.title;

            if (childNode && childNode.nodes) {
                levelParentPanel = levelPanel;
                nodes = childNode.nodes;
            }
            else {
                levelPanel.position = 'middle';
            }

        });

    }

    drillDown(node: any, animate: boolean = true) {

        this.cleanUpPath(this);

        if (!(node.nodes && node.nodes.length > 0))
            return;

        let factory = this.factoryResolver.resolveComponentFactory(NodePanel);

        this.childPanelRef = this.viewContainerRef.createComponent(factory);
        this.childPanel = <NodePanel>(this.childPanelRef.instance);

        this.childPanel.parentPanel = this;
        this.childPanel.parentTitle = node.title;
        this.childPanel.nodes = node.nodes;

        this.position = 'left';
        this.childPanel.position = animate ? 'right' : 'middle';

    }

    cleanUpPath(panel: NodePanel) {

        if (panel.childPanel)
            this.cleanUpPath(panel.childPanel);

        if (panel.childPanelRef)
            panel.childPanelRef.destroy();

    }

    moveDone(e: AnimationTransitionEvent) {

        if (e.fromState == 'left' && e.toState == 'middle')
            this.childPanelRef.destroy();

        if (e.fromState == 'void' && e.toState == 'right')
            this.position = 'middle';

    }

    goToLink(e, link: string) {

        e.stopPropagation();

        alert(link);

    }

}