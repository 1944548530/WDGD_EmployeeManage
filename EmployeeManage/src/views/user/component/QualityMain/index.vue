<template>
    <div>
        <!-- <el-breadcrumb separator="/">
            <el-breadcrumb-item style="font-weight:bold;">车间管理</el-breadcrumb-item>
            <el-breadcrumb-item style="font-weight:bold;">个人品质档案</el-breadcrumb-item>
        </el-breadcrumb> -->

        <el-card style="margin-left:10px;margin-right:10px;margin-top:10px;">
            <div style="width:500px;float:left;margin-left:10px;margin-bottom:10px;">
                
                <el-select v-model="queryParam.department" placeholder="车间" clearable>
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
                    <el-button type="primary" @click="exportExcel" size="medium" icon="el-icon-download">导出</el-button>
                    <el-button type="primary" @click="AddInfoBtn" size="medium" icon="el-icon-plus">新增</el-button>
                </el-button-group>
            </div>
            <!-- <div style="width:300px;float:right;margin-right:100px;margin-bottom:20px;">
                
            </div> -->
            <el-table :data="infoList" border style="width: 98%;margin-left:10px;margin-bottom:10px;margin-top:10px;" stripe :header-cell-style="{background:'#f5f7fa'}" >
                    <el-table-column type="index" fixed label="序号" prop="index" align="center" width="80px">
                        <template slot-scope="scope">
                            <span>{{ (queryParam.pageNum - 1) * queryParam.pagesize + scope.$index + 1}}</span>
                            <!-- (page - 1) * this.pageSize +  -->
                        </template>
                    </el-table-column>
                    <el-table-column label="姓名" align="center" width="120px" prop="cname" ></el-table-column>
                    <el-table-column label="工号" align="center" width="120px" prop="employeeId" ></el-table-column>
                    <el-table-column label="车间" align="center" width="100px" prop="department" ></el-table-column>
                    <el-table-column label="岗位" align="center" width="100px" prop="postNow" ></el-table-column>
                    <el-table-column label="发生日期" align="center" width="120px" prop="eventDate" ></el-table-column>
                    <el-table-column label="问题归纳" align="center" width="180px" prop="eventType" ></el-table-column>
                    <el-table-column label="问题描述" align="center" width="200px" prop="eventDesc" ></el-table-column>
                    <el-table-column label="处理类型" align="center" width="100px" prop="dealType" ></el-table-column>
                    <el-table-column label="处理结果" align="center" width="150px" prop="dealResult" ></el-table-column>
                    <el-table-column label="录入时间" align="center" width="120px" prop="checkDate" ></el-table-column>
                    <el-table-column label="录入人员姓名" align="center" width="120px" prop="checkName" ></el-table-column>
                    <el-table-column label="录入人员工号" align="center" width="120px" prop="checkEmployeeId" ></el-table-column>
                    <el-table-column label="操作" align="center" header-align="center" fixed="right" width="150px">
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
        </el-card>
        <el-dialog
            :visible.sync="addDialogVisible"
            @close="addDialogVisible=false"
            title="品质信息录入"
            width="70%"
            class="abow_dialog"
            :close-on-click-modal="false"
            style="overflow-x:scroll;"
        >
            <div class="tableDiv">
                <table>
                    <tr>
                        <td>工号</td><td>姓名</td><td>车间</td><td>入职日期</td><td>等级</td><td>技能</td><td>岗位</td>
                    </tr>
                    <tr class="blankCenter">
                        <td>
                            <el-input v-model="addForm.employeeId" placeholder="工号" id="employeeId" @keyup.enter.native="employeeKeyup"></el-input>
                        </td>
                        <td>
                            <span ref="cname" id="cname"></span>
                        </td>
                        <td>
                            <span ref="department" id="department"></span>
                        </td>
                        <td>
                            <span ref="indate"></span>
                        </td>
                        <td>
                            <span ref="grade"></span>
                        </td>
                        <td>
                            <el-tooltip class="item" effect="dark" :content="postSkill" placement="top-start">
                                <span ref="postSkill"></span>
                            </el-tooltip>
                        </td>
                        <td>
                            <span ref="postNow" id="postNow"></span>
                        </td>
                    </tr>
                    <tr>
                        <td><span>发生日期</span></td><td colspan="3"><span>问题描述</span></td><td colspan="3"><span>问题归纳</span></td>
                    </tr>
                    <tr class="blankTr">
                        <td>
                            <el-date-picker 
                                style="width:180px;"
                                size="medium"
                                v-model="addForm.eventDate"
                                value-format="yyyy-MM-dd"
                                placeholder="选择日期"
                            ></el-date-picker>
                        </td>
                        <td colspan="3" rowspan="4" >
                            <el-input type="textarea" :rows="8" v-model="addForm.eventDesc" placeholder="请输入内容" ></el-input>
                        </td>
                        <td colspan="3" rowspan="4">
                            &emsp;<el-radio v-model="addForm.eventType"  label="违反公司纪律">违反公司纪律</el-radio><br><br>
                            &emsp;<el-radio v-model="addForm.eventType" label="人为作业造成返工">人为作业造成返工</el-radio><br><br>
                            &emsp;<el-radio v-model="addForm.eventType" label="人为作业造成报废">人为作业造成报废</el-radio><br><br>
                            &emsp;<el-radio v-model="addForm.eventType" label="主动发现问题，避免公司损失">主动发现问题，避免公司损失</el-radio>
                        </td>
                    </tr>
                    <tr class="blankTr">
                        <td rowspan="3" style="text-align:center;">一</td>
                        
                    </tr>
                    <tr class="blankTr">
                    </tr>
                    <tr class="blankTr">
                    </tr>
                    <tr >
                        <td colspan="2">
                            <span>处理类型</span>
                        </td>
                        <td colspan="2">
                            <span>处理结果</span>
                        </td>
                        <td>
                            <span>录入时间</span>
                        </td>
                        <td>
                            <span>录入人员工号</span>
                        </td>
                        <td>
                            <span>录入人员姓名</span>
                        </td>
                    </tr>
                    <tr class="blankTr">
                        <td colspan="2" rowspan="4" style="text-align:center;">
                            <el-radio-group v-model="addForm.OKNG" @input="radioClick">
                                <el-radio label="NG">批评</el-radio>
                                <el-radio label="OK">表扬</el-radio>
                            </el-radio-group>
                            
                        </td>
                        <td colspan="2" rowspan="4">
                            &emsp;<el-radio v-model="addForm.dealResult" :disabled="dealDisabled" label="技能重新培训">技能重新培训</el-radio><br><br>
                            &emsp;<el-radio v-model="addForm.dealResult" :disabled="dealDisabled" label="书面面谈，重新培训">书面面谈，重新培训</el-radio><br><br>
                            &emsp;<el-radio v-model="addForm.dealResult" :disabled="dealDisabled" label="换岗">换岗</el-radio><br><br>
                            &emsp;<el-radio v-model="addForm.dealResult" :disabled="dealDisabled" label="劝退">劝退</el-radio>
                        </td>
                        <td>
                            <el-date-picker 
                                style="width:180px;"
                                size="medium"
                                v-model="addForm.checkDate"
                                value-format="yyyy-MM-dd"
                                placeholder="选择日期"
                            ></el-date-picker>
                        </td>
                        <td>
                            <el-input v-model="addForm.checkEmployeeId" placeholder="请输入工号" @change="employeeIdChange"></el-input>
                        </td >
                        <td ref="checkName" style="text-align:center;">
                            <!-- <el-input v-model="addForm.checkName" ></el-input> -->
                        </td>
                    </tr>
                    <tr class="blankTr">
                        <td rowspan="3" style="text-align:center;">一</td>
                        <td rowspan="3" style="text-align:center;">一</td>
                        <td rowspan="3" style="text-align:center;">一</td>
                    </tr>
                    <tr class="blankTr">
                    </tr>
                    <tr class="blankTr">
                        
                    </tr>
                </table>
            </div>
            <span slot="footer" class="dialog-footer">
                <el-button type="primary" @click="infoSaveClick" v-show="saveShow">保存</el-button>
                <el-button type="primary" @click="infoUpClick" v-show="upShow">修改</el-button>
                <el-button type="primary" @click="addDialogVisible = false">取消</el-button>
            </span>
        </el-dialog>
        
    </div>
