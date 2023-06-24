<template>
  <div>
    <!-- 一定要加template遍历  递归调用 不然无效 -->
    <template v-for="(item, i) in menu">
      <!-- 判断没有子路由的 -->
      <el-menu-item
        v-if="!item.children"
        :key="i"
        :index="item.path"
        @click="clickMenu(item.path, item.meta.title, item.meta.icon, item.name)"
      >
        <i :class="item.meta.icon"></i>
        <span slot="title">{{ item.meta.title }}</span>
      </el-menu-item>
      <!-- 有子路由的导航 -->
      <el-submenu v-else :key="i" :index="item.name" >
        <template slot="title">
          <i :class="item.meta.icon"></i>
          <span slot="title">{{ item.meta.title }}</span>
        </template>
        <!-- 子路由 -->
        <aside-item :menu="item.children"></aside-item>
      </el-submenu>
    </template>
  </div>
</template>
 
<script>
import { mapState } from 'vuex'
export default {
  name: "AsideItem",
  props: {
    menu: { type: Array },
  },
  data(){
    return{
      openedTab:[]
    } 
  },
  computed:{
    ...mapState(['collapse'])
  },
  methods:{
    clickMenu(pageIndex, pageName, pageIcon, component){
      this.openedTab = this.$store.state.opened
      
      const pageInfo = {"path":pageIndex, "tabName":pageName, 'icon':pageIcon}
      this.$store.state.current = pageInfo

      let arr = this.openedTab
      let tabNum = arr.findIndex((arr) => arr.path == pageIndex)
      if(tabNum === -1){
          //该标签当前没打开
          this.$store.commit('add', pageInfo)
          this.$store.commit('keepAliveAdd', component)
      }else{
          this.$store.commit('changeTab', pageInfo)
      }
    }
  }
};
</script>