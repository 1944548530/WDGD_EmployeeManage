<template>
    <div class="attendanceCss">
        <el-card style="margin-left:10px;margin-right:10px;margin-top:10px;">
            <div style="width:100%;float:left;margin-left:10px;margin-bottom:10px;">
                <el-select v-model="queryParam.department" placeholder="车间" style="width:180px !important;" clearable>
                    <el-option
                        v-for="item in departmentOption"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value"
                    >
                    </el-option>
                </el-select>&emsp;

                <el-date-picker 
                    style="width:180px;"
                    size="medium"
                    v-model="queryParam.date"
                    value-format="yyyy-MM-dd"
                    placeholder="日期"
                ></el-date-picker>&emsp;
                <el-button-group>
                    <el-button type="primary" @click="SearchBtn" size="medium" icon="el-icon-search">查询</el-button>
                    <el-button type="primary" @click="equipMainBtn" size="medium" icon="el-icon-bangzhu">设备维护</el-button>
                </el-button-group>

                <el-table :data="euipUseInfoList" border style="width: 100%;margin-bottom:10px;margin-top:10px;height:calc(100%-200px)" height="720" stripe :header-cell-style="{background:'#f5f7fa'}"  >
                    <el-table-column type="index" fixed label="序号" prop="index" align="center" width="80px">
                        <template slot-scope="scope">
                            <span>{{ (queryParam.pageNum - 1) * queryParam.pagesize + scope.$index + 1}}</span>
                            <!-- (page - 1) * this.pageSize +  -->
                        </template>
                    </el-table-column>
                    <el-table-column label="日期" align="center" width="180px" prop="date" ></el-table-column>
                    <el-table-column label="车间" align="center" width="180px" prop="department" ></el-table-column>
                    <el-table-column label="设备名称" align="center" width="180px" prop="equipmentName" ></el-table-column>
                    <el-table-column label="现有数量" align="center" width="150px" prop="equipmentNum" ></el-table-column>
                    <el-table-column label="开启数量" align="center" width="150px" prop="openNum" >
                        <template slot-scope="scope">
                            <el-input v-model="scope.row.openNum" width="110px"></el-input>
                        </template>
                    </el-table-column>
                    <el-table-column label="待机数量" align="center" width="150px" prop="cancelNum" >
                        <template slot-scope="scope">
                            <el-input v-model="scope.row.cancelNum" width="110px"></el-input>
                        </template>
                    </el-table-column>
                    <el-table-column label="维修数量" align="center" width="150px" prop="maintainNum" >
                        <template slot-scope="scope">
                            <el-input v-model="scope.row.maintainNum" width="110px"></el-input>
                        </template>
                    </el-table-column>
                    <el-table-column label="是否维护" align="center" width="150px" prop="lmstate" fixed="right">
                        <template slot-scope="scope">
                            <el-tag plain size="medium" :type="scope.row.lmstate=='Y'?'success':'warning'">
                                {{scope.row.lmstate=='Y'?'已维护':'未维护'}}
                            </el-tag>
                        </template>
                    </el-table-column>
                    <el-table-column label="维护人" align="center" width="160px" prop="lmEmployeeName" >
                    </el-table-column>
                    <el-table-column label="操作" align="center" header-align="center" fixed="right" >
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
            :visible.sync="dialogVisible"
            @close="dialogVisible=false"
            title="设备维护"
            width="60%"
            class="abow_dialog"
            :close-on-click-modal="false"
        >
            <el-form style="margin-left:20px;"  :inline="true" ref="formEditRef" v-model="equipForm" label-position="left">
                <el-form-item label="车间" prop="department">
                    <el-select v-model="equipForm.department" placeholder="车间" style="width:180px !important;" >
                        <el-option
                            v-for="item in departmentOption"
                            :key="item.value"
                            :label="item.label"
                            :value="item.value"
                        >
                        </el-option>
                    </el-select>&nbsp;
                </el-form-item>
                <el-form-item label="设备名称" prop="equipName">
                    <el-input v-model="equipForm.equipmentName" placeholder="设备名称">
                    </el-input>
                </el-form-item>
                <el-form-item label="设备数量" prop="equipmentNum">
                    <el-input-number v-model="equipForm.equipmentNum" :min="0"  label="设备数量"></el-input-number>
                </el-form-item>
                <el-form-item >
                    &emsp;&emsp;<el-button type="primary" @click="saveEquipment" size="medium" icon="el-icon-search">保存</el-button>
                    &emsp;&emsp;<el-button type="primary" @click="EquipSearchBtn" size="medium" icon="el-icon-search">查询</el-button>
                </el-form-item>

                <el-table :data="euipmentList" border stripe style="width:90%;margin-top:20px;margin-bottom:10px;" :header-cell-style="{background:'#f5f7fa'}">
                    <el-table-column type="index" fixed label="序号" prop="index" align="center" width="150px">
                        <template slot-scope="scope">
                            <span>{{ (equipParam.pageNum - 1) * equipParam.pagesize + scope.$index + 1}}</span>
                            <!-- (page - 1) * this.pageSize +  -->
                        </template>
                    </el-table-column>
                    <el-table-column label="车间" align="center" header-align="center"  width="200px" prop="department"></el-table-column>
                    <el-table-column label="设备名称" align="center" header-align="center"  width="200px" prop="equipmentName"></el-table-column>
                    <el-table-column label="现有数量" align="center" header-align="center"  width="200px" prop="equipmentNum"></el-table-column>
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
                                    @click="equipDel(scope.row)"
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
                        :current-page="equipParam.pageNum"
                        :page-size="equipParam.pagesize"
                        :page-sizes="[5,10]"
                        :total="equipTotal"
                        @current-change="equipPageChange"
                        @size-change="equipSizeChange"
                        background
                        layout="prev, pager, next, jumper, sizes, total"
                ></el-pagination>
            </el-form>

            

            <div slot="footer" class="dialog-footer">
                <el-button type="primary"  @click="dialogVisible = false">取 消</el-button>
            </div>
        </el-dialog>
    </div>

