<template>
  <div id="app">
      <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: 'App',
  mounted() {
      window.addEventListener('unload',this.saveState)
  },
  methods: {
    saveState(){
      localStorage.setItem("state",JSON.stringify(this.$store.state))
    }
  },

}
</script>

<style type="text/css">
  #nprogress .bar {
    /* rgb(255, 145, 0) */
    background:  rgb(255, 145, 0) !important; 
    height: 5px !important;
  }
</style>
