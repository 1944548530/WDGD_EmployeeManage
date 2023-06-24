<template>
    <div class="nm-login-default">
        <!-- <div class="nm-login-title">
            <img class="nm-login-logo-img" src="../../../assets/images/logo.png"/>
            <h1 style="color:white">Vue框架</h1>
        </div> -->
        <div class="loginDiv">
            <div class="login-top">登录</div>
            <el-form ref="form" :model="form" :rules="rules" label-width="18%" label-position="left">
                <el-form-item prop="userName">
                    <el-input v-model="form.userName" placeholder="用户名">
                    <template v-slot:prefix>
                        <i class="el-icon-user-solid"></i>
                    </template>
                    </el-input>
                </el-form-item>

                <el-form-item prop="password">
                    <el-input type="password" v-model="form.password" autocomplete="off" placeholder="密码" prefix-icon="el-icon-lock"  show-password>
                    </el-input>
                </el-form-item>
                <div class="errMesDiv">
                    <span ref="erroMess" id="erroMess" class="erroMessClass"></span>
                </div>
                <div class="login-bottom">
                    <el-button :loading="loading" class="btn-login" type="info" @click="tryLogin">登录</el-button>
                </div>
            </el-form>
        </div>
        <!-- <div class="copyright">文迪广电 Powered by .Net Core 3.1.0</div> -->
        
    </div>
</template>


<script>
    import {getToken} from '@/libs/util'
    import {getUserInfo} from '@/api/user'
    import {
        mapGetters,
        mapMutations,
        mapActions
    }from 'vuex'

    export default{
        name: 'login',
        data(){
            return{
                form:{
                    username: '',
                    password: '',
                    
                },
                rules:{
                    userName:[
                        {
                            required: true,
                            message:'请输入用户名',
                            trigger: 'blur'
                        }
                    ],
                    password:[
                        {
                            required: true,
                            message: '请输入密码',
                            trigger: 'blur'
                        }
                    ]
                },
                loading: false
            }
            
        },
        computed:{
            ...mapGetters('user', ['isLogin', 'userInfo', 'hasGetRoute'])
        },
        methods: {
            ...mapMutations('user', ['setUserState']),
            ...mapActions('user', ['getUserInfo', 'getDynamicRouteOfUser']),
            tryLogin(){
                this.loading = true
                this.form.userName =  this.form.userName.trim();
                this.$http.get('Auth/Login',{
                    params:{
                        username: this.form.userName,
                        password: this.form.password
                    }
                }).then(res => {
                    const data = res.data
                    if(data.code == 200){
                        this.$store.commit('setToken', data.Data)
                        this.$message({
                            showClose: true,
                            message: "用户信息验证成功，正在登录系统...",
                            type: "success",
                            duration: 2000,
                        }); 
                        this.getUserInfo()
                        //以下就是根据用户登陆信息，获取动态路由信息操作
                        window.setTimeout(
                            () =>  this.getDynamicRouteOfUser()
                        ,1000)
                        window.setTimeout(
                            () => this.$router.replace("/home")
                        , 2000)
                    }else{
                        this.loading = false
                        this.$refs.erroMess.innerHTML = res.data.message
                    }
                   
                })
                
                
            }
        }
    }
</script>

<style type="text/css" scoped>
    .login-top{
        font-size: 24px;
        text-align: center;
        box-sizing: border-box;
        color: #333;
        margin-bottom: 20px;
        padding-top: 50px;
    }
    .login-bottom{
        padding-top: 10px;
        width:100%;
        text-align: center;
    }
    .errMesDiv{
        width:250px;
        margin:0 auto;
        margin-top:-10px;
    }
    .erroMessClass{
        color:crimson;
    }
    .el-form-item .is-required{
        height:30px !important;
        line-height:30px !important;
    }
    .nm-login-default{
        height:100%;
        width:100%;
        background:url('../../assets/images/logo.png') no-repeat;
        background-size: cover;
        background-attachment: fixed;
        overflow: hidden;
    }
    .loginDiv{
        margin:250px auto;
        position: relative;
        /* margin-left:230px;
        margin-top:250px; */
        margin-left:15%;
        margin-top:15%;
        width:400px;
        height:430px;
        background: #FFF;
    }
    .el-input{
        width:250px;
    }
    .el-input__inner {
        width:250px !important;
        height:40px !important;
        line-height: 40px !important;
    }
    .btn-login {
      width: 250px;
      cursor: pointer;
      border: 1px solid #454a54;
      padding: 15px 20px;
      background:darkblue;
      color: #FFF;
    }
    .copyright {
        position: absolute;
        bottom: 22px;
        width: 100%;
        text-align: center;
        color: #FFF;
        font-size: 13px;
    } 
</style>
