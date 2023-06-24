<template>
    <div class="nm-main">
        <div class="nm-main-left">
            <nm-sidemenu />
        </div>
            <!-- <nm-tabnav>-->
        <div class="nm-main-right">
            <nm-tabnav class="nm-tabnav"/>
            
            <div class="nm-content">
                <!-- :include="keep-alive" -->
                <!-- 动态设置页面缓存！！！！！！！！！！！！！！ -->
                <!-- <keep-alive :include="keepAlive">
                    <router-view :key="key"></router-view> 
                </keep-alive> -->
                
                <!-- 全部页面都设置缓存 -->
                <keep-alive>
                    <router-view v-if="$route.meta.keepAlive" :key="key"></router-view> 
                    <router-view v-else></router-view> 
                </keep-alive>
            </div>
            
            
        </div>
    </div>
</template>

<script>
    import NmSidemenu from '../sidemenu'
    import NmTabnav from '../tabnav'
    import { mapState } from 'vuex'
    // import NmTabnav from '../tabnav'
    export default{
        components:{
            NmSidemenu,
            NmTabnav
        },
        data(){
            return {
                
            }
        },
        created(){
        },
        methods:{
            reload(name) {
                this.routerViewVisible = false
                //this.keepAliveRemove(name)

                this.$nextTick(() => {
                    this.routerViewVisible = true
                })
            }
        },
        computed:{
            ...mapState(['collapse']),
            keepAlive(){
                console.log(this.$store.state.keepAlive)
                return this.$store.state.keepAlive
            },
            key(){
                return this.$route.name
            }

        },
        provide() {
            return {
            reload: this.reload
            }
        },
    }
</script>

<style type="text/css">
    .nm-main{
        display:flex;
        -webkit-box-align:stretch;
        align-items:stretch;
        -webkit-box-flex:1;
        flex-grow:1;
        -webkit-box-orient:horizontal;
        -webkit-box-direction:normal;
        flex-direction:row;
        overflow:hidden;
    }
    .nm-main-left{
        /* display:flex;
        width:230px;
        height:100%;
        align-items:stretch;
        flex-direction: column; */
        display:flex;
        flex-shrink:0;
        z-index:1;
        background-color:#222d32;
    }
    .nm-content{
        position: relative;
        -webkit-box-flex: 1;
        flex-grow: 1;
        width:100%;
        height:100%;
        overflow:scroll;
        background-color:aliceblue ;
        /* margin-top:40px; */
    }
    .nm-main-right{
        /* display:flex;
        -webkit-box-align:stretch;
        align-items: stretch;
        -webkit-box-flex: 1;
        flex-grow: 1;
        -webkit-box-orient: vertical;
        -webkit-box-direction: normal;
        flex-direction: column;
        overflow-y: scroll; */
        /* margin-top:50px; */
        width:100%;
        overflow: hidden;
    }
    .nm-tabnav{
        /* position :relative; */
        width:100%;
        border-top:1px solid #F0F0F0;
        border-bottom:1px solid #F0F0F0;
    }
</style>