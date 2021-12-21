(function ($) {
    $(document).ready(function () {
        initChart();
    })

    function initChart() {
        // A point click event that uses the Renderer to draw a label next to the point
        // On subsequent clicks, move the existing label instead of creating a new one.
        Highcharts.addEvent(Highcharts.Point, 'click', function () {
            if (this.series.options.className.indexOf('popup-on-click') !== -1) {
                const chart = this.series.chart;
                const date = Highcharts.dateFormat('%A, %b %e, %Y', this.x);
                const text = `<b>${date}</b><br/>${this.y} ${this.series.name}`;

                const anchorX = this.plotX + this.series.xAxis.pos;
                const anchorY = this.plotY + this.series.yAxis.pos;
                const align = anchorX < chart.chartWidth - 200 ? 'left' : 'right';
                const x = align === 'left' ? anchorX + 10 : anchorX - 10;
                const y = anchorY - 30;
                if (!chart.sticky) {
                    chart.sticky = chart.renderer
                        .label(text, x, y, 'callout', anchorX, anchorY)
                        .attr({
                            align,
                            fill: 'rgba(0, 0, 0, 0.75)',
                            padding: 10,
                            zIndex: 7 // Above series, below tooltip
                        })
                        .css({
                            color: 'white'
                        })
                        .on('click', function () {
                            chart.sticky = chart.sticky.destroy();
                        })
                        .add();
                } else {
                    chart.sticky
                        .attr({ align, text })
                        .animate({ anchorX, anchorY, x, y }, { duration: 250 });
                }
            }
        });


        Highcharts.chart('monthChart', {

            chart: {
                scrollablePlotArea: {
                    minWidth: 700
                }
            },
            title: {
                text: ''
            },

            subtitle: {
                text: ''
            },

            xAxis: {
                type: 'datetime',
                labels: {
                    formatter: function () {
                        var date = new Date(this.value );
                        return   date.getFullYear()+
                            "-" + (date.getMonth() + 1) +
                            "-" + date.getDate()
                    }
                }
            },

            yAxis: [{ // left y axis
                title: {
                    text: null
                },
                labels: {
                    align: 'left',
                    x: 3,
                    y: 16,
                    format: '{value:.,0f}'
                },
                showFirstLabel: false
            }, { // right y axis
                linkedTo: 0,
                gridLineWidth: 0,
                opposite: true,
                title: {
                    text: null
                },
                labels: {
                    align: 'right',
                    x: -3,
                    y: 16,
                    format: '{value:.,0f}'
                },
                showFirstLabel: false
            }],

            legend: {
                align: 'left',
                verticalAlign: 'top',
                borderWidth: 0
            },
            
            tooltip: {
                shared: true,
                crosshairs: true,
                xDateFormat: '%B, %d %Y %H:%M:%S',

            },

            plotOptions: {
                series: {
                    cursor: 'pointer',
                    className: 'popup-on-click',
                    marker: {
                        lineWidth: 1
                    }
                }
            },
            series: [{
                color: "#a37d1b",
                name:"出貨單",
                data:$Page.shipmentOrderDeliveryChartData.map(it => {
                    return new Array(it.DeliveryDate, it.Count)
                })
            }]
        });

    }
})(jQuery)