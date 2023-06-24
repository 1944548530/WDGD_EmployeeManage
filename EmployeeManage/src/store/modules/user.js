import Vue from 'vue'
import Vuex from 'vuex'
import screenfull from 'screenfull'
import store from 'store'
import router,{DynamicRoutes} from '../../router'
//import router from '../../router'
import axios from 'axios'

const user = {
    namespaced: true,
    state: {
        // 用户状态相关 
        userState: JSON.parse(sessionStorage.getItem('userState')) || {ut: '', isLogin: false, userType: null},
        // 用户信息相关
        // userInfo: JSON.parse(sessionStorage.getItem('userInfo')) || {},
        userInfo:{},
        // 是否获取route
        hasGetRoute: false,
        // routeMap
        // routeMap: JSON.parse(sessionStorage.getItem('routeMap')) || [],
        routeMap:[]
    },
    getters:{
        ut : state => state.userState.ut,
        isLogin: state => !!state.userState.isLogin,
        userInfo: state => state.userInfo,
        hasGetRoute: state => state.hasGetRoute,
        routeMap: state => state.routeMap[0].children,
    },
    mutations: {
        setUserState(state, playload) {
            state.userState = playload
            sessionStorage.setItem('userState', JSON.stringify(state.userState))
        },
        setUserInfo(state, playload) {
            state.userInfo = playload
            sessionStorage.setItem('userInfo', JSON.stringify(state.userInfo))
        },
        setRouteMap(state, routers) {
            state.routeMap = routers
            // 为了防止用户刷新页面导致动态创建的路由失效，将其存储在本地中
            sessionStorage.setItem('routeMap', JSON.stringify(routers));
        },
        setDynamicRouteMap(state, routers) {
            state.hasGetRoute = true
            let routerMaps = filterRouter(routers)
            // 追加路由
            // 这块是重点，如果直接使用addRoute是无效的
            let MainContainer = DynamicRoutes.find(v => v.path === "")
            let children = MainContainer.children
            
            routerMaps.forEach(item => {
                children.push(item)
            })
            //VueRouter.addRoutes(DynamicRoutes)
            // DynamicRoutes.forEach(item => {
            //     VueRouter.addRoute(item)
            // })
            console.log("DynamicRoutes")
            console.log(DynamicRoutes)
            router.addRoutes(DynamicRoutes)
        },
        resetLogin() {
            sessionStorage.clear()
        }
    },
    actions:{
        // 获取用户信息
      getUserInfo({commit,state}) {
        axios.get('Auth/profile').then(res => {
            const data = res.data.Data
            commit('setUserInfo', data)
        })
      },
      //获取用户授权动态路由
      getDynamicRouteOfUser({commit,state}) {
        axios.get('Auth/GetMenuList',{
            params: {
                employeeId: state.userInfo.employeeId
                //'this.state.userInfo.employeeId'
            }
        }).then(res => {
            var menuData = res.data.Data
            // 刷新界面菜单
            //this.$store.dispatch('setNav', menuData)
            // this.$store.commit('setRouter', menuData)
            // let dynamicRoutes = addDynamicRoutes(menuData)
            // router.addRoutes(dynamicRoutes)
            commit('setRouteMap', menuData)
            commit('setDynamicRouteMap', menuData)
            
        });
      },
      // 刷新重置路由
      updateRouteOfUser({commit}, routerMap) {
          commit('setDynamicRouteMap', routerMap)
      },
    }
}

// handle views
//  const loadView = (viewPath) => {
//     return () => import('@/views/user' + viewPath)
//   }
  const loadView = (view) => {
    //return (resolve) => require([`../../views/user/${view}`], resolve)
    return () => import('../../views/user' + view)
  }
  
  // Handle routers
  const filterRouter = (routers) => {
    return routers.filter((router) => {
       // 区分布局与视图文件，因为加载方式不同
        if (router.component === 'Main') {
            router.component = Main
        }else {
            // view
            router.component = loadView(router.path)
        }
        // 删除路由记录中的无用字段：这段是本示例与后台协商的，但在vue-router中不被支持的字段信息，可忽略
        if (!router.redirect || !router.redirect.length) { delete router.redirect }
        // 判断是否存在子路由，并递归调用自己
        if(router.children && router.children.length) {
            router.children = filterRouter(router.children)
        }
        return true
    })
  } 

  export default user;