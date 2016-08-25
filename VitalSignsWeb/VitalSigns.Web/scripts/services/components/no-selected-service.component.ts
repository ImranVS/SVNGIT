import {Component} from '@angular/core';

import {ServiceTab} from '../models/service-tab.interface';

@Component({
    template: `
<div id="noServerSelectedWrapper">
    <div id="noServerSelected">
        <img id="noServerImg" class="svgInject" src="img/menu/servers.svg" title="Servers" alt="Servers" />
        <h2>No server selected</h2>
        <p>Choose a server by clicking a server on the list</p>
        <button type="button" class="btn btn-primary">or Add server</button>
    </div>
</div>
`,
})
export class NoSelectedService implements ServiceTab {
    serviceId: string;
}