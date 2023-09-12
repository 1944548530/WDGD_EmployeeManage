<template>
    <div>
        <el-card style="margin-left:10px;margin-right:10px;height:100%;font-size:18px;">
            <el-row>
                <el-col :span="12">
                    <el-upload
                        class="upload-demo"
                        ref="upload"
                        action=""
                        :http-request="httpRequest"
                        accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
                        :on-preview="handlePreview"
                        :on-remove="handleRemove"
                        :before-remove="beforeRemove"
                        multiple
                        :limit="1"
                        :on-exceed="handleExceed"
                        :auto-upload="false"
                        :file-list="fileList">
                        <el-button slot="trigger" size="medium" type="primary">选取文件</el-button>
                        <el-button style="margin-left: 10px;" size="medium" type="success" @click="submitUpload">上传</el-button>
                        <div slot="tip" class="el-upload__tip" style="margin-top:10px;">只能上传Excel文件</div>
                    </el-upload>
                </el-col>
            </el-row>
            <el-divider></el-divider>
            <el-row>
                <el-col :span="24">
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
                </el-col>
            </el-row>
            
        </el-card>
    </div>
    
</template>

<script>
    import qs from 'qs'
    import {downFile, getDateFormat} from '@/libs/helper.js'

    export default{
        name: 'ProductionPlan',
        data: function(){
            return{
                fileList: [],
                departmentOption: [],
                queryParam:{
                    department: ''
                }
            }
        },
        created: function(){

        },
        mounted: function(){
            
        },
        methods:{
            submitUpload() {
                this.$refs.upload.submit();
            },
            httpRequest(param){
                let fileObj = param.file
                let fd = new FormData()

                let url = "/ProductionPlan/ProductionImport"
                let config = {
                    headers:{
                        'Content-Type': 'multipart/form-data'
                    }
                }
                fd.append('file', fileObj)
                this.$http.post(url, fd, config).then(res => {
                    
                })
            },
            handleRemove(file, fileList) {
            },
            handlePreview(file) {
                console.log(file);
            },
            handleExceed(files, fileList) {
            },
            beforeRemove(file, fileList) {
                return this.$confirm(`确定移除 ${ file.name }？`);
            },
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