</template>

<script>
    import qs from 'qs'
    import {downFile, getDateFormat} from '@/libs/helper.js'
    export default {
        name: 'QualityMain',
        data: function(){
            return{
                addDialogVisible: false,
                saveShow: true,
                upShow: false,
                addForm:{
                    employeeId: '',
                    eventDate: '',
                    eventDesc: '',
                    checkDate: '',
                    checkEmployeeId: '',
                    eventType:'',
                    OKNG: 'NG',
                    dealResult: '',
                    employeeIdB: '',
                    eventTypeB: '',
                    checkDateB: ''
                },
                departmentOption:[
                    {label: '切割',value: '切割'},
                    {label: '镀膜',value: '镀膜'},
                    {label: '检测',value: '检测'},
                    {label: '出货',value: '出货'},
                    {label: 'ABC外观', value: 'ABC外观'},
                    {label: '自动化摆片',value: '自动化摆片'},
                    {label: '清洗', value: '清洗'},
                    {label: '冷加工',value: '冷加工'},
                    {label: 'Z-block',value: 'Z-block'},
                    {label: 'CWDM',value: 'CWDM'},
                    {label: '生化',value: '生化'},
                ],
                postSkill: '',
                dealDisabled: false,
                infoList:[],
                queryParam:{
                    pageNum: 1,
                    pagesize: 10,
                    department: ''
                },
                infoTotal: 0
            }
        },
        created: function(){
            this.addForm.eventDate = getDateFormat(new Date())
            this.addForm.checkDate = getDateFormat(new Date())

            this.getInfoList()
        },
        methods:{
            employeeIdChange: function(employeeId){
                this.$http.get('EmployeeInfoMain/GetInfoByID', {
                    params:{
                        employeeId: employeeId
                    }
                }).then(res => {
                    const result = res.data.Data
                    console.log("result")
                    console.log(result)
                    if(result.cname != null){
                        //this.addForm.checkName = result.cname
                        this.$refs.checkName.innerHTML = result.cname
                    }else{
                        this.$message({
                            showClose: true,
                            message: '查无此人！',
                            type: 'warning',
                            duration: 2000,
                        })
                    }
                    
                })
            },
            infoUpClick: function(){
                this.$http.post('QualityMain/InfoUp', this.$qs.stringify(this.addForm)).then(res => {
                    const result = res.data
                    if(result.code == "200"){
                        this.$message({
                            showClose: true,
                            message: result.message,
                            type: 'success',
                            duration: 2000,
                        })
                        
                        this.addDialogVisible = false
                        this.getInfoList()
                        
                        
                    }else{
                        this.$message({
                            showClose: true,
                            message: result.message,
                            type: 'error',
                            duration: 2000,
                        })
                    }
                })
            },
            updateInfo: function(e){
                this.addForm.employeeId = e.employeeId
                this.addForm.eventDate = e.eventDate
                this.addForm.eventDesc = e.eventDesc
                this.addForm.checkDate = e.checkDate
                this.addForm.checkName = e.checkName
                this.addForm.checkEmployeeId = e.checkEmployeeId
                this.addForm.eventType = e.eventType
                this.addForm.employeeIdB = e.employeeId
                this.addForm.checkDateB = e.checkDate
                this.addForm.eventTypeB = e.eventType
                this.addForm.dealResult = e.dealResult
                this.saveShow = false
                this.upShow = true
                this.addDialogVisible = true
                if(e.dealType == "表扬"){
                    this.addForm.OKNG = "OK"
                    this.dealDisabled = true
                }else{
                    this.addForm.OKNG = "NG"
                    this.dealDisabled = false
                }
                
            },
            delInfo: function(e){
                this.$confirm('确定删除?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(res =>{
                    this.$http.get('QualityMain/InfoDel',{
                        params:{
                            employeeId: e.employeeId,
                            eventType: e.eventType,
                            checkDate: e.checkDate
                        }
                    }).then(res => {
                        const result = res.data
                        if(result.code == "200"){
                            this.getInfoList()
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

            },
            SizeChange: function(newSize){
                
            },
            getInfoList: function(){
                this.$http.get('QualityMain/GetInfoList',{
                    params: this.queryParam
                }).then(res => {
                    const result = res.data
                    this.infoList = result.rows
                    this.infoTotal = result.total
                })
            },
            infoSaveClick: function(){
                if(this.checkInput()){
                    this.$http.post('QualityMain/QualityInfoSave', this.$qs.stringify(this.addForm)).then(res => {
                        const result = res.data
                        if(result.code == "200"){
                            this.$message({
                                showClose: true,
                                message: '保存成功',
                                type: 'success',
                                duration: 2000,
                            })
                            this.addDialogVisible = false
                            this.getInfoList()
                        }else{
                            this.$message({
                                showClose: true,
                                message: result.message,
                                type: 'error',
                                duration: 2000,
                            })
                        }
                    })
                }
            },
            AddInfoBtn: function(){
                this.addForm.employeeId = ''
                this.addForm.eventDate = getDateFormat(new Date())
                this.addForm.eventDesc = ''
                this.addForm.checkDate = getDateFormat(new Date())
                this.addForm.innerHTML = ''
                this.addForm.checkEmployeeId = ''
                this.addForm.eventType = ''
                this.addForm.dealResult = ''
                this.saveShow = true
                this.upShow = false
                this.addDialogVisible = true
            },
            SearchBtn: function(){
                this.queryParam.pageNum = 1
                this.getInfoList()
            },
            exportExcel: function(){
                this.$http.get('QualityMain/ExportExcel',{
                    params: this.queryParam,
                    responseType: 'blob',
                    headers:{
                        'Content-Type': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                    }
                }).then(res => {
                    const resultData = res.data
                    const fileName = "员工品质信息" + ".xlsx"
                    downFile(resultData, fileName)
                }).catch(err => {
                    this.$message({
                        showClose: true,
                        message: '网络未连接',
                        type: 'error',
                        duration: 2000,
                    })
                })
            },
            checkInput: function(){
                var result = true
                console.log("eventType:" + this.addForm.eventType)
                console.log("OKNG:" + this.addForm.OKNG)
                console.log("dealResult:" + this.addForm.dealResult)
                console.log("checkName:" + this.$refs.checkName.innerHTML)
                if(this.addForm.employeeId == "" || this.$refs.cname.innerHTML == "" || this.addForm.eventDesc == "" ||
                    this.addForm.eventType == "" || (this.addForm.OKNG == "NG" && this.addForm.dealResult == "") ||
                    this.$refs.checkName.innerHTML == "" || this.addForm.checkEmployeeId == ""){
                    result = false
                    this.$message({
                        showClose: true,
                        message: "请把信息填写完整！",
                        type:"warning",
                        duration: 2000
                    })    
                }
                return result
            },
            radioClick: function(value){
                if(value == "OK"){
                    this.addForm.dealResult =  ''
                    this.dealDisabled = true
                }else{
                    this.dealDisabled = false
                }
            },
            employeeKeyup: function(e){
                this.$http.get('QualityMain/GetInfoById',{
                    params:{
                        employeeId: this.addForm.employeeId
                    }
                }).then(res => {
                    const result = res.data
                    if(result.code == 200){
                        this.$refs.cname.innerHTML = result.Data.cname
                        this.$refs.department.innerHTML = result.Data.department
                        this.$refs.indate.innerHTML = result.Data.indate
                        this.$refs.grade.innerHTML = result.Data.grade
                        this.$refs.postSkill.innerHTML = result.Data.postSkill
                        this.$refs.postNow.innerHTML = result.Data.postNow
                        this.postSkill = result.Data.postSkill
                    }else{
                        this.$message({
                            showClose: true,
                            message: result.message,
                            type: 'error',
                            duration: 2000,
                        })
                    }
                })
            }
        }
    }
</script>

<style type="text/css" scoped>
    .el-breadcrumb__inner{
        font-weight: bold !important;
        font-size:15px !important;
    }
    .el-breadcrumb{
        margin-top:15px !important;
        margin-left:10px !important;
    }
    .tableDiv{
        margin: 0 auto;
        width:100%;
        font-size:16px;
        margin-right:10px;
    }
    .tableDiv span{
        font-size:16px;
    }
    .tableDiv table{
        border-collapse: collapse;
    }
    .blankTr{
        height:50px; 
    }
    .blankCenter{
        text-align: center;
    }
    .tableDiv table tr td{
        width:200px;
        border: 1px solid #D3D3D3;
    }
    .tableDiv table tr:nth-child(1) {
        background-color:cornflowerblue;
        text-align: center;
        height:40px;
        color:white;
        font-weight: bold;
    }
    .tableDiv table tr:nth-child(3) {
        background-color:cornflowerblue;
        text-align: center;
        height:40px;
        color:white;
        font-weight: bold;
    }
    .tableDiv table tr:nth-child(8) {
        background-color:cornflowerblue;
        text-align: center;
        height:40px;
        color:white;
        font-weight: bold;
    }
    .el-input__inner{
        height: 36px !important;
        line-height: 36px !important;
        width:198px;
    }
</style>
