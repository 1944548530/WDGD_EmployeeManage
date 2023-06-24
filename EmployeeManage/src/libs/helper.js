import Vue from 'vue'
import {Message} from 'element-ui'

export const downFile = (data, fileName) => {
    if (!data) {
        return false;
    }
    Message({
        showClose: true,
        type: "success",
        duration: 2000,
        message: "信息导出完成，请您注意浏览器的下载管理器！"
    });
    if(window.navigator && window.navigator.msSaveOrOpenBlob){
        window.navigator.msSaveOrOpenBlob(new Blob([data]), fileName)
    }else{
        // 下面就是DOM操作 1.添加一个a标签 2.给a标签添加了属性 3.给他添加了点击事件(点击后移除)   
        const url = window.URL.createObjectURL(new Blob([data]),{type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
        const link = document.createElement("a");
        link.style.display = "none";
        link.href = url;
        link.setAttribute("download", fileName);
        document.body.appendChild(link);
        link.click();
        link.remove();
    }
    // 下面就是DOM操作 1.添加一个a标签 2.给a标签添加了属性 3.给他添加了点击事件(点击后移除)   
    // const url = window.URL.createObjectURL(new Blob([data]),{type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
    // const link = document.createElement("a");
    // link.style.display = "none";
    // link.href = url;
    // link.setAttribute("download", fileName);
    // document.body.appendChild(link);
    // link.click();
    // link.remove();
}

export const getDateFormat = (Date) => {
    var Y = Date.getFullYear();
    var M = Date.getMonth() + 1;
    var D = Date.getDate();
    var times = Y + (M < 10 ? "-0" : "-") + M + (D < 10 ? "-0" : "-") + D;
    return times;
}

