import {WidgetController} from '../components/widget-controller';

export interface WidgetComponent {

    settings: any;
    
    refresh?: (settings?: any) => void;

}