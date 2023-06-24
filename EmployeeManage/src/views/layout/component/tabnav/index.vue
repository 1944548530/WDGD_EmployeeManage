<template>
  <div class="nm-tabnav">
    <slot name="before" />
    <div class="nm-tabnav-tabs" >
      <el-tabs :value="current.path" type="card" :closable="true" @tab-click="onTabClick" @tab-remove="onTabRemove" @contextmenu.prevent.native="showContextMenu" class="tabs__item">
        <el-tab-pane v-for="(item, i) in opened" :key="item.path" :name="item.path"
          ><span slot="label" :index="i"> <i :class="item.icon" ></i>{{ item.tabName }}</span>
        </el-tab-pane>
        <!--<nm-icon v-if="config.showIcon && item.icon" :name="item.icon" /><!-->
      </el-tabs>
    </div>
    <div class="nm-tabnav-control">
      <el-dropdown @command="cmd => handleCommand(cmd)" style="display: inline-block;position:relative;color:#606266;font-size:14px;width:30px;height:40px;">
        <span class="nm-tabnav-control-btn">
          <i class="el-icon-caret-bottom" ></i>
        </span>
        <el-dropdown-menu slot="dropdown">
          <el-dropdown-item command="Left"> <i class="el-icon-back" ></i>关闭左侧 </el-dropdown-item>
          <el-dropdown-item command="Right"> <i class="el-icon-right" ></i>关闭右侧 </el-dropdown-item>
          <el-dropdown-item command="Other"> <i class="el-icon-more" ></i>关闭其他 </el-dropdown-item>
          <el-dropdown-item command="All"> <i class="el-icon-menu" ></i>全部关闭 </el-dropdown-item>
        </el-dropdown-menu>
      </el-dropdown>
    </div>
    <div ref="contextmenu" class="nm-tabnav-contextmenu" v-show="contextmenu.visible" :style="{ top: contextmenu.top, left: contextmenu.left }">
      <ul>
        <li @click.stop="onRefresh"><i class="el-icon-help" ></i>重新载入</li>
        <li @click.stop="onClose('')"><i class="el-icon-close" ></i>关闭</li>
        <li @click.stop="onClose('Left')"><i class="el-icon-arrow-left" ></i>关闭左侧</li>
        <li @click.stop="onClose('Right')"><i class="el-icon-arrow-right" ></i>关闭右侧</li>
        <li @click.stop="onClose('Other')"><i class="el-icon-circle-close" ></i>关闭其它</li>
        <li @click.stop="onClose('All')"><i class="el-icon-circle-close" ></i>关闭全部</li>
      </ul>
    </div>
  </div>
</template>
<script>
import { mapState, mapActions } from 'vuex'
import { on, off, hasClass } from '../../../../libs/dom'
export default {
  name: 'Tabnav',
  data() {
    return {
      contextmenu: {
        visible: false,
        top: 0,
        left: 0
      },
      closeParams: { index: 0, router: this.$router }
    }
  },
  computed: {
    ...mapState({ opened: 'opened', current: 'current', defaultPage: 'default' }),
    ...mapState({ config: s => s.component.tabnav })
  },
  inject: ['reload'],
  methods: {
    ...mapActions(['close', 'closeLeft', 'closeRight', 'closeOther', 'closeAll']),
    showContextMenu(event) {
      const { path } = event
      for (let i = 0; i < path.length; i++) {
        if (hasClass(path[i], 'el-tabs__item')) {
          const index = path[i].querySelector('span').getAttribute('index')
          if (index) {
            this.contextmenu = {
              visible: true,
              top: event.y + 'px',
              left: event.x + 'px'
            }
            this.closeParams.index = parseInt(index)
          }
          break
        }
      }
    },
    hideContextMenu(e) {
      const { target } = e || window.event
      if (this.$refs.contextmenu !== target) this.contextmenu.visible = false
    },
    /**
     * @description 处理关闭标签下拉菜单命令
     * @param {String} cmd 命令
     * @param {String} tagName 选择的标签名称
     */
    handleCommand(cmd) {
      this.closeParams.index = this.opened.findIndex(m => m.path === this.current.path)
      this.onClose(cmd)
    },
    onTabClick(tab) {
      //this.current.path = tab.name
      
      const page = this.opened.find(page => page.path === tab.name)
      console.log("page")
      console.log(page)
      this.$store.state.current = page
      this.$router.push(page.path)
    },
    // onTabClick (route) {
    //   this.$store.commit('changeTab', route)
    //   this.$router.push(route)
    // },
    
    onTabRemove(path) {
      this.closeParams.index = this.opened.findIndex(m => m.path === path)
      let openedLen = this.$store.state.opened.length
      if(openedLen > 1){
        if(path != "/home"){
          this.onClose()
        }
      }
      console.log("path")
      console.log(path)
      if(path != "/home"){
        this.$store.commit('keepAliveRemove', path)
      }
      
    },
    onClose(type) {
      this[`close${type || ''}`](this.closeParams)
    },
    onRefresh() {
      this.reload(this.current.name)
    }
  },
  watch: {
    'contextmenu.visible'(val) {
      if (val) {
        on(document, 'mouseup', this.hideContextMenu)
      } else {
        off(document, 'mouseup', this.hideContextMenu)
      }
    }
  },
  created(){
    console.log("opend:")
    console.log(this.$store.state.opened)
  }

}
</script>

