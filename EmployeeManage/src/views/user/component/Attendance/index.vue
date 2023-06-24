<template>
    <div class="attendanceCss">
        <!-- <el-breadcrumb separator="/">
            <el-breadcrumb-item>车间管理</el-breadcrumb-item>
            <el-breadcrumb-item>考勤管理</el-breadcrumb-item>
        </el-breadcrumb> -->

        <el-card style="margin-left:10px;margin-right:10px;height:100%;font-size:18px;">
            <div style="width:100%;float:left;margin-left:10px;height:100%;">
                车间:
                <el-select v-model="queryParam.department" placeholder="请选择车间" clearable>
                    <el-option
                            v-for="item in departmentOption"
                            :key="item.value"
                            :label="item.label"
                            :value="item.value"
                        >
                        </el-option>
                </el-select>&nbsp;
                日期:
                <el-date-picker 
                    style="width:180px;"
                    size="medium"
                    v-model="queryParam.date"
                    value-format="yyyy-MM-dd"
                    placeholder="日期"
                ></el-date-picker>&nbsp;
                班别:&nbsp;
                <el-radio v-model="queryParam.shift" label="D">白班</el-radio>
                <el-radio v-model="queryParam.shift" label="N">晚班</el-radio>&emsp;
                <el-button-group>
                    <el-button type="primary" @click="SearchBtn" size="medium" icon="el-icon-search">查询</el-button>
                    <el-button type="primary" @click="exportExcel" size="medium" icon="el-icon-download">导出</el-button>
                    <el-button type="primary" @click="overTimeBtn" size="medium" >加班提报</el-button>
                </el-button-group>
                
                <!-- <el-button type="primary" @click="exportExcel" size="medium" icon="el-icon-download">导出</el-button><br><br> -->
                <el-table :data="infoList" border style="width: 100%;margin-bottom:10px;margin-top:10px;height:calc(100%-200px)" height="720" stripe :header-cell-style="{background:'#f5f7fa'}" >
                    <el-table-column type="index" fixed label="序号" prop="index" align="center" width="80px">
                        <template slot-scope="scope">
                            <span>{{ (queryParam.pageNum - 1) * queryParam.pagesize + scope.$index + 1}}</span>
                            <!-- (page - 1) * this.pageSize +  -->
                        </template>
                    </el-table-column>
                    <el-table-column label="姓名" align="center" width="120px" prop="cname" ></el-table-column>
                    <el-table-column label="工号" align="center" width="120px" prop="employeeId" ></el-table-column>
                    <el-table-column label="车间" align="center" width="120px" prop="department" ></el-table-column>
                    <el-table-column label="出勤状况" align="center" width="200px" prop="attendState" >
                        <template slot-scope="scope">
                            <el-select size="mini"   v-model="scope.row.attendState" style="width:130px;height:40px !important;line-height:40px !important;" clearable
                                placeholder="请选择">
                                <el-option v-for="item in attendStatusOp" :key="item.supplierId" :label="item.value" :value="item.label">
                                </el-option>
                            </el-select> 
                        </template>
                    </el-table-column>
                    <el-table-column label="出勤时间" align="center" width="230px" prop="attendTime" >
                        <template slot-scope="scope">
                            <el-time-picker
                                is-range
                                v-model="scope.row.attendTime"
                                range-separator="至"
                                start-placeholder="开始时间"
                                end-placeholder="结束时间"
                                placeholder="选择时间范围"
                                format="HH:mm">
                            </el-time-picker>
                        </template>
                    </el-table-column>
                    <el-table-column label="借出车间" align="center" width="200px" prop="borrowDept" >
                        <template slot-scope="scope">
                            <el-select size="mini"   v-model="scope.row.borrowDept" clearable
                                placeholder="请选择">
                                <el-option v-for="item in departmentOption" :key="item.value" :label="item.label" :value="item.value">
                                </el-option>
                            </el-select>
                        </template>
                    </el-table-column>
                    <!-- <el-table-column label="加班结束时间" align="center" width="200px" prop="overTime" >
                        <template slot-scope="scope">
                            <!-- <el-time-picker
                                is-range
                                v-model="scope.row.overTime"
                                range-separator="至"
                                start-placeholder="开始时间"
                                end-placeholder="结束时间"
                                placeholder="选择时间范围"
                                @change="overTimeChange(scope.row.overTime, scope.row)"
                                >
                            </el-time-picker> -->
                            <!-- <el-time-select
                                v-model="scope.row.overTime"
                                :picker-options="overTimeOption"
                                placeholder="选择时间"
                                @change="overTimeChange(scope.row.overTime, scope.row)">
                            </el-time-select>
                        </template>
                    </el-table-column>
                    <el-table-column label="加班时长(h)" align="center" width="120px" prop="overTimeCal" ></el-table-column> --> 
                    <el-table-column label="备注" align="center"  prop="tips" width="200px">
                        <template slot-scope="scope">
                            <el-input v-model="scope.row.tips" ></el-input>
                        </template>
                    </el-table-column>
                    <el-table-column label="维护人" align="center"  prop="lmName" width="120px"></el-table-column>
                    <el-table-column label="维护人工号" align="center"  prop="lmEmployeeId" width="120px"></el-table-column>
                    <el-table-column label="是否维护" align="center" width="100px" prop="overTime" fixed="right">
                        <template slot-scope="scope">
                            <el-tag plain size="medium" :type="scope.row.lmstate=='Y'?'success':'warning'">
                                {{scope.row.lmstate=='Y'?'已维护':'未维护'}}
                            </el-tag>
                        </template>
                    </el-table-column>
                    <el-table-column label="操作" align="center" width="120px" header-align="center" fixed="right" >
                        <template slot-scope="scope">
                            <!-- 删除按钮 -->
                            <el-tooltip
                                :enterable="false"
                                class="item"
                                content="保存"
                                effect="dark"
                                placement="top"
                            >
                                <el-button
                                    @click="SaveRow(scope.row)"
                                    circle
                                    icon="el-icon-document-checked"
                                    size="mini"
                                    type="primary"
                                ></el-button>
                            </el-tooltip>
                            <!-- 修改按钮
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
                            </el-tooltip> -->
                            
                            
                        </template>
                    </el-table-column>
                </el-table>
                
                <div class="paginationCss">
                    <el-pagination
                            :current-page="queryParam.pageNum"
                            :page-size="queryParam.pagesize"
                            :page-sizes="[25,50]"
                            :total="infoTotal"
                            @current-change="PageChange"
                            @size-change="SizeChange"
                            background
                            layout="prev, pager, next, jumper, sizes, total"
                            style="margin-bottom:30px;"
                    ></el-pagination>
                </div>
                
                
            </div>
        </el-card>
        <!-- <el-dialog
            :visible.sync="loginDialogVisible"
            title="登录"
            width="30%"
            center
            class="abow_dialog"
            :close-on-click-modal="false"
        >
            <el-form style="margin-left:20px;" :inline="true" label-width="20px;" ref="loginForm" v-model="loginForm" label-position="left">
                <el-form-item label="密码" prop="pass" style="width:250px;">
                    <el-input type="password" style="width:200px;" v-model="loginForm.password" autocomplete="off"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="loginSubmit">确认</el-button>
                </el-form-item>
            </el-form>
        </el-dialog> -->

        <el-dialog
            :visible.sync="overTimeDialogShow"
            @close="overTimeDialogShow=false"
            title="加班维护"
            width="80%"
            :close-on-click-modal="false"
        >
            <el-row>
                <el-col :span="24">
                    车间:
                    <el-select v-model="overTimeParam.department" placeholder="请选择车间" clearable>
                        <el-option
                                v-for="item in departmentOption"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            >
                            </el-option>
                    </el-select>&nbsp;
                    日期:
                    <el-date-picker 
                        style="width:180px;"
                        size="medium"
                        v-model="overTimeParam.date"
                        value-format="yyyy-MM-dd"
                        placeholder="日期"
                    ></el-date-picker>&nbsp;
                    班别:&nbsp;
                    <el-radio v-model="overTimeParam.shift" label="D">白班</el-radio>
                    <el-radio v-model="overTimeParam.shift" label="N">晚班</el-radio>&emsp;
                    <el-button-group>
                        <el-button type="primary" @click="OverSearchBtn" size="medium" icon="el-icon-search">查询</el-button>
                    </el-button-group>
                </el-col>
            </el-row>
            
            <el-table :data="overTimeInfoList" border stripe style="width:100%;margin-top:20px;margin-bottom:10px;" height="500" :header-cell-style="{background:'#f5f7fa'}">
                <el-table-column type="index" fixed label="序号" prop="index" align="center" width="150px">
                    <template slot-scope="scope">
                        <span>{{ (overTimeParam.overTimePage - 1) * overTimeParam.overTimePageNum + scope.$index + 1}}</span>
                        <!-- (page - 1) * this.pageSize +  -->
                    </template>
                </el-table-column>
                <el-table-column label="姓名" align="center" width="130px" prop="cname" ></el-table-column>
                <el-table-column label="工号" align="center" width="130px" prop="employeeId" ></el-table-column>
                <el-table-column label="车间" align="center" width="130px" prop="department" ></el-table-column>
                <el-table-column label="加班结束时间" align="center" width="220px" prop="overTime" >
                    <template slot-scope="scope">
                        <!-- <el-time-picker
                            is-range
                            v-model="scope.row.overTime"
                            range-separator="至"
                            start-placeholder="开始时间"
                            end-placeholder="结束时间"
                            placeholder="选择时间范围"
                            @change="overTimeChange(scope.row.overTime, scope.row)"
                            >
                        </el-time-picker> -->
                        <el-time-select
                            v-model="scope.row.overTime"
                            :picker-options="overTimeOption"
                            placeholder="选择时间"
                            @change="overTimeChange(scope.row.overTime, scope.row)">
                        </el-time-select>
                    </template>
                </el-table-column>
                <el-table-column label="加班时长(h)" align="center" width="120px" prop="overTimeCal" ></el-table-column>
                <el-table-column label="备注" align="center"  prop="overTips" width="200px">
                    <template slot-scope="scope">
                        <el-input v-model="scope.row.overTips" ></el-input>
                    </template>
                </el-table-column>
                <el-table-column label="维护人" align="center"  prop="overLmName" width="120px"></el-table-column>
                <el-table-column label="维护人工号" align="center"  prop="overLmEmployeeId" width="130px"></el-table-column>
                <el-table-column label="操作" align="center" width="100px"  fixed="right" >
                    <template slot-scope="scope">
                        <!-- 删除按钮 -->
                        <el-tooltip
                            :enterable="false"
                            class="item"
                            content="保存"
                            effect="dark"
                            placement="top"
                        >
                            <el-button
                                @click="SaveOverRow(scope.row)"
                                circle
                                icon="el-icon-document-checked"
                                size="mini"
                                type="primary"
                            ></el-button>
                        </el-tooltip>
                    </template>
                </el-table-column>
            </el-table>

            <div class="paginationCss">
                <el-pagination
                    :current-page="overTimeParam.overTimePage"
                    :page-size="overTimeParam.overTimePageNum"
                    :page-sizes="[25,50]"
                    :total="overTimeTotal"
                    @current-change="ovetTimePageChange"
                    @size-change="ovetTimeSizeChange"
                    background
                    layout="prev, pager, next, jumper, sizes, total"
                ></el-pagination>
            </div>

            <div slot="footer">
                <el-button type="primary"  @click="overTimeDialogShow = false">取 消</el-button>
            </div>
        </el-dialog>
    </div>
