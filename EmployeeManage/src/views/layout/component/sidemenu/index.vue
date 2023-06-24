<template>
    <div class="nm-sidebar" :class="{collapse:collapse}">
      <div class="nm-menu-wapper">
        <div class="nm-menus">
            <el-menu 
              :unique-opend="true"
              :default-active="this.$route.path" 
              background-color="#324157"
              text-color="#fff"
              router
              active-text-color="#fff" 
              :default-openeds="openList"
              :collapse-transition="false"
              :collapse="collapse">
              <AsideItems :menu="menu" />
            </el-menu>
        </div>
      </div>
    </div>
    
</template>



<script>
  import { mapState } from 'vuex'
  import AsideItems from './AsideItems'
  export default {
    components:{
      AsideItems
    },
    data() {
      return {
        openList:['User'],
        menu:[],
        openedTab:[]
      }
    },
    mounted(){
      const mRoutes = JSON.parse(sessionStorage.getItem('routeMap'))
      this.menu = mRoutes
    },
    computed:{
      ...mapState(['collapse'])
      
    },
    methods: {
     
    },
    created: function(){
      //this.$router.push('/DataMain')
    }
  }
</script>

<style>
  .nm-sidebar{
    position:relative;
    display:flex;
    width:230px;
    height:100%;
    -webkit-box-align:stretch;
    align-items:stretch;
    -webkit-box-orient:vertical;
    -webkit-box-direction:normal;
    flex-direction:column;
    flex-shrink:0;
    background-color:#222d32;
  }
  .nm-sidebar.collapse{
    width:65px;
  }
  .nm-menu-wapper{
    position:relative;
    -webkit-box-flex:1;
    flex-grow:1;
  }
  /* .el-scrollbar{
    height:100%;
    overflow:hidden;
    position:relative;
  } */
  /* .nm-navigation{
    position:fixed;
    height:100%;
    /* width:230px; */
    /* margin-top:50px; */
  .nm-menus{
    position:absolute;
    height:100%;
    width:100%;
    box-sizing:border-box;
  }
  /* #1c88cf  #0360B1*/
  .el-menu-item.is-active {
      background-color:  #0c8308 !important;
      color: red;
  }
  .el-menu{
    height:100%;
  }
</style>