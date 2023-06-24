<template>
    <div class="nm-header" >
        <div style="-webkit-flex: 1;flex:1;">
            <a class="sidebar-toggle-btn-left" @click.prevent="sidebarToggle">
                <i :class="sidebarCollapse ? 'el-icon-s-unfold' : 'el-icon-s-fold'"></i>
            </a>
        </div>
        
        <div style="font-weight:bold;font-size:18px;color:white;line-height:50px;-webkit-flex: 5;flex:5;text-align:center;">
            车间管理
        </div>

        <!-- <div style="-webkit-flex: 1;flex:1;">
            <el-tooltip :content="screenIcon ? '全屏' : '退出全屏' " placement="bottom">
                <i :class="screenIcon ? 'el-icon-full-screen' : 'el-icon-rank'" @click="handleChange" class="nm-sidebar-toggle-btn"></i>
            </el-tooltip>
        </div> -->

        <div class="nm-header-right">
            <div class="nm-toolbar">
                <el-dropdown style="cursor: pointer;margin-right:10px;">
                    <i :class="userIcon" class="userIcon"></i>
                    <span style="font-size:12px;color:#fff;margin-left:-10px;">{{this.$store.state.user.userInfo.user_name}}</span>
                    <!-- {{this.$store.state.userInfo.userName}} -->
                    <el-dropdown-menu slot="dropdown">
                        <el-dropdown-item name="message">账户信息</el-dropdown-item>
                        <el-dropdown-item name="logout">修改密码</el-dropdown-item>
                    </el-dropdown-menu>
                </el-dropdown>

                <el-tooltip content="退出登录" placement="bottom">
                    <i :class="logoOutIcon" class="nm-sidebar-toggle-btn" @click="logout"></i>
                </el-tooltip>
                 <el-tooltip :content="screenIcon ? '全屏' : '退出全屏' " placement="bottom">
                    <i :class="screenIcon ? 'el-icon-full-screen' : 'el-icon-rank'" @click="handleChange" class="nm-sidebar-toggle-btn"></i>
                </el-tooltip>
            </div>
        </div>
    </div>
</template>

<script>
    import { mapState, mapGetters, mapActions } from 'vuex'
    export default{  
        data(){
            return{
                userIcon: 'el-icon-user-solid',
                logoOutIcon: 'el-icon-switch-button'
            }
        },
        computed:{
            
            ...mapState({sidebarCollapse: 'collapse'}),
            ...mapState({screenIcon: 'active'}),
            
        },
        methods:{
            ...mapActions({ sidebarToggle: 'toggle' }),
            ...mapActions({ handleChange: 'screenToggle'}),
            logout(){
                this.$confirm("您确认要退出登录吗", "提示", {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(async () =>{
                    setTimeout(
                        ()=>{
                            sessionStorage.clear()
                            this.$store.commit('setToken', '')
                            this.$store.commit('resetLogin')
                            // console.log("name:")
                            // const userInfo = JSON.parse(sessionStorage.getItem('userInfo'))
                            // console.log(userInfo)
                            this.$router.push("/login")
                            location.reload();
                        }
                    ,1500);
                })
            },
        },
        mounted(){
            const userInfo = JSON.parse(sessionStorage.getItem('userInfo'))
        },
        created(){
        }
    }
</script>

<style type="text/css">
    .nm-header{
        /* padding-right:0;
        display:flex;
        flex-shrink:0;
        height:50px;
        background:#03a9f4;
        box-shadow:0 1px 1px rgb(0 0 0 / 10%); */
        /* background:#03a9f4;  bottom repeat-x*/
        /* url(../../assets/images/bg_header.jpg)  ;  #03A9F4;*/
        background-color: #242F42;
        background-size: 100% 100%;
        background-position: center center;
        padding-right:0;
        display:flex;
        -webkit-justify-content: space-around;
        justify-content: space-around;
        flex-shrink:0;
        height:50px;
        box-shadow:0 1px 1px rgb(0 0 0 / 10%);
    }
    .nm-logo-img{
        float:left;
        margin:5px 11px;
        width:40px;
        height:40px;
    }
    

    .nm-logo-text{
        color:#fff;
        float:left;
        height:50px;
        line-height:50px;
        font-size:20px;
        white-space:nowrap;
    }
    .nm-logo-box{
        padding-right:5px;
        height:50px;
        width:225px;
        line-height:50px;
        box-shadow:0 0 4px rgb(0 0 0 / 26%), 0 -1px 0 rgb(0 0 0 / 2%);
        z-index:1;
        float:left;
    }
    .nm-sidebar-toggle-btn{
        color:#fff;
        position: relative;
        float: right;
        height:50px;
        line-height:50px;
        width:50px;
        font-size:25px;
        text-align:center;
        cursor:pointer;
    }
    .sidebar-toggle-btn-left{
        color:#fff;
        position: relative;
        float: left;
        height:50px;
        line-height:50px;
        width:50px;
        font-size:25px;
        text-align:center;
        cursor:pointer;
    }
    .userIcon{
        color:#fff;
        position: relative;
        float: left;
        height:50px;
        line-height:50px;
        width:50px;
        font-size:25px;
        text-align:center;
        cursor:pointer;
    }
    
    .nm-header-toolbar{
        float:right;
        height:50px;
        line-height:50px;
        margin-right:10px;
    }

    .el-badge__content.is-fixed.is-dot{
        top:12px;
    }

    .el-avatar--icon{
        font-size:20px;
    }

    .el-avatar el-avatar--icon el-avatar--circle{
        vertical-align: middle;
    }

    .nm-header-right{
        /* float:right;
        width:200px;
        height:50px;
        line-height:50px;
        -webkit-box-pack:end;
        justify-content:flex-end; */
        display:flex;
        -webkit-box-flex:1;
        flex-grow:1;
        height:50px;
        line-height:50px;
        -webkit-box-pack:end;
        justify-content:flex-end;
    }

    .nm-toolbar{
        height:50px;
        overflow:hidden;
    }

    .nm-sidebar-toggle-btn{
        color:#fff;
        position: relative;
        float: right;
        height:50px;
        line-height:50px;
        width:50px;
        font-size:25px;
        text-align:center;
        cursor:pointer;
    }

</style>