</template>
    
<script>
    import qs from 'qs'
    import {downFile, getDateFormat} from '@/libs/helper.js'
    export default{
        name: 'Attendance',
        data: function(){
            return{
                overTimeDialogShow:false,
                infoList: [],
                overTimeInfoList: [],
                infoTotal: 0,
                overTimeTotal: 0,
                loginFlag: false,
                overTimeOption:{
                    start: '17:00',
                    step: '00:30',
                    end: '23:00'
                },
                deptOp:[],
                //loginDialogVisible: false,
                loginForm:{
                    password: '',
                },
                queryParam:{
                    date: '',
                    department: '',
                    shift: 'D',
                    pageNum: 1,
                    pagesize: 25
                },
                attendStatusOp:[
                    {value: '正常',label: '正常'},
                    {value: '请假',label: '请假'},
                    {value: '旷工',label: '旷工'},
                    {value: '离职',label: '离职'},
                    {value: '借出',label: '借出'},
                    {value: '借入',label: '借入'}
                ],
                departmentOption:[],
                overTimeParam:{
                    overTimePage: 1,
                    overTimePageNum: 25,
                    department: '',
                    date: '', 
                    shift: 'D'
                }
            }
        },
        created: function(){
            this.queryParam.date = getDateFormat(new Date())
            this.overTimeParam.date = getDateFormat(new Date())
            this.GetInfoList()
            
        },
        mounted(){
            this.getDeptOp()
        },
        methods:{
            SaveOverRow: function(row){
                let param = {
                        date: this.overTimeParam.date,
                        shift: this.overTimeParam.shift,
                        cname: row.cname,
                        employeeId: row.employeeId,
                        overEndTime: row.overTime,
                        overTimeHour: row.overTimeCal,
                        overTips: row.overTips,
                        department: row.department
                    }
                console.log("row.overTips")
                console.log(row.overTips)
                this.$http.post('Attendance/SaveOverTimeRow',this.$qs.stringify(param)).then(res => {
                    const result = res.data
                    if(result.code == 200){
                        this.GetOverTimeList()
                        this.$message({
                            showClose: true,
                            message: result.message,
                            type: 'success',
                            duration: 2000,
                        })
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
            delInfo: function(row){

            },
            overTimeBtn: function(){
                this.overTimeDialogShow = true
                this.GetOverTimeList()
            },
            getDeptOp: function(){
                this.$http.get('EmployeeInfoMain/GetDeptOption').then(res => {
                    const result = res.data
                    this.departmentOption = result.Data
                    //this.queryParam.department = this.departmentOption[0].value
                })
            },
            SaveRow: function(row){
                let attendStartTime = ''
                let attendEndTime = ''
                if(row.attendTime != null){
                    attendStartTime = this.getHMS(row.attendTime[0])
                    attendEndTime = this.getHMS(row.attendTime[1])
                }
                // let overStartTime = ''
                // let overEndTime = ''
                // if(row.overTime != null){
                //     overStartTime = this.getHMS(row.overTime[0])
                //     overEndTime = this.getHMS(row.overTime[1])
                // }
                let param = {
                        date: this.queryParam.date,
                        shift: this.queryParam.shift,
                        cname: row.cname,
                        employeeId: row.employeeId,
                        attendState: row.attendState,
                        attendStartTime: attendStartTime,
                        attendEndTime: attendEndTime,
                        borrowDept: row.borrowDept,
                        //overStartTime: row.overStartTime,
                        overEndTime: row.overTime,
                        overTimeHour: row.overTimeCal,
                        tips: row.tips,
                        department: row.department
                    }
                this.$http.post('Attendance/SaveAttendanceRow',this.$qs.stringify(param)).then(res => {
                    const result = res.data
                    if(result.code == 200){
                        this.GetInfoList()
                        this.$message({
                            showClose: true,
                            message: result.message,
                            type: 'success',
                            duration: 2000,
                        })
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
            SearchBtn: function(){
                // if(this.loginFlag == false){
                //     //this.loginDialogVisible = true
                // }else{
                //     this.queryParam.pageNum = 1
                //     this.GetInfoList()
                // }
                
                this.GetInfoList()
            },
            OverSearchBtn: function(){
                this.GetOverTimeList()
            },
            // loginSubmit: function(){
            //     if(this.loginForm.password == "wd.123"){
            //         this.loginFlag = true
            //         this.loginDialogVisible = false
            //         this.GetInfoList()
            //     }else{
            //         this.$message({
            //             showClose: true,
            //             message: '密码错误！',
            //             type: 'error',
            //             duration: 2000,
            //         })
            //     }
            // },
            overTimeChange: function(ele, row){
                if(ele == null){
                    row.overTimeCal = ''
                }else{
                    // let endTime = new Date(ele[1])
                    // let startHour = startTime.getHours()
                    // let endHour = endTime.getHours()
                    //console.log("加班时长:" + (endHour - startHour))
                    //row.overTimeCal = endHour - startHour
                    const overTimeHH = parseInt(ele.split(':')[0])
                    const overTimeMM = parseInt(ele.split(':')[1])
                    if(this.queryParam.shift == 'D'){
                            row.overTimeCal = overTimeMM / 60 + overTimeHH - (16 + 30 / 60)
                    }else{
                        row.overTimeCal = Math.round(overTimeMM / 60) + overTimeHH - (5)
                    }
                    
                    
                }
                
            },
            exportExcel: function(){
                this.$http.get('Attendance/ExportAttendanceInfo',{
                    params: this.queryParam,
                    responseType: 'blob',
                    headers:{
                        'Content-Type': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                    }
                }).then(res => {
                    const resultData = res.data
                    const fileName = "出勤信息" + ".xlsx"
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
            GetOverTimeList: function(){
                if(this.overTimeParam.shift == 'N'){
                    this.overTimeOption.start = '5:00'
                    this.overTimeOption.end = '11:00'
                }else{
                    this.overTimeOption.start = '17:00'
                    this.overTimeOption.end = '23:00'
                }
                
                this.$http.get('Attendance/GetOvertTimeInfo',{
                    params: this.overTimeParam
                }).then(res => {
                    const result = res.data
                    const rowsNew = []
                    result.rows.forEach((item, index)=>{
                        const obj = {
                            cname: item.cname,
                            employeeId: item.employeeId,
                            department: item.department,
                            overTime: item.overTime,
                            overTimeCal: item.overTimeHour,
                            lmstate: item.lmstate,
                            overLmName: item.overLmName,
                            overLmEmployeeId: item.overLmEmployeeId,
                            overTips: item.overTips
                        }
                        rowsNew.push(obj)
                    })
                    this.overTimeInfoList = rowsNew
                    this.overTimeTotal = result.total
                    console.log(this.overTimeInfoList)
                })
            },
            GetInfoList: function(){
                if(this.queryParam.shift == 'N'){
                    this.overTimeOption.start = '5:00'
                    this.overTimeOption.end = '11:00'
                }else{
                    this.overTimeOption.start = '17:00'
                    this.overTimeOption.end = '23:00'
                }
                this.$http.get('Attendance/GetAttendanceInfo',{
                    params: this.queryParam
                }).then(res => {
                    const result = res.data
                    const rowsNew = []
                    result.rows.forEach((item, index)=>{
                        let attendTimeArr = []
                        if(item.attendStartTime != null && item.attendStartTime != ""){
                            attendTimeArr.push(new Date(this.queryParam.date + " " + item.attendStartTime))
                            attendTimeArr.push(new Date(this.queryParam.date + " " + item.attendEndTime))
                        }else{
                            attendTimeArr = null
                        }
                        
                        let overTimeArr = []
                        if(item.overStartTime != null && item.overStartTime != ""){
                            overTimeArr.push(new Date(this.queryParam.date + " " + item.overStartTime))
                            overTimeArr.push(new Date(this.queryParam.date + " " + item.overEndTime))
                        }else{
                            overTimeArr = null
                        }
                        const obj = {
                            cname: item.cname,
                            employeeId: item.employeeId,
                            department: item.department,
                            attendState: item.attendState,
                            attendTime: attendTimeArr, 
                            borrowDept: item.borrowDept,
                            overTime: item.overTime,
                            overTimeCal: item.overTimeHour,
                            lmstate: item.lmstate,
                            lmName: item.lmName,
                            lmEmployeeId: item.lmEmployeeId,
                            tips: item.tips
                        }
                        rowsNew.push(obj)
                    })
                    this.infoList = rowsNew
                    this.infoTotal = result.total
                })
            },
            ovetTimePageChange: function(newPage){
                this.overTimeParam.overTimePage = newPage 
                this.GetOverTimeList()
            },
            ovetTimeSizeChange: function(newSize){
                this.overTimeParam.overTimePageNum = newSize
                this.GetOverTimeList()
            },
            PageChange: function(newPage){
                this.queryParam.pageNum =  newPage 
                this.GetInfoList()
            },
            SizeChange: function(newSize){
                this.queryParam.pagesize = newSize
                this.GetInfoList()
            },
            getHMS: function(time){
                let hour = time.getHours().toString().padStart(2, '0')
                let minute = time.getMinutes().toString().padStart(2, '0')
                let second = time.getSeconds().toString().padStart(2, '0')
                return hour + ":" + minute + ":" + second
            }

        },

    }
</script>
    
<style type="text/css" >
    /* .tableDiv{
        width:100%;
        height:720px;
        overflow-y: scroll;
    } */
    /* .tableOverDiv{
        height:500px;
        overflow-y: scroll;
    } */
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
    .attendanceCss .el-select{
        width:180px !important;
    }
    .attendanceCss .el-select .el-input__inner{
        width:180px !important;
        height:37px !important;
        line-height: 37px !important;
    }
    .attendanceCss .el-input--mini .el-input__inner{
        height:37px !important;
        line-height:37px !important;
    }
    .attendanceCss .el-input{
        width:180px !important;
    }
    .el-date-editor .el-range-input{
        width: 19% !important;
    }
    .el-date-editor--daterange.el-input, .el-date-editor--daterange.el-input__inner, .el-date-editor--timerange.el-input, .el-date-editor--timerange.el-input__inner{
        width:206px !important;
    } 
    .el-date-editor .el-range-input{
        width:50% !important;
    }
    .attendanceCss .el-pagination__editor.el-input{

        width:100px !important;
    }
    .attendanceCss .el-pagination .el-select .el-input{
        width:100px !important;
    }
    .attendanceCss .el-pagination .el-select .el-input .input__inner{
        width:100px !important;
    }
    .attendanceCss .paginationCss  .el-input--mini{
        width:100px !important;
    }
    .attendanceCss .paginationCss .el-select{
        width:100px !important;
    }
    .attendanceCss .paginationCss  .el-input--mini .el-input__inner{
        height: 28px !important;
        line-height: 28px !important;
        width:100px !important;
    }
    .el-pagination{
        margin-bottom:-10px;
    }
    .paginationCss{
        margin-top:20px;
    }

</style>