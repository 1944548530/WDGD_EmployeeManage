import Cookies from 'js-cookie'

export const TOKEN_KEY = 'token'

export const setToken = (token) => {
    localStorage.setItem(TOKEN_KEY, token);
}

export const getToken = () => {
    const token = localStorage.getItem(TOKEN_KEY);
    if (token) return token
    else return ""
}

//路由动态生成(后端返回什么，就展示什么)
export const addDynamicRoutes = (res) => {
    res.forEach(v=>{
        routes.push({
            path:v.path,
            name:v.name,
            meta:{title:v.title},
            component:()=>import('@/views/user/${item.component}' + v.component)
        })
    });
    return routes;
}
// export const getMenuList = () => {
//     this.$http.get('Auth/GetMenuList', {
//         params:{
//             employeeId: this.$store.state.employeeId,
//         }
//     }).then(res => {
//         const data = res.data.Data
//         return data
//     })
// }