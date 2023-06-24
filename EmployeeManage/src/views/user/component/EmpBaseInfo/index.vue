<template>
    <div  class="empBaseInfoCss">
        <!-- <el-breadcrumb separator="/">
            <el-breadcrumb-item style="font-weight:bold;">车间管理</el-breadcrumb-item>
            <el-breadcrumb-item style="font-weight:bold;">员工信息维护</el-breadcrumb-item>
        </el-breadcrumb> -->

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
                        <el-button type="primary" @click="exportExcel" size="medium" icon="el-icon-download">导出</el-button>
                        <el-button type="primary" @click="AddInfoBtn" size="medium" icon="el-icon-plus">新增</el-button>
                        <el-button type="primary" @click="AddDeptBtn" size="medium" icon="el-icon-help">车间维护</el-button>
                        <el-button type="primary" @click="AddPostBtn" size="medium" icon="el-icon-place">岗位维护</el-button>
                        <el-button type="primary" @click="ShiftReverseBtn" size="medium" icon="el-icon-sort">翻班</el-button>
                    </el-button-group>
                </div>
                
                    
                <el-table :data="infoList" border style="width: 98%;margin-left:12px;margin-bottom:10px;" stripe :header-cell-style="{background:'#f5f7fa'}" >
                    <el-table-column type="index" fixed label="序号" prop="index" align="center" width="80px">
                        <template slot-scope="scope">
                            <span>{{ (queryParam.pageNum - 1) * queryParam.pagesize + scope.$index + 1}}</span>
                            <!-- (page - 1) * this.pageSize +  -->
                        </template>
                    </el-table-column>
                    <el-table-column label="姓名" align="center" width="150px" prop="cname" ></el-table-column>
                    <el-table-column label="工号" align="center" width="150px" prop="employeeId" ></el-table-column>
                    <el-table-column label="班别" align="center" width="150px" prop="shift" ></el-table-column>
                    <el-table-column label="车间" align="center" width="150px" prop="department" ></el-table-column>
                    <el-table-column label="入职日期" align="center" width="180px" prop="indate" ></el-table-column>
                    <el-table-column label="等级" align="center" width="150px" prop="grade" ></el-table-column>
                    <el-table-column label="上岗技能" align="center" width="220px" prop="postSkill" ></el-table-column>
                    <el-table-column label="当前岗位" align="center" width="150px" prop="postNow" ></el-table-column>
                    <el-table-column label="操作" align="center"  width="150px" header-align="center" fixed="right" >
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
            :visible.sync="addDialogVisible"
            @close="addDialogVisible=false"
            append-to-body
            title="新增"
            width="1100px"
            class="abow_dialog"
            modal
            close-on-click-modal
        >
            <el-form :inline="true" class="addForm" label-width="30%" ref="formEditRef" v-model="modelMainForm" label-position="left">
                <el-form-item label="姓名" prop="cname" class="addFormItem">
                    <el-input v-model="modelMainForm.cname" placeholder="请输入内容" ></el-input>
                </el-form-item>
                <el-form-item label="工号" prop="employeeId" class="addFormItem">
                    <el-input v-model="modelMainForm.employeeId" placeholder="请输入内容" ></el-input>
                </el-form-item>
                <el-form-item label="车间" prop="department" class="addFormItem">
                    <el-select v-model="modelMainForm.department" placeholder="请选择" style="width:180px !important;">
                        <el-option
                            v-for="item in departmentOption"
                            :key="item.value"
                            :label="item.label"
                            :value="item.value"
                        >
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="入职日期" prop="indate" class="addFormItem">
                    <el-date-picker 
                        style="width:180px;"
                        size="medium"
                        v-model="modelMainForm.indate"
                        value-format="yyyy-MM-dd"
                        placeholder="选择日期"
                    >
                    </el-date-picker>
                </el-form-item>
                <el-form-item label="等级" prop="grade" class="addFormItemNum">
                    <!-- LV&emsp;<el-input-number style="width:120px;" v-model="modelMainForm.grade" controls-position="right" :min="1" :max="10"></el-input-number> -->
                    <el-select v-model="modelMainForm.grade" placeholder="请选择" style="width:180px !important;">
                        <el-option
                            v-for="item in gradeOption"
                            :key="item.value"
                            :label="item.label"
                            :value="item.value"
                        >
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="上岗技能" prop="postSkill" class="addFormItem">
                    <el-select v-model="modelMainForm.postSkill" placeholder="请选择" multiple collapse-tags default-first-option>
                        <el-option
                            v-for="item in postOption"
                            :key="item.value"
                            :label="item.label"
                            :value="item.value"
                        >
                        </el-option>
                    </el-select>&emsp;
                </el-form-item>
                <el-form-item label="当前岗位" prop="postNow" class="addFormItem">
                    <el-select v-model="modelMainForm.postNow" placeholder="请选择" >
                        <el-option
                            v-for="item in postOption"
                            :key="item.value"
                            :label="item.label"
                            :value="item.value"
                        >
                        </el-option>
                    </el-select>&emsp;
                </el-form-item>
                <el-form-item label="班别" prop="postNow" class="addFormItem">
                    <el-select v-model="modelMainForm.shift" placeholder="请选择" >
                        <el-option
                            v-for="item in shiftOption"
                            :key="item.value"
                            :label="item.label"
                            :value="item.value"
                        >
                        </el-option>
                    </el-select>&emsp;
                </el-form-item>
            </el-form>

            <span slot="footer" class="dialog-footer">
                <el-button type="primary" @click="infoSaveClick">保存</el-button>
                <el-button type="primary" @click="addDialogVisible = false">取消</el-button>
            </span>
        </el-dialog>

        <el-dialog
            :visible.sync="upDialogVisible"
            @close="upDialogVisible=false"
            title="信息修改"
            width="35%"
            class="abow_dialog"
            :close-on-click-modal="false"
        >
            <div class="dialogDiv">
                <el-form style="margin-left:50px;" label-width="20%" ref="infoUpForm" v-model="infoUpForm" label-position="left">
                    <el-form-item label="姓名" prop="cname">
                        <el-input style="width:180px;" v-model="infoUpForm.cname" disabled placeholder="请输入内容"></el-input>
                    </el-form-item>
                    <el-form-item label="工号" prop="employeeId">
                        <el-input style="width:180px;" v-model="infoUpForm.employeeId" disabled placeholder="请输入内容"></el-input>
                    </el-form-item>
                    <el-form-item label="车间" prop="employeeId">
                        <el-select v-model="infoUpForm.department" placeholder="请选择" >
                            <el-option
                                v-for="item in departmentOption"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            >
                            </el-option>
                        </el-select>
                    </el-form-item>
                    <el-form-item label="入职日期" prop="dateUp">
                        <el-date-picker 
                            style="width:180px;"
                            size="small"
                            v-model="infoUpForm.indate"
                            value-format="yyyy-MM-dd"
                            placeholder="选择日期"
                        >
                        </el-date-picker>
                    </el-form-item>
                    <el-form-item label="等级" prop="grade" class="upFormItemNum">
                        <!-- <el-input-number 
                            style="width:120px;"
                            v-model="infoUpForm.grade" 
                            controls-position="right" 
                            :min="1" 
                            :max="10">
                        </el-input-number> -->
                        <el-select v-model="infoUpForm.grade" placeholder="请选择" >
                            <el-option
                                v-for="item in gradeOption"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            >
                            </el-option>
                        </el-select>
                    </el-form-item>
                    <el-form-item label="上岗技能" prop="grade">
                        <el-select v-model="infoUpForm.postSkill" placeholder="请选择" multiple collapse-tags default-first-option>
                            <el-option
                                v-for="item in postOption"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            >
                            </el-option>
                        </el-select>
                    </el-form-item>
                    <el-form-item label="当前岗位" prop="grade">
                        <el-select v-model="infoUpForm.postNow" placeholder="请选择" >
                            <el-option
                                v-for="item in postOption"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            >
                            </el-option>
                        </el-select>
                    </el-form-item>
                    <el-form-item label="班别" prop="grade">
                        <el-select v-model="infoUpForm.shift" placeholder="请选择" >
                            <el-option
                                v-for="item in shiftOption"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            >
                            </el-option>
                        </el-select>
                    </el-form-item>
                </el-form>
            </div>
            
            <div slot="footer" class="dialog-footer">
                <el-button type="primary" @click="upDialogVisible = false">取 消</el-button>
                <el-button type="primary" @click="infoUpSave">确 定</el-button>
            </div>
        </el-dialog>

        <el-dialog
            :visible.sync="postDialogVisible"
            @close="postDialogVisible=false"
            title="岗位维护"
            width="40%"
            class="abow_dialog"
            :close-on-click-modal="false"
        >
            <el-form style="margin-left:20px;" label-width="10%" ref="formEditRef" v-model="postForm" label-position="left">
                <el-form-item label="岗位" prop="modelUp">
                    <el-input v-model="postForm.post" placeholder="请输入内容" ></el-input>
                    &emsp;&emsp;
                    <el-button @click="postInfoSave" size="medium" type="primary" style="margin-left:80px;">保存</el-button>
                </el-form-item>
            </el-form>

            <el-table :data="postInfoList" border stripe style="width:555px;margin-left:2%;margin-top:20px;margin-bottom:10px;" :header-cell-style="{background:'#f5f7fa'}">
                <el-table-column type="index" fixed label="序号" prop="index" align="center" width="150px">
                    <template slot-scope="scope">
                        <span>{{ (postPagination.postPageNum - 1) * postPagination.postPagesize + scope.$index + 1}}</span>
                        <!-- (page - 1) * this.pageSize +  -->
                    </template>
                </el-table-column>
                <el-table-column label="岗位" align="center" header-align="center"  width="250px" prop="post"></el-table-column>
                <el-table-column label="操作" align="center" header-align="center" >
                    <template slot-scope="scope">
                        <el-tooltip
                            :enterable="false"
                            class="item"
                            content="删除"
                            effect="dark"
                            placement="top"
                        >
                            <el-button
                                @click="postDel(scope.row)"
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
                    :current-page="postPagination.postPageNum"
                    :page-size="postPagination.postPagesize"
                    :page-sizes="[5,10]"
                    :total="postTotal"
                    @current-change="postPageChange"
                    @size-change="postSizeChange"
                    background
                    layout="prev, pager, next, jumper, sizes, total"
            ></el-pagination>

            <div slot="footer" class="dialog-footer">
                <el-button type="primary"  @click="postDialogVisible = false">取 消</el-button>
            </div>
        </el-dialog>

        <el-dialog
            :visible.sync="deptDialogVisible"
            @close="deptDialogVisible=false"
            title="车间维护"
            width="40%"
            class="abow_dialog"
            :close-on-click-modal="false"
        >
            <el-form style="margin-left:20px;" label-width="10%" ref="formEditRef" v-model="deptForm" label-position="left">
                <el-form-item label="车间" prop="modelUp">
                    <el-input v-model="deptForm.dept" placeholder="请输入内容" ></el-input>
                    &emsp;&emsp;
                    <el-button @click="deptInfoSave" size="medium" type="primary" style="margin-left:80px;">保存</el-button>
                </el-form-item>
            </el-form>

            <el-table :data="deptInfoList" border stripe style="width:555px;margin-left:2%;margin-top:20px;margin-bottom:10px;" :header-cell-style="{background:'#f5f7fa'}">
                <el-table-column type="index" fixed label="序号" prop="index" align="center" width="150px">
                    <template slot-scope="scope">
                        <span>{{ (deptPagination.deptPageNum - 1) * deptPagination.deptPagesize + scope.$index + 1}}</span>
                        <!-- (page - 1) * this.pageSize +  -->
                    </template>
                </el-table-column>
                <el-table-column label="车间" align="center" header-align="center"  width="250px" prop="dept"></el-table-column>
                <el-table-column label="操作" align="center" header-align="center" >
                    <template slot-scope="scope">
                        <el-tooltip
                            :enterable="false"
                            class="item"
                            content="删除"
                            effect="dark"
                            placement="top"
                        >
                            <el-button
                                @click="deptDel(scope.row)"
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
                    :current-page="deptPagination.deptPageNum"
                    :page-size="deptPagination.deptPagesize"
                    :page-sizes="[5,10]"
                    :total="deptTotal"
                    @current-change="deptPageChange"
                    @size-change="deptSizeChange"
                    background
                    layout="prev, pager, next, jumper, sizes, total"
            ></el-pagination>

            <div slot="footer" class="dialog-footer">
                <el-button type="primary"  @click="deptDialogVisible = false">取 消</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
    import qs from 'qs'
    import {downFile, getDateFormat} from '@/libs/helper.js'
    export default{
        name: 'EmpBaseInfo',
        data: function(){
            return{
                postDialogVisible: false,
                infoList: [],
                deptInfoList: [],
                infoTotal: 0,
                deptTotal: 0,
                queryParam:{
                    pageNum: 1,
                    pagesize: 10,
                    employeeId: '',
                    department: ''
                },
                postInfoList:[],
                postTotal: 0,
                postPagination:{
                    postPageNum: 1,
                    postPagesize: 5
                },
                deptPagination:{
                    deptPageNum: 1,
                    deptPagesize: 5
                },
                postForm:{
                    post: ''
                },
                excelParam:{
                    employeeId: '',
                },
                addDialogVisible: false,
                upDialogVisible: false,
                deptDialogVisible: false,
                modelMainForm:{
                    cname: '',
                    employeeId: '',
                    indate: '',
                    grade: '培训期',
                    postSkill: '',
                    postNow: '',
                    department: '',
                    shift: 'D'
                },
                infoUpForm:{
                    cname: '',
                    employeeId: '',
                    department: '',
                    indate: '',
                    grade: 1,
                    postSkill: '',
                    postNow: '',
                    shift: ''
                },
                deptForm:{
                    dept: ''
                },
                shiftOption:[
                    {
                        label: 'D',
                        value: 'D'
                    },
                    {
                        label: 'N',
                        value: 'N'
                    }
                ],
                postOption: [
                ],
                gradeOption:[
                    {label: '培训期',value: '培训期'},
                    {label: '0',value: '0'},
                    {label: '1',value: '1'},
                    {label: '2',value: '2'},
                    {label: '3',value: '3'},
                    {label: '4',value: '4'},
                ],
                departmentOption:[]
            }
        },
        created: function(){
            var sevenDaysBefore = new Date(new Date() - 1000 * 60 * 60 * 24 * 7); //7天前
            var yesterday = new Date(new Date() - 1000 * 60 * 60 * 24);//前一天
            //this.dateBegin = getDateFormat(sevenDaysBefore)
            //this.dateEnd = getDateFormat(yesterday)
            this.modelMainForm.indate = getDateFormat(new Date())
            this.getDeptOp()
            this.getPostOp()
        },
        mounted(){
            this.getInfoList()
        },
        methods:{
            ShiftReverseBtn: function(){
                this.$http.post('EmployeeInfoMain/ReverseShift',this.$qs.stringify(this.queryParam)).then(res => {
                    const result = res.data
                    if(result.code == "200"){
                        this.$message({
                            showClose: true,
                            message: result.message,
                            type: 'success',
                            duration: 2000,
                        })
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
            postInfoSave: function(){
                if(this.postForm.post == ""){
                    this.$message({
                        showClose: true,
                        message: "输入项不能为空",
                        type:"warning",
                        duration: 2000
                    })
                }else{
                    this.$http.get('EmployeeInfoMain/PostInfoSave', {
                        params: this.postForm
                    }).then(res => {
                        const result = res.data
                        if(result.code == "200"){
                            this.$message({
                                showClose: true,
                                message: result.message,
                                type: 'success',
                                duration: 2000,
                            })
                            this.getPostList()
                            this.postForm.post = ''
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
            AddPostBtn: function(){
                this.postDialogVisible = true
                this.getPostList()
            },
            getDeptOp: function(){
                this.$http.get('EmployeeInfoMain/GetDeptOption').then(res => {
                    const result = res.data
                    this.departmentOption = result.Data
                    this.modelMainForm.department = this.departmentOption[0].value
                })
            },
            getPostOp: function(){
                this.$http.get('EmployeeInfoMain/GetPostOption').then(res => {
                    const result = res.data
                    this.postOption = result.Data
                    this.modelMainForm.postNow = this.postOption[0].value
                })
            },
            deptDel: function(e){
                this.$confirm('确定删除?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(res =>{
                    this.$http.get('EmployeeInfoMain/DeptListDel',{
                        params:{
                            dept: e.dept
                        }
                    }).then(res => {
                        const result = res.data
                        if(result.code == "200"){
                            this.getDeptList()
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
            postDel: function(e){
                this.$confirm('确定删除?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(res =>{
                    this.$http.get('EmployeeInfoMain/PostListDel',{
                        params:{
                            post: e.post
                        }
                    }).then(res => {
                        const result = res.data
                        if(result.code == "200"){
                            this.getPostList()
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
            deptInfoSave: function(){
                if(this.deptForm.dept == ""){
                    this.$message({
                        showClose: true,
                        message: "输入项不能为空",
                        type:"warning",
                        duration: 2000
                    })
                }else{
                    this.$http.get('EmployeeInfoMain/deptInfoSave', {
                        params: this.deptForm
                    }).then(res => {
                        const result = res.data
                        if(result.code == "200"){
                            this.$message({
                                showClose: true,
                                message: result.message,
                                type: 'success',
                                duration: 2000,
                            })
                            this.deptForm.dept = ''
                            this.getDeptList()
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
            exportExcel: function(){
                this.$http.get('/EmployeeInfoMain/ExportExcel',{
                    params: this.excelParam,
                    responseType: 'blob',
                    headers:{
                        'Content-Type': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                    }
                }).then(res => {
                    const resultData = res.data
                    const fileName = "员工基本信息" + ".xlsx"
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
            infoUpSave: function(){
                if (this.infoUpForm.postSkill) {
                    this.infoUpForm.postSkill = this.infoUpForm.postSkill.join(",");
                }
                this.$http.get('EmployeeInfoMain/InfoListUp', {
                    params: this.infoUpForm
                }).then(res => {
                    const result = res.data
                    if(result.code == "200"){
                        this.$message({
                            showClose: true,
                            message: result.message,
                            type: 'success',
                            duration: 2000,
                        })
                        this.getInfoList()
                        this.upDialogVisible = false
                    }else{
                        this.$message({
                            showClose: true,
                            message: result.message,
                            type: 'error',
                            duration: 2000,
                        })
                    }
                    this.infoUpForm.postSkill = this.infoUpForm.postSkill.split(",")
                })
            },
            updateInfo: function(e){
                this.upDialogVisible = true
                this.infoUpForm.cname = e.cname
                this.infoUpForm.employeeId = e.employeeId
                this.infoUpForm.department = e.department
                this.infoUpForm.indate = e.indate
                this.infoUpForm.grade = e.grade
                this.infoUpForm.postSkill = e.postSkill.split(',')
                this.infoUpForm.postNow = e.postNow
                this.infoUpForm.shift = e.shift
            },
            delInfo: function(e){
                this.$confirm('确定删除?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(res =>{
                    this.$http.get('EmployeeInfoMain/InfoListDel',{
                        params:{
                            employeeId: e.employeeId
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
            getInfoList: function(){
                this.$http.get('EmployeeInfoMain/GetInfoList',{
                    params:this.queryParam
                }).then(res => {
                    const result = res.data
                    this.infoList = result.rows
                    this.infoTotal = result.total
                })
            },
            getDeptList: function(){
                this.$http.get('EmployeeInfoMain/GetDeptList',{
                    params:this.deptPagination
                }).then(res => {
                    const result = res.data
                    this.deptInfoList = result.rows
                    this.deptTotal = result.total
                })
            },
            getPostList: function(){
                this.$http.get('EmployeeInfoMain/GetPostList',{
                    params:this.postPagination
                }).then(res => {
                    const result = res.data
                    this.postInfoList = result.rows
                    this.postTotal = result.total
                })
            },
            PageChange: function(newPage){
                this.queryParam.pageNum = newPage
                this.getInfoList()
            },
            SizeChange: function(newSize){
                this.queryParam.pagesize = newSize
                this.getInfoList()
            },
            deptPageChange: function(newPage){
                this.deptPagination.deptPageNum =  newPage
                this.getDeptList()
            },
            deptSizeChange: function(newSize){
                this.deptPagination.deptPagesize = newSize
                this.getDeptList()
            },
            postPageChange: function(newPage){
                this.postPagination.postPageNum =  newPage
                this.getPostList()
            },
            postSizeChange: function(newSize){
                this.postPagination.postPagesize = newSize
                this.getPostList()
            },
            infoSaveClick: function(){
                if (this.modelMainForm.postSkill) {
                    this.modelMainForm.postSkill = this.modelMainForm.postSkill.join(",");
                }
                if(this.CheckEdit()){
                    this.$http.get('EmployeeInfoMain/EmployeeInfoSave',{
                        params:this.modelMainForm
                    }).then(res => {
                        const result = res.data
                        if(result.code == 200){
                            this.$message({
                                showClose: true,
                                message: '保存成功',
                                type: 'success',
                                duration: 2000,
                            })
                            this.addDialogVisible = false
                            this.modelMainForm.cname = ''
                            this.modelMainForm.employeeId = ''
                            this.modelMainForm.postSkill = ''
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
                }else{
                    this.$message({
                        showClose: true,
                        message: "输入项不能为空",
                        type:"warning",
                        duration: 2000
                    })
                }
                
            },
            CheckEdit: function(){
                if(this.modelMainForm.cname == "" || this.modelMainForm.employeeId == "" || this.modelMainForm.postSkill == ""){
                    return false
                }else{
                    return true
                }
            },
            SearchBtn: function(){
                this.queryParam.pageNum = 1
                this.getInfoList()
            },
            AddInfoBtn: function(){
                this.addDialogVisible = true
            },
            DataRefreshBtn: function(){

            },
            closeAddDialog: function(){
                this.addDialogVisible = false
            },
            AddDeptBtn: function(){
                this.deptDialogVisible = true
                this.getDeptList()
                
            },
            addInfoSave: function(){

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
    
    a{
       text-decoration: none;
       color:#303133;
    }
    .rewSel .el-input--suffix{
        width:150px;
        
    }
    .el-input{
        width:180px;
    }
    .el-input__inner {
        width:180px !important;
        height:30px !important;
        line-height: 30px !important;
    }
    .addFormItem{
        width:300px;
        font-size:15px;
    }
    .el-form-item__label{
        font-size:15px;
    }
    .addFormItemNum{
        width:300px;
        font-size:15px;
    }
    .el-input-number.is-controls-right .el-input__inner{
        width:120px !important;
    }
     .el-select{
        width:180px !important;
        height:30px !important;
    }
    .empBaseInfoCss .el-select .el-input__inner{
        width:180px !important;
        height:30px !important;
    }
    .el-pagination{
        margin-bottom:20px;
    }
</style>