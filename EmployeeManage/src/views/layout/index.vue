<template>
    <div class="nm-wrapper">
        <nm-header />
        <nm-body />
    </div>
</template>

<script>
    import NmHeader from './component/header'
    import NmBody from './component/body'
    export default{
        name:"layout",
        components:{
            NmHeader,
            NmBody
        },
        data(){
            return{

            }
        },
        methods:{

        }
    }
</script>

<style type="text/css">
    .nm-wrapper{
        margin:0;
        padding:0;
        height:100%;
        display:flex;
        -webkit-box-align:stretch;
        align-items:stretch;
        -webkit-box-orient:vertical;
        -webkit-box-direction:normal;
        flex-direction:column;
    }
</style>