<template>
    <div>
        <!-- <el-breadcrumb separator="/">
            <el-breadcrumb-item>车间管理</el-breadcrumb-item>
            <el-breadcrumb-item>车间人员看板</el-breadcrumb-item>
        </el-breadcrumb> -->

        <el-card style="margin-left:10px;margin-right:10px;margin-top:10px;">
            <div style="width:100%;float:left;margin-left:10px;font-size:18px;">
                车间:
                <el-select v-model="queryParam.department" placeholder="请选择" >
                    <el-option
                        v-for="item in departmentOption"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value"
                    >
                    </el-option>
                </el-select>&emsp;
                日期:
                <el-date-picker 
                    style="width:180px;"
                    size="medium"
                    v-model="queryParam.date"
                    value-format="yyyy-MM-dd"
                    placeholder="选择日期"
                ></el-date-picker>&emsp;
                班别:&nbsp;
                <el-radio v-model="queryParam.shift" label="D">白班</el-radio>
                <el-radio v-model="queryParam.shift" label="N">晚班</el-radio>&emsp;
                <el-button type="primary" @click="SearchBtn" size="medium" icon="el-icon-search">查询</el-button>&emsp;
                <!-- <el-button type="primary" @click="exportExcel" size="medium" icon="el-icon-download">导出</el-button> -->
            </div>
            <div style="width:100%;margin-top:70px;margin-bottom:10px;font-size:15px;">
                <span class="greenCircleS"></span>&nbsp;有当前岗位技能+一周内无品质异常&emsp;
                <span class="yellowCircleS"></span>&nbsp;a.有当前岗位技能+一周内发生1笔品质异常；  b.有当前岗位技能未经过培训部考核人员;&emsp;
                <span class="redCicleS"></span>&nbsp;a.入职≤7天内培训期新人；b.有当前岗位技能+一周内发生≥2笔品质异常
            </div>
            <div class="tableDiv"> 
                <table>
                    <thead>
                        <tr>
                            <th>岗位</th>
                            <th>姓名</th>
                            <th>等级</th>
                            <th style="200px;">上岗技能</th>
                            <th>灯牌</th>
                        </tr>
                    </thead>
                    <tbody>   
                        <template v-for="(item) in kanBanInfo" >
                            <tr v-for="(person, i) in item.kanbanPerByPost" >
                                <template v-if="i == 0">
                                    <td :rowspan="GetRowspan(item.kanbanPerByPost.length)">{{item.post}}</td>
                                    <td @click="clickName(person.cname, item.post)">{{person.cname}}</td>
                                    <td>{{person.grade}}</td>
                                    <td>{{person.postSkill}}</td>
                                    <td><span :class="GetPostColor(person.positionColor)"></span></td>
                                </template>
                                <template v-else>
                                    <td @click="clickName(person.cname)">{{person.cname}}</td>
                                    <td>{{person.grade}}</td>
                                    <td>{{person.postSkill}}</td>
                                    <td><span :class="GetPostColor(person.positionColor)"></span></td>
                                </template> 
                            </tr>
                        </template>
                    </tbody>
                </table>
                
            </div>
        </el-card>
        <el-dialog
            :visible.sync="DialogVisible"
            @close="DialogVisible=false"
            title="员工基本信息"
            width="30%"
            class="abow_dialog"
            :close-on-click-modal="false"
        >
            <div class="dialogDiv">
                
                <el-form style="margin-left:50px;" label-width="25%" ref="infoUpForm" v-model="infoUpForm" label-position="left">
                    <el-form-item label="姓名:" prop="cname" style="font-size:18px;">
                        <span  ref="dialogCname" id="dialogCname" style="font-size:18px;font-weight:bold;"></span>
                    </el-form-item>
                    <el-form-item label="工号:" prop="employeeId">
                        <span ref="employeeId" id="employeeId" style="font-size:18px;font-weight:bold;"></span>
                    </el-form-item>
                    <el-form-item label="车间:" prop="employeeId">
                        <span ref="dept" id="dept" style="font-size:18px;font-weight:bold;"></span>
                    </el-form-item>
                    <el-form-item label="入职日期:" prop="dateUp">
                        <span ref="indate" id="indate" style="font-size:18px;font-weight:bold;"></span>
                    </el-form-item>
                    <el-form-item label="等级:" prop="grade" class="upFormItemNum">
                        <span ref="grade" id="grade" style="font-size:18px;font-weight:bold;"></span>
                    </el-form-item>
                    <el-form-item label="上岗技能:" prop="grade">
                        <span ref="postSkill" id="postSkill" style="font-size:18px;font-weight:bold;"></span>
                    </el-form-item>
                    <el-form-item label="当前岗位:" prop="grade">
                        <span ref="postNow" id="postNow" style="font-size:18px;font-weight:bold;"></span>
                    </el-form-item>
                </el-form>
            </div>
            
            <div slot="footer" class="dialog-footer">
                <el-button type="primary" @click="DialogVisible = false">关闭</el-button>
            </div>
        </el-dialog>
    </div>
    
