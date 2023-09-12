import Vue from 'vue'
import VueRouter from 'vue-router'
import Login from '../views/login'
import Page404 from '../views/erroPage/404.vue'
import Home from '../views/home'
import Layout from "../views/layout"
import {getToken} from '../libs/util'
import store from '@/store/index'

Vue.use(VueRouter)

// 解决重复点击路由报错的BUG
const originalPush = VueRouter.prototype.push
VueRouter.prototype.push = function push(location) {
    return originalPush.call(this, location).catch((err) => err)
}

const routes = [
    {
        path: '/login',
        name: 'Login',
        component: Login
    }
	
]

export const DynamicRoutes = [
	{
		path: "",
		component: Layout,
		name: 'layout',
		redirect: "home",
		meta:{
			requiresAuth: false,
			name: "首页"
		},
		children:[
			{
				path: "home",
				component: Home,
				name: "home",
				meta:{
					keepAlive: true,
					name: "首页",
					icon:"el-icon-s-home"
				}
			}
		]
	},
	{
		path: "*",
		component: Page404
	}
]

// 定义静态路由集合
const staticRouterMap = [
	'login',
	'404',
	'DATAVIS'
]

const router = new VueRouter({
	//mode: 'hash',
	mode: 'hash',
	base: process.env.BASE_URL,
	routes,
})

router.beforeEach((to, from, next) => {
	const hasToken = getToken()
	if (!hasToken) {
		// 没有登录
		// 如果前往页面非公共路由，则跳转至首页
		
		if (staticRouterMap.indexOf(to.path) < 0) {
			next()
		} else {
			//next(`/login?redirect=${to.path}`)
			next("/login")
		}
	} else {
		if (to.path == '/login') {
			next()
		}else{
			// 登录
			// 已经存在路由列表: 注意刚登陆成功第一次调转route时相应store数据还未更新
			const hasGetRoute = store.getters['user/hasGetRoute']
			const routeMap = JSON.parse(sessionStorage.getItem('routeMap'))
			if(!hasGetRoute && routeMap) {
				// 刷新页面且有route记录数据，可再次追加动态路由
				store.dispatch('user/updateRouteOfUser', routeMap)
				next({...to, replace: true})
			} else {
				next()
			}
		}
		
	}
})

export default router



