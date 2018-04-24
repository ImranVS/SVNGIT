import {WidgetController} from '../components/widget-controller';

export interface WidgetComponent {

    settings: any;

    onPropertyChanged?: (key: string, value: any) => void;

    refresh?: (settings?: any) => void;

}