</template>

<script>
    import qs from 'qs'
    import {downFile, getDateFormat} from '@/libs/helper.js'

    export default{
        name: 'Kanban',
        data: function(){
            return{
                infoUpForm:{

                },
                DialogVisible: false,
                kanBanInfo:[],
                queryParam:{
                    date: '',
                    department: '切割',
                    shift: 'D',
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
            }
        },
        created: function(){
            this.queryParam.date = getDateFormat(new Date())
        },
        mounted: function(){
            this.GetKanbanInfo()
        },
        methods:{
            clickName: function(name, post){
                this.DialogVisible = true
                this.$http.get('Kanban/GetEmployeeInfo',{
                    params: {
                        name: name,
                        post: post
                    }
                }).then(res => {
                    const resultData = res.data.Data
                    this.$refs.dialogCname.innerHTML = resultData.cname
                    this.$refs.employeeId.innerHTML = resultData.employeeId
                    this.$refs.dept.innerHTML = resultData.department
                    this.$refs.indate.innerHTML = resultData.indate
                    this.$refs.grade.innerHTML = resultData.grade
                    this.$refs.postSkill.innerHTML = resultData.postSkill
                    this.$refs.postNow.innerHTML = resultData.postNow
                })
                
            },
            SearchBtn: function(){
                this.GetKanbanInfo()
            },
            exportExcel: function(){

            },
            GetRowspan: function(rowspan){
                return rowspan
            },
            GetKanbanInfo: function(){
                this.$http.get('Kanban/GetKanbanInfo',{
                    params: this.queryParam
                }).then(res => {
                    const resultData = res.data
                    this.kanBanInfo = resultData.Data
                    console.log("this.kanBanInfo")
                    console.log(this.kanBanInfo)
                })
            },
            GetPostColor: function(color){
                if(color == "red"){
                    return "redCicle"
                }else if(color == "green"){
                    return "greenCircle"
                }else if(color == "yellow"){
                    return "yellowCircle"
                }
            }
        }
    }
</script>


<style type="text/css" scoped>
    .el-breadcrumb__inner{
        font-weight: bold !important;
        font-size:15px !important;
    }
    .el-breadcrumb__item:last-child .el-breadcrumb__inner{
        font-weight: bold !important;
        font-size:15px !important;
    }
    .el-breadcrumb{
        margin-top:15px !important;
        margin-left:10px !important;
    }
    .el-select{
        width:180px !important;
    }
    .tableDiv{
        width:100%;
        margin-top:20px;
        margin-bottom:30px;
    }
    .tableDiv table{
        border-collapse: collapse;
    }
    .tableDiv table thead tr{
        background-color: cornflowerblue;
    }
    .tableDiv table thead tr th{
        border: 1px solid #D3D3D3;
        color:white;
        font-size: 20px;
        height:60px;
        width:260px;
    }
    .tableDiv table tbody tr td{
        font-size: 18px;
        text-align: center;
        height:60px;
        width:260px;
        border: 1px solid #D3D3D3;
    }
    .redCicle{
        display: inline-block;
        width:30px;
        height:30px;
        border-radius: 50%;
        background-color: red;
    }
    .greenCircle{
        display: inline-block;
        width:30px;
        height:30px;
        border-radius: 50%;
        background-color: green;
    }
    .yellowCircle{
        display: inline-block;
        width:30px;
        height:30px;
        border-radius: 50%;
        background-color: yellow;
    }
    .redCicleS{
        display: inline-block;
        width:15px;
        height:15px;
        border-radius: 50%;
        background-color: red;
    }
    .greenCircleS{
        display: inline-block;
        width:15px;
        height:15px;
        border-radius: 50%;
        background-color: green;
    }
    .yellowCircleS{
        display: inline-block;
        width:15px;
        height:15px;
        border-radius: 50%;
        background-color: yellow;
    }
    .el-form--label-left .el-form-item__label{
        font-size: 18px !important;
    }
</style>