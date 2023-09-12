<template>
    <div>
        <el-card style="margin-left:10px;margin-right:10px;margin-top:10px;">
            <div>
                <div style="width:100%;float:left;margin-left:10px;margin-bottom:10px;">
                    <el-input v-model="queryParam.employeeId" placeholder="工号" ></el-input>&nbsp;
                    
                    <el-select v-model="queryParam.department" placeholder="车间" style="width:180px !important;" clearable>
                        <el-option
                            v-for="item in departmentOption"
                            :key="item.value"
                            :label="item.label"
                            :value="item.value"
                        >
                        </el-option>
                    </el-select>&nbsp;
                    <el-button-group>
                        <el-button type="primary" @click="SearchBtn" size="medium" icon="el-icon-search">查询</el-button>
                        <el-button type="primary" @click="AddInfoBtn" size="medium" icon="el-icon-plus">新增</el-button>
                    </el-button-group>
                </div>
                <el-table :data="infoList" border style="width: 95%;margin-left:12px;margin-bottom:10px;" stripe :header-cell-style="{background:'#f5f7fa'}" >
                    <el-table-column type="index" fixed label="序号" prop="index" align="center" width="80px">
                        <template slot-scope="scope">
                            <span>{{ (queryParam.pageNum - 1) * queryParam.pagesize + scope.$index + 1}}</span>
                            <!-- (page - 1) * this.pageSize +  -->
                        </template>
                    </el-table-column>
                    <el-table-column label="Guid" align="center" width="180px" prop="Guid" v-if="false"></el-table-column>
                    <el-table-column label="姓名" align="center" width="180px" prop="DisplayName" ></el-table-column>
                    <el-table-column label="工号" align="center" width="180px" prop="EmployeeId" ></el-table-column>
                    <el-table-column label="部门" align="center" width="180px" prop="Department" ></el-table-column>
                    <el-table-column label="账号" align="center" width="180px" prop="LoginName" ></el-table-column>
                    <el-table-column label="密码" align="center" width="190px" prop="Password" ></el-table-column>
                    <el-table-column label="角色" align="center" width="180px" prop="UserType" :formatter="StatusFormat"></el-table-column>
                    <el-table-column label="启用/禁用" align="center" width="180px" prop="Status" >
                        <template slot-scope="scope">
                            <el-tag plain :type="scope.row.Status=='1'?'success':'warning'">
                                {{scope.row.Status=='1'?'启用':'禁用'}}
                            </el-tag>
                        </template>
                    </el-table-column>
                    <el-table-column label="操作" align="center"   header-align="center"  >
                        <template slot-scope="scope">
                            <!-- 修改按钮 -->
                            <el-tooltip
                                :enterable="false"
                                class="item"
                                content="修改"
                                effect="dark"
                                placement="top"
                            >
                                <el-button
                                    @click="updateInfo(scope.row)"
                                    circle
                                    icon="el-icon-edit"
                                    size="mini"
                                    type="primary"
                                ></el-button>
                            </el-tooltip>
                            <!-- 删除按钮 -->
                            <el-tooltip
                                :enterable="false"
                                class="item"
                                content="删除"
                                effect="dark"
                                placement="top"
                            >
                                <el-button
                                    @click="delInfo(scope.row)"
                                    circle
                                    icon="el-icon-delete"
                                    size="mini"
                                    type="primary"
                                ></el-button>
                            </el-tooltip>
                            
                        </template>
                    </el-table-column>
                </el-table>
                <el-pagination
                        :current-page="queryParam.pageNum"
                        :page-size="queryParam.pagesize"
                        :page-sizes="[10,20]"
                        :total="infoTotal"
                        @current-change="PageChange"
                        @size-change="SizeChange"
                        background
                        layout="prev, pager, next, jumper, sizes, total"
                ></el-pagination>
            </div>
        </el-card>

        <el-dialog
            :visible.sync="DialogVisible"
            @close="DialogVisible=false"
            title="账号管理"
            width="40%"
            class="abow_dialog"
            :close-on-click-modal="false"
        >
            <div class="dialogDiv">
                
                <el-form style="margin-left:50px;" label-width="25%" ref="addInfoForm" :model="addInfoForm" label-position="left" :rules="rules">
                    <el-form-item label="Guid:" prop="Guid" style="font-size:18px;" v-if="false">
                        <el-input style="width:180px;" v-model="addInfoForm.Guid"  placeholder="请输入姓名"></el-input>
                    </el-form-item>
                    <el-form-item label="姓名:" prop="DisplayName" style="font-size:18px;">
                        <el-input style="width:180px;" v-model="addInfoForm.DisplayName"  placeholder="请输入姓名"></el-input>
                    </el-form-item>
                    <el-form-item label="工号:" prop="EmployeeId">
                        <el-input style="width:180px;" v-model="addInfoForm.EmployeeId"  placeholder="请输入工号"></el-input>
                    </el-form-item>
                    <el-form-item label="部门:" prop="department">
                        <el-select v-model="addInfoForm.department" placeholder="请选择" style="width:180px;">
                            <el-option
                                v-for="item in departmentOption"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            >
                            </el-option>
                        </el-select>
                    </el-form-item>
                     <el-form-item label="角色">
                        <el-radio-group v-model="addInfoForm.UserType">
                        <el-radio label="2">普通用户</el-radio>
                        <el-radio label="1">管理员</el-radio>
                        <el-radio label="0">超级管理员</el-radio>
                        </el-radio-group>
                    </el-form-item>
                    <el-form-item label="账号:" prop="LoginName">
                        <el-input style="width:180px;" v-model="addInfoForm.LoginName"  placeholder="请输入账号"></el-input>
                    </el-form-item>
                    <el-form-item label="密码:" prop="Password">
                        <el-input type="password" v-model="addInfoForm.Password" autocomplete="off" placeholder="密码"   show-password></el-input>
                    </el-form-item>
                    <el-form-item label="确认密码:" prop="checkPassword">
                        <el-input type="password" v-model="addInfoForm.checkPassword" autocomplete="off" placeholder="密码"   show-password></el-input>
                    </el-form-item>
                    <el-form-item label="禁用/启用:" prop="Status">
                        <el-switch
                            v-model="addInfoForm.Status"
                            active-color="#13ce66"
                            inactive-color="#ff4949"
                            :active-value="1"
                            :inactive-value="0"
                            active-text="启用"
                            inactive-text="禁用">
                            </el-switch>
                    </el-form-item>
                    
                </el-form>
            </div>
            
            <div slot="footer" class="dialog-footer">
                <el-button type="primary" @click="UpdateForm('addInfoForm')" v-show="upBtnShow">更新</el-button>
                <el-button type="primary" @click="submitForm('addInfoForm')" v-show="saveBtnShow">保存</el-button>
                <el-button type="primary" @click="DialogVisible = false">关闭</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
    export default{
        name:'UserManage',
        data: function(){
            var validatePass = (rule,value,callback)=>{
                let reg= /^(?=.*[0-9])(?=.*[a-zA-Z])[0-9a-zA-Z!@#$\%\^\&\*\(\)]{6,16}/
                if(value === ''){
                    callback(new Error('请输入密码'))
                }else if(!reg.test(value)){
                    callback(new Error('密码必须是由6-16位字母+数字组合'))
                }
                else{
                    if(this.addInfoForm.checkPassword !== ''){
                        this.$refs.addInfoForm.validateField('checkPassword')
                    }
                    callback()
                }
            };
            var validatePass2 = (rule, value, callback) => {
                if (value === '') {
                    callback(new Error('请再次输入密码'))
                } else if (value !== this.addInfoForm.Password) {
                    callback(new Error('两次输入密码不一致!'))
                } else {
                    callback()
                }
            };
            return{
                DialogVisible:false,
                upBtnShow: false,
                saveBtnShow: false,
                queryParam:{
                    employeeId: '',
                    department: '',
                    pageNum: 1,
                    pagesize:10
                },
                departmentOption:[],
                infoList:[],
                infoTotal:0,
                addInfoForm:{
                    Guid: '',
                    DisplayName: '' ,
                    EmployeeId: '' ,
                    department: '' ,
                    LoginName: '' ,
                    Password: '',
                    checkPassword: '',
                    Status: 1,
                    UserType: '1'
                },
                rules: {
                    Password: [
                        { required: true, message: '请输入密码' },
                        {validator: validatePass, trigger: 'blur'},
                    ],
                    checkPassword: [
                        { required: true, validator: validatePass2, trigger: 'blur' }
                    ],
                    DisplayName: [
                        { required: true, message: '请输入姓名', trigger: 'blur' }
                    ],
                    EmployeeId: [
                        { required: true, message: '请输入工号', trigger: 'blur' }
                    ],
                    LoginName: [
                        { required: true, message: '请输入账号', trigger: 'blur' }
                    ]
                }
            }
        },
        created: function(){
            this.getDeptOp()
            this.GetAccountInfo()
        },
        mounted: function(){

        },
        methods:{
            StatusFormat(row, column){
                // if(row.UserType == 0){
                //     return '超级管理员'
                // }else if(row.UserType == 1){
                //     return '普通用户'
                // }else if(row.UserType == 2){
                //     return '管理员'
                // }
                return this.GetUserType(row.UserType)
            },
            UpdateForm(formName){
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        this.$http.get('Auth/AccountUp',{
                            params: this.addInfoForm
                        }).then(res => {
                            const result = res.data
                            if(result.code == "200"){
                                this.$message({
                                    showClose: true,
                                    message: result.message,
                                    type: 'success',
                                    duration: 2000,
                                })
                                this.DialogVisible = false
                                this.$refs.addInfoForm.resetFields()
                                this.GetAccountInfo()
                            }else{
                                this.$message({
                                    showClose: true,
                                    message: result.message,
                                    type: 'error',
                                    duration: 2000,
                                })
                            }
                        })
                    }else{
                        this.$message({
                            showClose: true,
                            message: '表单验证不通过!',
                            type: 'error',
                            duration: 2000,
                        })
                        return false;
                    }
                })
                
            },
            submitForm(formName){
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        this.addInfoForm.Status == true? 1 : 0
                        this.$http.get('Auth/Regist', {
                            params: this.addInfoForm
                        }).then(res => {
                            const result = res.data
                            if(result.code == "200"){
                                this.$message({
                                    showClose: true,
                                    message: result.message,
                                    type: 'success',
                                    duration: 2000,
                                })
                                this.$nextTick(() => {
                                    this.$refs.addInfoForm.resetFields()
                                })
                                this.DialogVisible = false
                                this.GetAccountInfo()
                            }else{
                                this.$message({
                                    showClose: true,
                                    message: result.message,
                                    type: 'error',
                                    duration: 2000,
                                })
                            }
                        })
                    } else {
                        this.$message({
                            showClose: true,
                            message: '表单验证不通过!',
                            type: 'error',
                            duration: 2000,
                        })
                        return false;
                    }
                });
            },
            getDeptOp: function(){
                this.$http.get('EmployeeInfoMain/GetDeptOption').then(res => {
                    const result = res.data
                    this.departmentOption = result.Data
                    //this.queryParam.department = this.departmentOption[0].value
                })
            },
            SearchBtn(){
                this.GetAccountInfo()
            },
            AddInfoBtn(){
                this.$nextTick(() => {
                    this.$refs.addInfoForm.resetFields()
                })
                this.upBtnShow = false
                this.saveBtnShow = true
                this.DialogVisible = true
            },
            GetAccountInfo(){
                this.$http.get('Auth/GetAccountList',{
                    params:this.queryParam
                }).then(res => {
                    const result = res.data
                    this.infoList = result.rows
                    this.infoTotal = result.total
                })
            },
            updateInfo(e){
                this.DialogVisible = true
                this.upBtnShow = true
                this.saveBtnShow = false
                this.addInfoForm.Guid = e.Guid
                this.addInfoForm.DisplayName = e.DisplayName
                this.addInfoForm.EmployeeId = e.EmployeeId
                this.addInfoForm.department = e.Department
                this.addInfoForm.LoginName = e.LoginName
                this.addInfoForm.Password = e.Password
                this.addInfoForm.checkPassword = e.Password
                this.addInfoForm.Status = e.Status 
                this.addInfoForm.UserType =  e.UserType + ""
                //console.log(e)
            },
            GetUserType(userType){
                if(userType == 0){
                    return "超级管理员"
                }else if(userType == 1){
                    return "管理员"
                }else{
                    return "普通用户"
                }
            },
            delInfo(e){
                this.$confirm('确定删除?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(res =>{
                    this.$http.get('Auth/AccountDel',{
                        params:{
                            loginName: e.LoginName
                        }
                    }).then(res => {
                        const result = res.data
                        if(result.code == "200"){
                            this.GetAccountInfo()
                        }else{
                            this.$message({
                                showClose: true,
                                message: result.message,
                                type: 'error',
                                duration: 2000,
                            })
                        }
                    })
                })
            },
            PageChange: function(newPage){
                this.queryParam.PageNum = newPage
                this.GetAccountInfo()
            },
            SizeChange: function(newSize){
                this.queryParam.pageSize = newSize
                this.GetAccountInfo()
            }
        }
    }
</script>

<style stype="text/css" scoped>
    .el-input{
        width:180px;
    }
    .el-input__inner {
        width:180px !important;
        height:30px !important;
        line-height: 30px !important;
    }
</style>