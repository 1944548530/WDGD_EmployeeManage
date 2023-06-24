// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import 'element-ui/lib/theme-chalk/index.css';
import Vuex from 'vuex'
import store from './store'
import App from './App'

import axios from 'axios'
import ElementUI from 'element-ui'
import NProgress from 'nprogress'
import * as echarts from 'echarts';
import {Message} from 'element-ui'
import "babel-polyfill"
import moment from 'moment'
import qs from 'qs'
import {getToken} from './libs/util'


import './assets/css/reset.css'// 导入全局样式
import './assets/iconFont/iconfont.css'// 导入图标
import './assets/css/index.css'
import 'nprogress/nprogress.css'
import router from './router'
import permisson from './router'
import china from './views/home/js/china.json'


Vue.config.productionTip = false
Vue.config.productionTip = false
Vue.use(Vuex)
Vue.use(permisson)
Vue.config.productionTip = false
Vue.use(ElementUI);
Vue.prototype.$moment = moment;
//Vue.use(Message);
Vue.prototype.$message = Message;
Vue.prototype.$qs = qs;
axios.defaults.baseURL = "http://localhost:56238"

//localhost:56238
//10.85.17.233/MIAOIClient
//192.168.1.195/EmployeeManage
NProgress.configure({ease:'ease-out',speed:500})
echarts.registerMap("china", china);
Vue.prototype.$echarts = echarts;
// axios.interceptors.request.use(config => {
//   //NProgress.start();
//   config.headers.Authorization = window.sessionStorage.getItem('token');
//   return config;
//   //这里拿出token，做了一次处理，然后请求服务器的时候就会使用这个处理过的请求头。
//   //原本在header中是没有Authorization这个属性的，用了上面语句后，在请求头中就有了这个值。
//   //有了这个值后，后台就可以取这个值进行安全验证了
// })

axios.interceptors.request.use(config => {
  NProgress.start();
  let token = getToken()
  //请求头Authorization一定要加Bearer！！！！！！！
  //config.headers.Authorization = "Bearer " + token;
  config.headers.Authorization = token;
  //config.headers.common['Authorization'] = token
  return config;
  //这里拿出token，做了一次处理，然后请求服务器的时候就会使用这个处理过的请求头。
  //原本在header中是没有Authorization这个属性的，用了上面语句后，在请求头中就有了这个值。
  //有了这个值后，后台就可以取这个值进行安全验证了
})

axios.interceptors.request.use(config => {
  if(config.url.indexOf('?')>-1){
    config.url=config.url+`&n=${encodeURIComponent(Math.random())}`
  }
  else{
    config.url=config.url+`?n=${encodeURIComponent(Math.random())}`
  }
  return config;
}, error => {
  return Promise.reject(error);
});

// const whiteList = ['/login']
// router.beforeEach( async (to, from, next) => {
//   NProgress.start()
//   document.title = to.meta.title;
  
//   // 获取用户token，用来判断当前用户是否登录
//   const hasToken = getToken()
//   if (hasToken) {
//       if (to.path === '/login') {
//           next()
//           NProgress.done()
//       } else {
//           //异步获取store中的路由
//           let route = store.state.router;
//           const hasRouters = route && route.length>0;
//           console.log("router:")
//           console.log(route)
//           //判断store中是否有路由，若有，进行下一步
//           if ( hasRouters ) {
//               next()
//           } 
//           // else {
//           //     //store中没有路由，则需要获取获取异步路由，并进行格式化处理
//           //     try {
//           //         console.log("getMenuList:")
//           //         initRouter()
//           //         // next({ ...to})
//           //         next('/main')
//           //     } catch (error) {
//           //         // Message.error('出错了')
//           //         Message.error(error || 'Has Error')
//           //         next(`/login?redirect=${to.path}`)
//           //         NProgress.done()
//           //     }
//           // }
//       }
//   } else {
//       if (whiteList.indexOf(to.path) !== -1) {
//           next()
//       } else {
//           next(`/login?redirect=${to.path}`)
//           NProgress.done()
//       }
//   }
// })


// //  路由判断登录 根据路由配置文件的参数
// router.beforeEach((to, from, next) => {

//   //  to,将要访问的路径
//   // from  从哪个路径跳转过来的
//   // next 是一个函数,代表放行
//     let token = getToken()
//     console.log(token)
//     if(to.path==='/login'){
//      return next()
//     }else{
//       if(!token){
//        return next('/login')
//       }else{
//         next()
//       }
//     }
//     // if(to.path === '/login'){
//     //   next()
//     //   var token = getToken()
//     // }
//     // if(!token){
//     //   next()
//     // }else{
//     //   next('/login')
//     // }
//  })
 
axios.interceptors.response.use(config => {
  NProgress.done();
  return config;
})

//将axios挂载到Vue的实例上，这样Vue中的每一个组件都可以通过this直接访问到$http从而来发起ajax请求
Vue.prototype.$http = axios;

/* eslint-disable no-new */

Vue.prototype.$store = store
/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  store,
  created() {
    //在页面加载时读取sessionStorage里的状态信息
    if (sessionStorage.getItem("store")) {
      //console.log('页面重新加载');
      let storet = sessionStorage.getItem("store");
      this.$store.replaceState(Object.assign({}, this.$store.state, JSON.parse(storet == null ? '' : storet)))
    }
    //在页面刷新时将vuex里的信息保存到sessionStorage里
    window.addEventListener("beforeunload", () => {
      //console.log('页面被刷新');
      let state = JSON.stringify(this.$store.state)
      sessionStorage.setItem("store", state == null ? '' : state)
    })
  },
  components: { App },
  template: '<App/>'
})
