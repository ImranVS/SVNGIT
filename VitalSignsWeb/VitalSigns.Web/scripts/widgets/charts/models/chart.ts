import { ChartSerie } from './chart-serie';
import { ChartSeries } from './chart-series';

export interface Chart {
    title: string,
    series: ChartSerie[],
    series2: ChartSerie[],
    drilldown: ChartSeries
}