</template>

<script>
    import qs from 'qs'
    import {downFile, getDateFormat} from '@/libs/helper.js'
    export default{
        name: 'EquipmentManage',
        data: function(){
            return{
                queryParam:{
                    department: '',
                    date:'',
                    pageNum: 1,
                    pagesize: 20
                },
                equipForm:{
                    department: '',
                    equipmentName: '',
                    equipmentNum: 0
                },
                dialogVisible: false,
                departmentOption: [],
                euipmentList: [],
                euipUseInfoList: [],
                equipParam:{
                    pageNum: 1,
                    pagesize: 5,
                    department: '',
                    equipmentName: ''
                },
                equipTotal: 0,
                infoTotal: 0
            }
        },
        created: function(){
            this.getDeptOp()
            this.queryParam.date  = getDateFormat(new Date())
            this.getEquipmentUseList()
        },
        methods:{
            getDeptOp: function(){
                this.$http.get('EmployeeInfoMain/GetDeptOption').then(res => {
                    const result = res.data
                    this.departmentOption = result.Data
                    this.equipForm.department = result.Data[0].value
                })
            },
            SearchBtn: function(){
                this.getEquipmentUseList()
            },
            EquipSearchBtn: function(){
                this.getEquipmentList()
            },
            equipMainBtn: function(){
                this.dialogVisible = true
                this.getEquipmentList()
            },
            SaveOverRow: function(e){
                if(e.equipmentNum < (parseInt(e.openNum) + parseInt(e.cancelNum) + parseInt(e.maintainNum))){
                    this.$message({
                        showClose: true,
                        message: "请确认维护数量！",
                        type: 'error',
                        duration: 2000,
                    })
                    return
                }
                const param = {
                    openNum: e.openNum,
                    cancelNum: e.cancelNum,
                    maintainNum: e.maintainNum,
                    department: e.department,
                    date: e.date,
                    equipmentName: e.equipmentName
                }
                this.$http.post('Equipment/SaveOverRow', this.$qs.stringify(param)).then(res => {
                    const result = res.data
                    console.log(result)
                    if(result.code == "200"){
                        this.getEquipmentUseList()
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
            saveEquipment: function(){
                if(this.equipForm.equipmentName == ""){
                    this.$message({
                        showClose: true,
                        message: "请把信息填写完整！",
                        type: 'warning',
                        duration: 2000,
                    })
                    return
                }
                this.$http.post('Equipment/SaveEquipmentInfo', this.$qs.stringify(this.equipForm)).then(res => {
                    const result = res.data
                    if(result.code == "200"){
                        this.$message({
                            showClose: true,
                            message: result.message,
                            type: 'success',
                            duration: 2000,
                        })
                        this.equipForm.equipmentName = ''
                        this.getEquipmentList()
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
            getEquipmentList: function(){
                this.equipParam.department = this.equipForm.department
                this.equipParam.equipmentName = this.equipForm.equipmentName
                this.$http.get('Equipment/GetEquipmentList',{
                    params:this.equipParam
                }).then(res => {
                    const result = res.data.Data
                    
                    this.euipmentList = result.rows
                    this.equipTotal = result.total
                })
            },
            getEquipmentUseList: function(){
                this.$http.get('Equipment/GetEquipmentUseInfo', {
                    params: this.queryParam
                }).then(res => {
                    const result = res.data.Data
                    const infoLi = this.euipUseInfoMap(result.infoLi)
                    this.euipUseInfoList = infoLi
                    console.log("result")
                    console.log(result)
                    this.infoTotal = result.total
                })
            },
            euipUseInfoMap: function(arr){
                const result = arr.map(item => {
                    return{
                        date: item.date,
                        department: item.department,
                        equipmentName: item.equipmentName,
                        equipmentNum: item.equipmentNum === 0 ? '' : item.equipmentNum,
                        openNum: item.openNum === 0 ? '' : item.openNum,
                        cancelNum: item.cancelNum === 0 ? '' : item.cancelNum,
                        maintainNum: item.maintainNum === 0 ? '' : item.maintainNum,
                        lmstate: item.lmstate,
                        lmEmployeeName: item.lmEmployeeName
                    }
                    
                })
                return result;
            },
            equipDel: function(e){
                var queryBody = {
                    department: e.department,
                    equipmentName: e.equipmentName
                }
                this.$http.post('Equipment/EquipmentInfoDel', this.$qs.stringify(queryBody)).then(res => {
                    const result = res.data
                    if(result.code == "200"){
                        this.getEquipmentList()
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
            equipPageChange: function(newPage){
                this.equipParam.pageNum = newPage
                this.getEquipmentList()
            },
            equipSizeChange: function(newSize){
                this.equipParam.pagesize = newSize
                this.getEquipmentList()
            },
            PageChange: function(newPage){
                this.queryParam.pageNum = newPage
                this.getEquipmentUseList()
            },
            SizeChange: function(newSize){
                this.queryParam.pagesize = newSize
                this.getEquipmentUseList()
            }
        }
    }
</script>

<style type="text/css" scoped>
    
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
    /* .attendanceCss .el-input{
        width:180px !important;
    } */
    .el-date-editor .el-range-input{
        width: 19% !important;
    }
    .el-date-editor--daterange.el-input, .el-date-editor--daterange.el-input__inner, .el-date-editor--timerange.el-input, .el-date-editor--timerange.el-input__inner{
        width:200px !important;
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
        height: 37px !important;
        line-height: 37px !important;
        width:100px !important;
    }
    .el-pagination{
        margin-bottom:10px;
    }
    .paginationCss{
        margin-top:20px;
    }
    .el-input--small .el-input__inner{
        height: 37px !important;
        line-height: 37px !important;
    }
    .el-input--mini .el-input__inner{
        height: 28px !important;
        line-height: 28px !important;
        width:100px !important;
    }
</style>