import Vue from 'vue'
import Vuex from 'vuex'
import screenfull from 'screenfull'
import store from 'store'
import user from './modules/user'

import{
  setToken
} from '@/libs/util'
// import user from './module/user'
// import app from './module/app'

Vue.use(Vuex)

/**
 * @description 页面是否缓存
 * @param {Object} page
 */
 const isCache = page => {
  return !page.meta || page.meta.cache !== false
}

export default new Vuex.Store({
  namespaced: true,
  modules: {
    user
  },
  state: {
    router:[],
    cacheKey: 'tabnav',
    collapse: false,
    active: true,
    keepAlive: ['home'],
    userName: '',
    userType:'',
    employeeId: '',
    opened: [
      {
        path: '/home',
        tabName: '首页',
        icon: 'el-icon-s-home'
      }
    ],
    current: {
      path: '/home',
      tabName: '首页',
      icon: 'el-icon-s-home'
    },
    // 默认页
    default: '',
    /**组件配置 */
    component:{
      //标签页
      tabnav: {
        //是否启用
        enabled: true,
        //是否显示首页
        showHome: true,
        /** 首页 */
        homeUrl: '',
        //是否显示图标
        showIcon: true,
        //最大页面数量
        maxOpenCount: 20
      }
    }
  },
  getters:{
    keepAlive: state => state.keepAlive,
  },
  mutations: {
    resetLogin(state) {
        sessionStorage.clear()
        state.opened = [
          {
            path: '/home',
            tabName: '首页',
            icon: 'el-icon-s-home'
          }
        ]
        state.keepAlive = ['home']
        state.current = {
          path: '/home',
          tabName: '首页',
          icon: 'el-icon-s-home'
        }
    },
    setRouter(state, router){
      state.router = router
    },
    setToken(state, token) {
      state.token = token
      setToken(token)
    },
    setUserName(state, name) {
      state.userName = name
    },
    setUserType(state, userType) {
      state.userType = userType
    },
    setEmployeeId(state, employeeId) {
      state.employeeId = employeeId
    },
    collapseSet(state, collapse) {
      state.collapse = collapse
    },
    screenSet(state, active){
      state.active = active
    },
    setKeepAlive:(state, keepAlive) => {
      state.keepAlive = keepAlive
    },
    /**
     * @description 删除一个页面的缓存设置
     * @param {Object} state vuex state
     * @param {String} name name
     */
    keepAliveRemove(state, name) {
      const list = [...state.keepAlive]
      //console.log(list)
      const index = list.findIndex(item => item === name)
      //console.log(name)
      if (index > -1) {
        list.splice(index, 1)
        state.keepAlive = list
      }
    },
    keepAliveAdd(state, name){
      //console.log(name)
      const list = [...state.keepAlive]
      const index = list.findIndex(item => item === name)
      if(index < 0){
        state.keepAlive.push(name)
      }
      //console.log(state.keepAlive)
    },
    /**
     * @description 清空已打开页面
     * @param {*} state
     */
     clearOpened(state) {
      state.opened = []
    },
    /**
     * @description 清空页面缓存设置
     * @param {Object} state vuex state
     */
     keepAliveClean(state) {
      state.keepAlive = []
    },
    /**
     * @description 添加一个页面
     */
     add(state, page) {
      state.opened.push(page)
    },
    /**
     * @description 删除一个页面
     * @param {Number} index 下标
     */
    remove(state, index) {
      state.opened.splice(index, 1)
    },
    /**
     * @description 页面已打开，切换页面
     * @param {Number} index 下标
     */
     changeTab (state, index) {
      state.current = index
    }
    
  },
  actions: {
     
    setNav({ router, commit }){
      commit('setRouter', router)
    },
    /**
       * @description 切换折叠状态
       */
     toggle({ state, commit }) {
      commit('collapseSet', !state.collapse)
    },
    /**
     * @description 隐藏
     */
    hide({ commit }) {
      commit('collapseSet', false)
    },
    /**
     * @description 显示
     */
    show({ commit }) {
      commit('collapseSet', true)
    },
    screenToggle({ commit }) {
        return new Promise(resolve => {
        if (screenfull.isFullscreen) {
            screenfull.exit()
            commit('screenSet', true)
        } else {
            screenfull.request()
            commit('screenSet', false)
        }
        // end
        resolve()
        })
    },
    /**
     * @description 关闭标签
     * @param {String} path 页面的路径
     * @param {Object} router 路由对象
     */
     async close({ state, commit, dispatch }, { index, router, to }) {
      let newPage = to
      const page = state.opened[index]
      if (index > -1) { 
        if (!to) {
          // 如果关闭的页面就是当前显示的页面
          if (state.current.path === page.path) {
            // 打开一个新的页面
            if (index > 0) {
              newPage = state.opened[index - 1]
            } else {
              newPage = state.opened[index + 1]
            }
            state.current = newPage
          }
        }
        commit('remove', index)
        await dispatch('cacheInsert')

        if (isCache(page) === true) {
          commit('keepAliveRemove', page.name)
        }
      }
      if (newPage) router.push(newPage)
    },
    closeLeft({ state, commit, dispatch }, { index, router }) {
      if (index > 0) {
        const page = state.opened[index]
        state.opened.splice(0, index).forEach(item => {
          if (isCache(item) === true) commit('keepAliveRemove', item.name)

          //如果关闭的页面包括当前打开的页面，则跳转到选择的页
          if (item === state.current) {
            router.push(page)
          }
        })

        dispatch('cacheInsert')
      }
    },
    closeRight({ state, commit, dispatch }, { index, router }) {
      if (index < state.opened.length - 1) {
        const page = state.opened[index]
        state.opened.splice(index + 1).forEach(item => {
          if (isCache(item) === true) commit('keepAliveRemove', item.name)

          //如果关闭的页面包括当前打开的页面，则跳转到选择的页npm run
          if (item.path === state.current.path) {
            router.push(page)
          }
        })
        dispatch('cacheInsert')
      }
    },
    /**
     * @description 关闭其它标签
     * @param {String} path 选择的页面路径
     * @param {Object} router 路由对象
     */
     closeOther({ state, commit, dispatch }, { index, router }) {
      if (index > -1) {
        const page = state.opened[index]
        // 删除右侧
        state.opened.splice(index + 1).forEach(item => {
          if (isCache(item) === true) commit('keepAliveRemove', item.name)

          //如果关闭的页面包括当前打开的页面，则跳转到选择的页
          if (item.path === state.current.path) {
            router.push(page)
          }
        })
        // 删除左侧
        state.opened.splice(0, index).forEach(item => {
          if (isCache(item) === true) commit('keepAliveRemove', item.name)

          //如果关闭的页面包括当前打开的页面，则跳转到选择的页
          if (item.path === state.current.path) {
            router.push(page)
          }
        })
        dispatch('cacheInsert')
      } else {
        dispatch('closeAll')
      }
    },
    /**
     * @description 关闭所有标签
     * @param {String} path 选择的页面路径
     * @param {Object} router 路由对象
     */
     closeAll({ commit, dispatch }, { router }) {
      commit('clearOpened')
      commit('keepAliveClean')
      dispatch('cacheInsert')
      // 跳转到首页
      router.push('/')
    },
    /**
     * @description 缓存插入
     */
     cacheInsert({ state }) {
      store.set(`${state.cacheKey}`, state.opened)
    }
  },
  
})


