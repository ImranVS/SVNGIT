import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {HttpModule}    from '@angular/http';
import {ActivatedRoute} from '@angular/router';
import {DragulaService} from 'ng2-dragula/ng2-dragula';

import {RESTService} from '../../../core/services/rest.service';
import {SiteMapTreeService} from '../services/sitemap-tree.service';

const FORM_HEIGHTS: any = {
    EXPANDED: '300px',
    COLLAPSED: '70px'
}

const CLIPBOARD_ACTION: any = {
    CUT: 'cut',
    COPY: 'copy'
}

@Component({
    selector: 'sitemap-editor',
    templateUrl: '/app/navigation/editor/components/sitemap-editor.component.html',
    providers: [
        HttpModule,
        RESTService,
        SiteMapTreeService
    ]
})
export class SiteMapEditor implements OnInit {

    siteMap: any;
    siteMapId: string;

    selectedNode: any;
    selectedNodeForm: FormGroup;

    clipboardNode: any;

    errorMessage: string;

    formHeight: string = FORM_HEIGHTS.COLLAPSED;
    formExpanded: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private route: ActivatedRoute,
        private dragulaService: DragulaService,
        private siteMapTreeService: SiteMapTreeService
    ) {

        this.dragulaService.setOptions('node-bag', {
            revertOnSpill: true,
            moves: function (el, container, handle) {

                return handle.className === 'aDragger';

            }
        });

        this.dragulaService.drag.subscribe((value) => {

            this.onNodeDrag(value);

        });

        this.dragulaService.drop.subscribe((value) => {

            this.onNodeDrop(value);

        });

        this.selectedNodeForm = this.formBuilder.group({
            'title': ['', Validators.required],
            'url': ['']
        });

    }

    ngOnInit() {

        this.route.params.subscribe(params => {

            this.siteMapId = params['sitemap'];

            this.dataProvider.get(`https://private-f4c5b-vitalsignssandboxserver.apiary-mock.com/navigation/editor/site-maps/${this.siteMapId}`)
                .subscribe(
                data => this.siteMap = data,
                error => this.errorMessage = <any>error
                );

        });

    }

    private domIndexOf(child: any, parent: any): any {

        return Array.prototype.indexOf.call(parent.children, child);

    }

    private dragElement: any;
    private dragIndex: number;

    private onNodeDrag(args) {

        let [name, dragElement, source] = args;

        this.dragElement = dragElement;
        this.dragIndex = this.domIndexOf(dragElement, source);

    }

    private onNodeDrop(args) {

        let [name, dropElement, target, source] = args;

        let dropIndex = this.domIndexOf(dropElement, target);
        let sourceModel = this.siteMapTreeService.getContainerModel(source);

        if (target === source) {

            sourceModel.splice(dropIndex, 0, sourceModel.splice(this.dragIndex, 1)[0]);

        } else {

            let targetModel = this.siteMapTreeService.getContainerModel(target);
            let dropElementModel = sourceModel[this.dragIndex];
            
            targetModel.splice(dropIndex, 0, dropElementModel);
            sourceModel.splice(this.dragIndex, 1);
            
        }

    }

    private selectNode(node) {
    
        this.unselectNode(this.siteMap.nodes);

        if (this.selectedNode == node) {

            this.selectedNode = undefined;

            this.selectedNodeForm.setValue({
                title: '',
                url: ''
            });

            if (this.formExpanded)
                this.collapseForm();

        }
        else {

            this.selectedNode = node;
            node.selected = true;

            this.selectedNodeForm.setValue({
                title: node.title,
                url: node.url
            });

        }

    }

    private editSelectedNode() {

        if (!this.selectedNode)
            return;

        this.expandForm();

    }

    private expandForm() {

        this.formExpanded = true;
        this.formHeight = FORM_HEIGHTS.EXPANDED;

    }

    private collapseForm() {

        this.formExpanded = false;
        this.formHeight = FORM_HEIGHTS.COLLAPSED;

    }

    private toggleForm() {

        this.formExpanded = !this.formExpanded;
        this.formHeight = this.formExpanded ? FORM_HEIGHTS.EXPANDED : FORM_HEIGHTS.COLLAPSED;

    }

    private unselectNode(node: any[]) {

        node.forEach(node => {

            node.selected = false;

            if (node.nodes)
                this.unselectNode(node.nodes);

        });

    }

    private deleteNode(node: any, nodes: any[]) {

        for (let i = nodes.length - 1; i >= 0; i--) {

            if (nodes[i] === node) {
                nodes.splice(i, 1);
            }
            else if (nodes[i].nodes) {
                this.deleteNode(node, nodes[i].nodes);
            }

        }

    }

    private deleteSelectedNode() {

        this.deleteNode(this.selectedNode, this.siteMap.nodes);

        this.selectedNode = undefined;

        this.selectedNodeForm.setValue({
            title: '',
            url: ''
        });

        if (this.formExpanded)
            this.collapseForm();

    }

    private onNodeSave(node: any): void {

        this.selectedNode.title = node.title;
        this.selectedNode.url = node.url;

        this.collapseForm();

    }
    
    private insertSibling() {

        let selecteNodeParent: any;

        let selectedNodeParentFinder = (node: any) : any => {

            if (Array.isArray(node.nodes)) {

                let found = false;

                node.nodes.forEach(childNode => {

                    if (childNode.selected) {
                    
                        found = true;
                        return;

                    }

                });

                if (found) {

                    selecteNodeParent = node;

                }
                else {

                    node.nodes.forEach(childNode => {

                        selectedNodeParentFinder(childNode);

                    });

                }

            }

        }

        selectedNodeParentFinder(this.siteMap);

        if (selecteNodeParent) {

            let newNode = {
                title: 'New item',
                url: ''
            };

            selecteNodeParent.nodes.push(newNode);
            this.selectNode(newNode);

            this.editSelectedNode();
        }

    }

    private insertChild() {

        if (!Array.isArray(this.selectedNode.nodes))
            this.selectedNode.nodes = [];

        let newNode = {
            title: 'New item',
            url: ''
        };

        this.selectedNode.nodes.push(newNode);
        this.selectNode(newNode);

        this.editSelectedNode();

    }
    
    private sortChildren() {

        if (Array.isArray(this.selectedNode.nodes))
            this.selectedNode.nodes.sort((a, b) => {

                return a.title.localeCompare(b.title);

            });

    }

    private copySelectedNode() {

        this.clipboardNode = this.selectedNode;
        this.clipboardNode.clipboard = CLIPBOARD_ACTION.COPY;
        
    }

    private cutSelectedNode() {

        this.clipboardNode = this.selectedNode;
        this.clipboardNode.clipboard = CLIPBOARD_ACTION.CUT;

    }

    private pasteNode() {

        if (!Array.isArray(this.selectedNode.nodes))
            this.selectedNode.nodes = [];

        let newNode = JSON.parse(JSON.stringify(this.clipboardNode));

        delete newNode.selected;
        delete newNode.clipboard;

        this.selectedNode.nodes.push(newNode);

        if (this.clipboardNode.clipboard == CLIPBOARD_ACTION.CUT) {

            let clipboardNodeParent;

            let clipboardNodeParentFinder = (node: any): any => {

                if (Array.isArray(node.nodes)) {

                    let found = false;

                    node.nodes.forEach(childNode => {

                        if (childNode.clipboard == CLIPBOARD_ACTION.CUT) {

                            found = true;
                            return;

                        }

                    });

                    if (found) {

                        clipboardNodeParent = node;

                    }
                    else {

                        node.nodes.forEach(childNode => {

                            clipboardNodeParentFinder(childNode);

                        });

                    }

                }

            }

            clipboardNodeParentFinder(this.siteMap);

            if (clipboardNodeParent)
                clipboardNodeParent.nodes.splice(clipboardNodeParent.nodes.indexOf(this.clipboardNode), 1);

        }
        
    }

    private saveSiteMap() {

        // TODO: To be implemented
        console.log(this.siteMap);

    }

    ngOnDestroy() {

        this.dragulaService.destroy('node-bag');

    }

}