<style>
  .nm-tabnav{
    
    display:flex;
    -webkit-box-orient: horizontal;
    -webkit-box-direction: normal;
    flex-direction: row;
    -webkit-box-align: stretch;
    align-items: stretch;
    height:40px;
    width:100%;
  }
  .nm-tabnav-tabs{
    overflow-y: hidden;
    overflow-x: auto;
    -webkit-box-flex: 1;
    flex-grow: 1;
    /* background: #F0F8FF; */
    /* background: #D3D3D3; */
    background:#fff;
    display:flex;
  }
  .el-tabs--card>.el-tabs__header .el-tabs__nav{
    border: none;
  }
  .el-tabs--card>.el-tabs__header .el-tabs__item:first-child{
    border: 1px solid #244CC7;
  }
  .nm-tabnav-control{
    flex-shrink: 0;
    width:30px;
  }
  .nm-tabnav-control-btn{
    height:40px;
    line-height:40px;
    display: inline-block;
    width:30px;
    text-align: center;
    cursor: pointer;
    font-size:16px;
    box-sizing: border-box;
  }
  .el-tabs__item{
    position: relative;
    padding: 0 20px !important;
    /* border-right: 2px solid white; */
    margin:5px 2px;
    user-select: none;
    background-color:#fff;
    /* background-color:#CCCCCC; */
    border-radius: 2px;
    height:30px;
    line-height:30px;
  }
  .el-tabs--card>.el-tabs__header .el-tabs__item.is-active.is-closable{
    background-color:#0c8308 !important;
    color:#fff;

  }
  .el-tabs__item .is-top .is-active{
    border:none !important;
  }
  .el-tabs--card>.el-tabs__header .el-tabs__item:first-child{
    border:none !important;
  }
  .nm-tabnav-contextmenu{
    position:fixed;
    background-color:#fff;
    border-radius:4px;
    box-shadow:0 2px 12px 0 rgb(0 0 0 / 10%);
    z-index:9999;
  }
  .nm-tabnav-contextmenu li{
    list-style:none;
    line-height:30px;
    padding:0 15px;
    margin:0;
    font-size:13px;
    color:#606266;
    cursor:pointer;
    outline:none;
  }
  .nm-tabnav-contextmenu ul{
    padding-inline-start: 5px;

  }
  .el-tabs__nav-prev{
    line-height:45px;
    font-size:20px;
    width:30px;
    text-align:center;
    left:0;
    position:absolute;
    cursor:pointer;
    color:#909399;
  }
  .el-tabs__nav-next{
    line-height:45px;
    right:0;
    position:absolute;
    cursor:pointer;
    color:#909399;
  }
  .nm-tabnav-tabs .el-tabs__nav-wrap.is-scrollable {
    padding: 0 30px;
  }
  .el-tabs__nav-wrap.is-scrollable{
    box-sizing:border-box;
    padding: 0 30px;
  }
  .el-tabs__nav-wrap{
    overflow:hidden;
    margin-bottom:-1px;
    position:relative;
  }
  .container-tab >>> .el-tabs__content{
    flex-grow: 1;
    overflow-y: scroll;
  }
</style>


