/*
调用示例：
html代码：
        <div id="testchart" style="width:300px;height:300px"></div>
js代码：
        var option = {};
        option.id = "testchart";
        option.title = "主标题";
        option.subtitle = "副标题";
        option.yDescribe = "y轴描述";
        option.xAxis = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        option.yAxis = [{
            name: 'Tokyo',
            data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
        }, {
            name: 'New York',
            data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5]
        }, {
            name: 'Berlin',
            data: [-0.9, 0.6, 3.5, 8.4, 13.5, 17.0, 18.6, 17.9, 14.3, 9.0, 3.9, 1.0]
        }, {
            name: 'London',
            data: [3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8]
        }];

        linechart(option);
*/


/*
折线图
option.id                控件id
option.title             标题
option.subtitle          副标题
option.yDescribe         Y轴描述
option.xAxis             x轴值
option.yAxis             y轴值
option.unit              数值单位
*/
function linechart(option) {
    $('#' + option.id).highcharts({
        title: {
            text: option.title,
            x: -20 //center
        },
        subtitle: {
            text: option.subtitle,
            x: -20
        },
        xAxis: {
            categories: option.xAxis
        },
        yAxis: {
            title: {
                text: option.yDescribe
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: option.unit
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: option.yAxis
    });
}

/*
柱状图
option.id                控件id
option.title             标题
option.subtitle          副标题
option.yDescribe         Y轴描述
option.xAxis             x轴值
option.yAxis             y轴值
option.unit              数值单位
*/
function barchart() {
    $('#' + option.id).highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: option.title
        },
        subtitle: {
            text: option.subtitle
        },
        xAxis: {
            categories: option.xAxis
        },
        yAxis: {
            min: 0,
            title: {
                text: option.yDescribe
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span>',
            pointFormat: '' +
                '',
            footerFormat: '<table><tbody><tr><td style="color:{series.color};padding:0">{series.name}: </td><td style="padding:0"><b>{point.y:.1f} ' + option.unit + '</b></td></tr></tbody></table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: option.yAxis
    });
}