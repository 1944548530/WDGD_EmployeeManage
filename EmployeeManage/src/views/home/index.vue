<template>
  <div class="body">
    <header>
      <h1>数据可视化-ECharts</h1>
      <div class="showTime">{{ currentTime }}</div>
    </header>
    <section class="mainbox">
      <div class="column">
        <div class="panel bar">
          <h2>柱形图-就业行业</h2>
          <div class="chart"></div>
          <div class="panel-footer"></div>
        </div>
        <div class="panel line">
          <h2>
            折线图-人员变化 <a href="javacript:;">2020</a
            ><a href="javascript:;">2021</a>
          </h2>
          <div class="chart">图表</div>
          <div class="panel-footer"></div>
        </div>
        <div class="panel pie">
          <h2>饼形图-年龄分布</h2>
          <div class="chart">图表</div>
          <div class="panel-footer"></div>
        </div>
      </div>
      <div class="column">
        <!-- no模块制作 -->
        <div class="no">
          <div class="no-hd">
            <ul>
              <li>125811</li>
              <li>100000</li>
            </ul>
          </div>
          <div class="no-bd">
            <ul>
              <li>前端需求人数</li>
              <li>市场供应人数</li>
            </ul>
          </div>
        </div>
        <!-- 地图模块 -->
        <div class="map">
          <div class="map1"></div>
          <div class="map2"></div>
          <div class="map3"></div>
          <div class="chart"></div>
        </div>
      </div>
      <div class="column">
        <div class="panel bar2">
          <h2>柱形图-就业行业</h2>
          <div class="chart">图表</div>
          <div class="panel-footer"></div>
        </div>
        <div class="panel line2">
          <h2>折线图-播放量</h2>
          <div class="chart">图表</div>
          <div class="panel-footer"></div>
        </div>
        <div class="panel pie2">
          <h2>饼形图-地区分布</h2>
          <div class="chart">图表</div>
          <div class="panel-footer"></div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import dataJs from './js/data.js';
import dataChina from './js/dataChina.js'
//import './js/china.js';
import './js/flexible.js';
export default {
  data() {
    return {
      myColor: ["#1089E7", "#F57474", "#56D0E3", "#F8B448", "#8B78F6"],
      current: new Date() 
        
    };
  },
  computed:{
    currentTime(){
        const time =  this.current
        var y = time.getFullYear();
        var mm = time.getMonth() + 1;
        var d = time.getDate();
        var h = time.getHours(); 
        var m = time.getMinutes() < 10 ? "0" + time.getMinutes() : time.getMinutes(); 
        var s = time.getSeconds() < 10 ? "0" + time.getSeconds() : time.getSeconds(); 
        return "当前时间：" + y + "-" + mm + "-" + d + " " + h + ":" + m + ":" + s  
    }
  },
  methods: {
    
  },
  mounted() {
    setInterval(() => {
        this.current = new Date()
    })
    var myChart = this.$echarts.init(document.querySelector(".bar .chart"));
    myChart.setOption(dataJs.option1)
    // 4. 让图表跟随屏幕自动的去适应
    window.addEventListener("resize", function() {
        myChart.resize();
    });

    var myChart2 = this.$echarts.init(document.querySelector(".bar2 .chart"));
    myChart2.setOption(dataJs.option2)
    // 4. 让图表跟随屏幕自动的去适应
    window.addEventListener("resize", function() {
        myChart2.resize();
    });

    var myChart3 = this.$echarts.init(document.querySelector(".line .chart"));
    myChart3.setOption(dataJs.option3)
    // 4. 让图表跟随屏幕自动的去适应
    window.addEventListener("resize", function() {
        myChart3.resize();
    });
    // $(".line h2").on("click", "a", function() {
    //     // alert(1);
    //     // console.log($(this).index());
    //     // 点击 a 之后 根据当前a的索引号 找到对应的 yearData的相关对象
    //     // console.log(yearData[$(this).index()]);
    //     var obj = dataJs.yearData[$(this).index()];
    //     dataJs.option3.series[0].data = obj.data[0];
    //     dataJs.option3.series[1].data = obj.data[1];
    //     // 需要重新渲染
    //     myChart.setOption(dataJs.option3);
    // });

    var myChart4 = this.$echarts.init(document.querySelector(".line2 .chart"));
    myChart4.setOption(dataJs.option4)
    // 4. 让图表跟随屏幕自动的去适应
    window.addEventListener("resize", function() {
        myChart4.resize();
    });

    var myChart5 = this.$echarts.init(document.querySelector(".pie .chart"));
    myChart5.setOption(dataJs.option5)
    // 4. 让图表跟随屏幕自动的去适应
    window.addEventListener("resize", function() {
        myChart5.resize();
    });

    var myChart6 = this.$echarts.init(document.querySelector(".pie2 .chart"));
    myChart6.setOption(dataJs.option6)
    // 4. 让图表跟随屏幕自动的去适应
    window.addEventListener("resize", function() {
        myChart6.resize();
    });

    var myChart7 = this.$echarts.init(document.querySelector(".map .chart"));
    //myChart7.setOption(dataJs.option7);
    myChart7.setOption(dataChina.option);
    console.log(dataChina.option)
  // 监听浏览器缩放，图表对象调用缩放resize函数
    window.addEventListener("resize", function() {
        myChart7.resize();
    });
  },
};
</script>

<style scoped>
@import "css/index.css";
/* 您可以在这里添加CSS样式 */
.body{
    background:darkblue;
    line-height: 1.15;
}
.showTime{
    position: absolute;
    right: 0.375rem;
    top: 0;
    line-height: 0.9375rem;
    color: rgba(255, 255, 255, 0.7);
    font-size: 0.25rem;
}
</style>
