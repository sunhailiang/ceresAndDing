(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-794acb6c"],{"277b":function(e,t,a){"use strict";var r=a("ac06"),n=a.n(r);n.a},ac06:function(e,t,a){},e017:function(e,t,a){"use strict";var r=a("e1b6"),n=a.n(r);n.a},e1b6:function(e,t,a){},ebdd:function(e,t,a){"use strict";a.r(t);var r=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("a-card",{attrs:{bordered:!1}},[a("div",{staticClass:"table-page-search-wrapper"},[a("a-form",{attrs:{layout:"inline"}},[a("a-row",{attrs:{gutter:48}},[a("a-col",{attrs:{md:8,sm:24}},[a("a-form-item",{attrs:{label:"姓名"}},[a("a-input",{attrs:{placeholder:""},model:{value:e.queryParam.customerName,callback:function(t){e.$set(e.queryParam,"customerName",t)},expression:"queryParam.customerName"}})],1)],1),a("a-col",{attrs:{md:8,sm:24}},[a("a-form-item",{attrs:{label:"手机"}},[a("a-input",{attrs:{placeholder:""},model:{value:e.queryParam.cellphone,callback:function(t){e.$set(e.queryParam,"cellphone",t)},expression:"queryParam.cellphone"}})],1)],1),e.advanced?[a("a-col",{attrs:{md:8,sm:24}},[a("a-form-item",{attrs:{label:"服务类别"}},[a("a-select",{attrs:{placeholder:"请选择","default-value":"0"},model:{value:e.queryParam.serviceOid,callback:function(t){e.$set(e.queryParam,"serviceOid",t)},expression:"queryParam.serviceOid"}},e._l(e.serviceType,(function(t){return a("a-select-option",{key:t.oid,attrs:{value:t.oid}},[e._v(" "+e._s(t.name)+" ")])})),1)],1)],1),a("a-col",{attrs:{md:8,sm:24}},[a("a-form-item",{attrs:{label:"归属客服"}},[a("a-select",{attrs:{placeholder:"请选择","default-value":"0"},model:{value:e.queryParam.supporterOid,callback:function(t){e.$set(e.queryParam,"supporterOid",t)},expression:"queryParam.supporterOid"}},e._l(e.supporterList,(function(t){return a("a-select-option",{key:t.oid,attrs:{value:t.oid}},[e._v(" "+e._s(t.userName)+" ")])})),1)],1)],1),a("a-col",{attrs:{md:8,sm:24}},[a("a-form-item",{attrs:{label:"归属代理"}},[a("a-select",{attrs:{placeholder:"请选择","default-value":"0"},model:{value:e.queryParam.agenterOid,callback:function(t){e.$set(e.queryParam,"agenterOid",t)},expression:"queryParam.agenterOid"}},e._l(e.agenterList,(function(t){return a("a-select-option",{key:t.oid,attrs:{value:t.oid}},[e._v(" "+e._s(t.userName)+" ")])})),1)],1)],1)]:e._e(),a("a-col",{attrs:{md:e.advanced?24:8,sm:24}},[a("span",{staticClass:"table-page-search-submitButtons",style:e.advanced&&{float:"right",overflow:"hidden"}||{}},[a("a-button",{attrs:{type:"primary"},on:{click:e.loadDataCondition}},[e._v("查询")]),a("a-button",{staticStyle:{"margin-left":"8px"},on:{click:function(){return e.queryParam={}}}},[e._v("重置")]),a("a",{staticStyle:{"margin-left":"8px"},on:{click:e.toggleAdvanced}},[e._v(" "+e._s(e.advanced?"收起":"展开")+" "),a("a-icon",{attrs:{type:e.advanced?"up":"down"}})],1)],1)])],2)],1)],1),a("div",{staticClass:"table-operator"},[a("a-button",{attrs:{type:"primary",icon:"plus"},on:{click:function(t){return e.CreateUser()}}},[e._v("新建")])],1),a("s-table",{ref:"table",attrs:{size:"default",rowKey:"id",columns:e.columns,data:e.loadData,alert:e.options.alert,rowSelection:e.options.rowSelection,delete:e.options.delete,showPagination:"auto"},scopedSlots:e._u([{key:"serial",fn:function(t,r,n){return a("span",{},[e._v(e._s(n+1))])}},{key:"status",fn:function(t){return a("span",{},[a("a-badge",{attrs:{status:e._f("statusTypeFilter")(t),text:e._f("statusFilter")(t)}})],1)}},{key:"description",fn:function(t){return a("span",{},[a("ellipsis",{attrs:{length:4,tooltip:""}},[e._v(e._s(t))])],1)}},{key:"action",fn:function(t,r){return a("span",{},[[a("a",{on:{click:function(t){return e.handleEdit(r.oid)}}},[e._v("详情")])]],2)}}])}),a("AddCustomerModal",{ref:"addUserModal",on:{save:e.saveUser}})],1)},n=[],s=(a("acd8"),a("e25e"),a("c1df")),i=a.n(s),o=a("2af9"),l=(a("a4d3"),a("99af"),a("4de4"),a("caad"),a("d81d"),a("b0c0"),a("a9e3"),a("e439"),a("dbb4"),a("b64b"),a("2532"),a("159b"),a("2638")),c=a.n(l),u=a("53ca"),d=a("ade3"),p=a("372e"),h=a("c832"),f=a.n(h);function m(e,t){var a=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);t&&(r=r.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),a.push.apply(a,r)}return a}function g(e){for(var t=1;t<arguments.length;t++){var a=null!=arguments[t]?arguments[t]:{};t%2?m(Object(a),!0).forEach((function(t){Object(d["a"])(e,t,a[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(a)):m(Object(a)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(a,t))}))}return e}var v={data:function(){return{needTotalList:[],selectedRows:[],selectedRowKeys:[],localLoading:!1,localDataSource:[],localPagination:Object.assign({},this.pagination)}},props:Object.assign({},p["a"].props,{rowKey:{type:[String,Function],default:"key"},data:{type:Function,required:!0},pageNum:{type:Number,default:1},pageSize:{type:Number,default:10},showSizeChanger:{type:Boolean,default:!0},size:{type:String,default:"default"},alert:{type:[Object,Boolean],default:null},rowSelection:{type:Object,default:null},delete:{type:Object,default:null},showAlertInfo:{type:Boolean,default:!1},showPagination:{type:String|Boolean,default:"auto"},pageURI:{type:Boolean,default:!1}}),watch:{data:function(e){this.data=e,this.localPagination=Object.assign({},{current:1,pageSize:this.pageSize}),this.loadData()},"localPagination.current":function(e){this.pageURI&&this.$router.push(g({},this.$route,{name:this.$route.name,params:Object.assign({},this.$route.params,{pageIndex:e})}))},pageNum:function(e){Object.assign(this.localPagination,{current:e})},pageSize:function(e){Object.assign(this.localPagination,{pageSize:e})},showSizeChanger:function(e){Object.assign(this.localPagination,{showSizeChanger:e})}},created:function(){var e=this.$route.params.pageIndex,t=this.pageURI&&e&&parseInt(e)||this.pageNum;this.localPagination=["auto",!0].includes(this.showPagination)&&Object.assign({},this.localPagination,{current:t,pageSize:this.pageSize,showSizeChanger:this.showSizeChanger})||!1,this.needTotalList=this.initTotalList(this.columns),this.loadData()},methods:{refresh:function(){var e=arguments.length>0&&void 0!==arguments[0]&&arguments[0];e&&(this.localPagination=Object.assign({},{current:1,pageSize:this.pageSize})),this.loadData()},loadData:function(e,t,a){var r=this;this.localLoading=!0;var n=Object.assign({pageIndex:e&&e.current||this.showPagination&&this.localPagination.current||this.pageNum,pageSize:e&&e.pageSize||this.showPagination&&this.localPagination.pageSize||this.pageSize},a&&a.field&&{sortField:a.field}||{},a&&a.order&&{sortOrder:a.order}||{},g({},t)),s=this.data(n);"object"!==Object(u["a"])(s)&&"function"!==typeof s||"function"!==typeof s.then||s.then((function(t){if(r.localPagination=r.showPagination&&Object.assign({},r.localPagination,{current:t.pageIndex,total:t.dataCount,showSizeChanger:r.showSizeChanger,pageSize:e&&e.pageSize||r.localPagination.pageSize})||!1,0===t.data.length&&r.showPagination&&r.localPagination.current>1)return r.localPagination.current--,void r.loadData();try{["auto",!0].includes(r.showPagination)&&t.dataCount<=t.pageIndex*r.localPagination.pageSize&&(r.localPagination.hideOnSinglePage=!0)}catch(a){r.localPagination=!1}r.localDataSource=t.data,r.localLoading=!1}))},initTotalList:function(e){var t=[];return e&&e instanceof Array&&e.forEach((function(e){e.needTotal&&t.push(g({},e,{total:0}))})),t},updateSelect:function(e,t){this.selectedRows=t,this.selectedRowKeys=e;var a=this.needTotalList;this.needTotalList=a.map((function(e){return g({},e,{total:t.reduce((function(t,a){var r=t+parseInt(f()(a,e.dataIndex));return isNaN(r)?0:r}),0)})}))},clearSelected:function(){this.rowSelection&&(this.rowSelection.onChange([],[]),this.updateSelect([],[]))},renderClear:function(e){var t=this,a=this.$createElement;return this.selectedRowKeys.length<=0?null:a("a",{style:"margin-left: 24px",on:{click:function(){e(),t.clearSelected()}}},["清空选项"])},renderDelete:function(e){var t=this,a=this.$createElement;return this.selectedRowKeys.length<=0?null:a("a",{style:"margin-left: 24px",on:{click:function(){e(),t.clearSelected()}}},["删除"])},renderAlert:function(){var e=this.$createElement,t=this.needTotalList.map((function(t){return e("span",{style:"margin-right: 12px"},[t.title,"总计"," ",e("a",{style:"font-weight: 600"},[t.customRender?t.customRender(t.total):t.total])])})),a="boolean"===typeof this.alert.clear&&this.alert.clear?this.renderClear(this.clearSelected):null!==this.alert&&"function"===typeof this.alert.clear?this.renderClear(this.alert.clear):null,r=this.delete.show&&"function"===typeof this.delete.delete?this.renderDelete(this.delete.delete):null;return e("a-alert",{attrs:{showIcon:!0},style:"margin-bottom: 16px"},[e("template",{slot:"message"},[e("span",{style:"margin-right: 12px"},["已选择: ",e("a",{style:"font-weight: 600"},[this.selectedRows.length])]),t,a,r])])}},render:function(){var e=this,t=arguments[0],a={},r=Object.keys(this.$data),n="object"===Object(u["a"])(this.alert)&&null!==this.alert&&this.alert.show&&"undefined"!==typeof this.rowSelection.selectedRowKeys||this.alert;Object.keys(p["a"].props).forEach((function(t){var s="local".concat(t.substring(0,1).toUpperCase()).concat(t.substring(1));if(r.includes(s))return a[t]=e[s],a[t];if("rowSelection"===t){if(n&&e.rowSelection)return a[t]=g({},e.rowSelection,{selectedRows:e.selectedRows,selectedRowKeys:e.selectedRowKeys,onChange:function(a,r){e.selectedRowKeys=a,e.updateSelect(a,r),"undefined"!==typeof e[t].onChange&&e[t].onChange(a,r)}}),a[t];if(!e.rowSelection)return a[t]=null,a[t]}return e[t]&&(a[t]=e[t]),a[t]}));var s=t("a-table",c()([{},{props:a,scopedSlots:g({},this.$scopedSlots)},{on:{change:this.loadData}}]),[Object.keys(this.$slots).map((function(a){return t("template",{slot:a},[e.$slots[a]])}))]);return t("div",{class:"table-wrapper"},[n?this.renderAlert():null,s])}},b=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("a-modal",{attrs:{title:"设置VIP",width:640,visible:e.visible,destroyOnClose:!0,confirmLoading:e.confirmLoading},on:{cancel:e.closeModal}},[a("a-spin",{attrs:{spinning:e.confirmLoading}},[a("a-form",{attrs:{form:e.form}},[a("a-form-item",{attrs:{label:"检索客户",labelCol:e.labelCol,wrapperCol:e.wrapperCol}},[a("div",{staticClass:"search"},[a("div",{staticClass:"phone"},[a("a-input-search",{staticStyle:{width:"200px"},attrs:{placeholder:"请输入手机号"},model:{value:e.phone,callback:function(t){e.phone=t},expression:"phone"}}),a("a-button",{staticClass:"search",attrs:{type:"primary"},on:{click:e.getCustomerByPhone}},[e._v("搜索")])],1)])])],1),a("div",{staticClass:"tb"},[a("ul",{staticClass:"title"},[a("li"),a("li",[e._v("姓名")]),a("li",[e._v("性别")]),a("li",[e._v("城市")]),a("li",[e._v("手机")])]),e.userInfo["userId"]?a("ul",{staticClass:"data"},[a("li",[a("a-checkbox",{on:{change:e.selectUser}})],1),e._l(Object.keys(e.userInfo).filter((function(e){return"userId"!=e&&"province"!=e&&"age"!=e&&"initWeight"!=e&&"initHeight"!=e})),(function(t){return a("li",{key:t},[e._v(" "+e._s(e.userInfo[t])+" ")])}))],2):e._e(),e.userInfo["userId"]?e._e():a("ul",{staticClass:"nodata"},[a("li",[e._v("暂无数据！")])])]),e.userId?a("div",{staticClass:"form"},[a("a-form",{attrs:{form:e.form}},[a("a-form-item",{attrs:{label:"姓名","label-col":{span:5},"wrapper-col":{span:12}}},[a("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["name",{rules:[{required:!0,message:"请输入客户姓名"}],initialValue:this.userInfo.userName}],expression:"[\n              'name',\n              { rules: [{ required: true, message: '请输入客户姓名' }], initialValue: this.userInfo.userName }\n            ]"}],staticClass:"num-input",attrs:{value:"xxx",placeholder:"请输入客户姓名"}})],1),a("a-form-item",{attrs:{label:"年龄","label-col":{span:5},"wrapper-col":{span:12}}},[a("a-input-number",{directives:[{name:"decorator",rawName:"v-decorator",value:["age",{rules:[{required:!0,message:"请输入客户年龄"}],initialValue:this.userInfo.age}],expression:"[\n              'age',\n              { rules: [{ required: true, message: '请输入客户年龄' }], initialValue: this.userInfo.age }\n            ]"}],staticClass:"num-input",attrs:{min:10,placeholder:"请输入客户年龄"}})],1),a("a-form-item",{attrs:{label:"省","label-col":{span:5},"wrapper-col":{span:12}}},[a("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["province",{rules:[{required:!0,message:"请输入客户所在省"}],initialValue:this.userInfo.province}],expression:"[\n              'province',\n              { rules: [{ required: true, message: '请输入客户所在省' }], initialValue: this.userInfo.province }\n            ]"}],attrs:{placeholder:"请输入客户所在省"}})],1),a("a-form-item",{attrs:{label:"市","label-col":{span:5},"wrapper-col":{span:12}}},[a("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["city",{rules:[{required:!0,message:"请输入客户所在市"}],initialValue:this.userInfo.city}],expression:"[\n              'city',\n              { rules: [{ required: true, message: '请输入客户所在市' }], initialValue: this.userInfo.city }\n            ]"}],attrs:{placeholder:"请输入客户所在市"}})],1),a("a-form-item",{attrs:{label:"初始体重","label-col":{span:5},"wrapper-col":{span:12}}},[a("a-input-number",{directives:[{name:"decorator",rawName:"v-decorator",value:["weight",{rules:[{required:!0,message:"请输入客户初始体重"}],initialValue:this.userInfo.initWeight}],expression:"[\n              'weight',\n              { rules: [{ required: true, message: '请输入客户初始体重' }], initialValue: this.userInfo.initWeight }\n            ]"}],staticClass:"num-input",attrs:{min:0,placeholder:"请输入初始体重"}})],1),a("a-form-item",{attrs:{label:"身高","label-col":{span:5},"wrapper-col":{span:12}}},[a("a-input-number",{directives:[{name:"decorator",rawName:"v-decorator",value:["height",{rules:[{required:!0,message:"请输入客户身高"}],initialValue:this.userInfo.initHeight}],expression:"[\n              'height',\n              { rules: [{ required: true, message: '请输入客户身高' }], initialValue: this.userInfo.initHeight }\n            ]"}],staticClass:"num-input",attrs:{min:0,placeholder:"请输入客户身高"}})],1),a("a-form-item",{attrs:{label:"工作名称","label-col":{span:5},"wrapper-col":{span:12}}},[a("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["job",{rules:[{required:!0,message:"请输入客户工作名称"}]}],expression:"['job', { rules: [{ required: true, message: '请输入客户工作名称' }] }]"}],attrs:{placeholder:"请输入客户工作名称"}})],1),a("a-form-item",{attrs:{label:"工作强度","label-col":{span:5},"wrapper-col":{span:12}}},[a("a-radio-group",{directives:[{name:"decorator",rawName:"v-decorator",value:["intensity"],expression:"['intensity']"}],attrs:{defaultValue:"1",buttonStyle:"solid"}},[a("a-radio-button",{attrs:{value:"1"}},[e._v("极轻")]),a("a-radio-button",{attrs:{value:"2"}},[e._v("轻")]),a("a-radio-button",{attrs:{value:"3"}},[e._v("中")]),a("a-radio-button",{attrs:{value:"4"}},[e._v("重")])],1)],1),a("a-form-item",{attrs:{label:"类别","label-col":{span:5},"wrapper-col":{span:12}}},[a("a-select",{directives:[{name:"decorator",rawName:"v-decorator",value:["type",{rules:[{required:!0,message:"请选择客户类别"}]}],expression:"['type', { rules: [{ required: true, message: '请选择客户类别' }] }]"}],attrs:{placeholder:"请选客户类别"}},e._l(e.serviceType,(function(t){return a("a-select-option",{key:t.oid,attrs:{value:t.oid}},[e._v(" "+e._s(t.name)+" ")])})),1)],1),a("a-form-item",{attrs:{label:"归属客服","label-col":{span:5},"wrapper-col":{span:12}}},[a("a-select",{directives:[{name:"decorator",rawName:"v-decorator",value:["service",{rules:[{required:!0,message:"请选择归属客服"}]}],expression:"['service', { rules: [{ required: true, message: '请选择归属客服' }] }]"}],attrs:{placeholder:"请选择归属客服"}},e._l(e.supporterList,(function(t){return a("a-select-option",{key:t.oid,attrs:{value:t.oid}},[e._v(" "+e._s(t.userName)+" ")])})),1)],1),a("a-form-item",{attrs:{label:"归属代理","label-col":{span:5},"wrapper-col":{span:12}}},[a("a-select",{directives:[{name:"decorator",rawName:"v-decorator",value:["agency",{rules:[{required:!0,message:"请选择归属代理"}]}],expression:"['agency', { rules: [{ required: true, message: '请选择归属代理' }] }]"}],attrs:{placeholder:"请选择归属代理"}},e._l(e.agenterList,(function(t){return a("a-select-option",{key:t.oid,attrs:{value:t.oid}},[e._v(" "+e._s(t.userName)+" ")])})),1)],1)],1)],1):e._e()],1),a("template",{slot:"footer"},[a("a-button",{key:"cancel",style:{float:"left"},on:{click:e.closeModal}},[e._v("取消")]),a("a-button",{key:"confirm",on:{click:e.saveData}},[e._v("确认")])],1)],2)},y=[],w=a("0fea"),I={name:"FormModal",data:function(){var e;return e={bakUser:{},formLayout:"horizontal",form:this.$form.createForm(this),userPhone:"",labelCol:{xs:{span:24},sm:{span:7}},wrapperCol:{xs:{span:24},sm:{span:13}},visible:!1,confirmLoading:!1,currentStep:0,mdl:{},userInfo:{},userId:""},Object(d["a"])(e,"form",this.$form.createForm(this)),Object(d["a"])(e,"phone",""),Object(d["a"])(e,"serviceType",[]),Object(d["a"])(e,"supporterList",[]),Object(d["a"])(e,"agenterList",[]),e},mounted:function(){var e=this;Object(w["d"])().then((function(t){"null"!=t.response&&(e.agenterList=t.response)})),Object(w["z"])().then((function(t){"null"!=t.response&&(e.supporterList=t.response)})),Object(w["y"])().then((function(t){"null"!=t.response&&(e.serviceType=t.response)}))},methods:{getCustomerByPhone:function(){var e=this;this.userId="",this.userInfo={userId:"",userName:"",sex:"",city:"",cellphone:""},Object(w["x"])(this.phone).then((function(t){if(t.success){var a=t.response;e.userInfo.userId=a.oid,e.userInfo.userName=a.userName,e.userInfo.sex=1==a.sex?"男":"女",e.userInfo.city=a.city,e.userInfo.cellphone=a.cellphone,e.userInfo.initHeight=a.initHeight,e.userInfo.initWeight=a.initWeight,e.userInfo.province=a.province,e.bakUser=a}}))},selectUser:function(e){e.target.checked&&""!=this.userInfo.userId?this.userId=this.userInfo.userId:this.userId=""},openModal:function(){this.visible=!0},closeModal:function(){this.visible=!1},refresh:function(){this.form.validateFields((function(e,t){e||{}}))},saveData:function(){var e=this;this.form.validateFields((function(t,a){void 0==a.intensity?e.bakUser.jobStrength="1":e.bakUser.jobStrength=a.intensity,e.bakUser.initHeight=a.height,e.bakUser.initWeight=a.weight,e.bakUser.jobName=a.job,t||e.$emit("save",Object.assign(e.bakUser,a))}))}}},O=I,S=(a("277b"),a("2877")),j=Object(S["a"])(O,b,y,!1,null,"94835e58",null),x=j.exports,C=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("a-modal",{attrs:{title:"新建规则",width:640,visible:e.visible,confirmLoading:e.confirmLoading},on:{ok:e.handleSubmit,cancel:e.handleCancel}},[a("a-spin",{attrs:{spinning:e.confirmLoading}},[a("a-form",{attrs:{form:e.form}},[a("a-form-item",{attrs:{label:"描述",labelCol:e.labelCol,wrapperCol:e.wrapperCol}},[a("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["desc",{rules:[{required:!0,min:5,message:"请输入至少五个字符的规则描述！"}]}],expression:"['desc', {rules: [{required: true, min: 5, message: '请输入至少五个字符的规则描述！'}]}]"}]})],1)],1)],1)],1)},P=[],_={data:function(){return{labelCol:{xs:{span:24},sm:{span:7}},wrapperCol:{xs:{span:24},sm:{span:13}},visible:!1,confirmLoading:!1,form:this.$form.createForm(this)}},methods:{add:function(){this.visible=!0},handleSubmit:function(){var e=this,t=this.form.validateFields;this.confirmLoading=!0,t((function(t,a){t?e.confirmLoading=!1:setTimeout((function(){e.visible=!1,e.confirmLoading=!1,e.$emit("ok",a)}),1500)}))},handleCancel:function(){this.visible=!1}}},k=_,q=Object(S["a"])(k,C,P,!1,null,null,null),N=q.exports,L={0:{status:"default",text:"女"},1:{status:"processing",text:"男"}},z={name:"TableList",components:{STable:v,Ellipsis:o["e"],CreateForm:N,AddCustomerModal:x},data:function(){var e=this;return{agenterList:[],supporterList:[],serviceType:[],userInfo:{oid:"",userName:"",sex:0,age:0,province:"",city:"",initHeight:0,initWeight:0,supporterOid:"",jobName:"",jobStrength:"",serviceOid:"",agenterOid:""},mdl:{},advanced:!1,queryParam:{},columns:[{title:"客户编号",dataIndex:"oid"},{title:"姓名",dataIndex:"userName"},{title:"性别",dataIndex:"sex",customRender:function(e){return 1==e?"男":"女"}},{title:"年龄",dataIndex:"age"},{title:"地区",dataIndex:"city"},{title:"类别",dataIndex:"serviceName"},{title:"归属客服",dataIndex:"supporterName"},{title:"操作",dataIndex:"action",width:"150px",scopedSlots:{customRender:"action"}}],loadData:function(t){return Object(w["r"])(Object.assign(t,e.queryParam)).then((function(e){return null!==e.response?e.response.customerList:{pageIndex:1,pageSize:10,dataCount:0,pageCount:0,data:[]}}))},selectedRowKeys:[],selectedRows:[],options:{alert:{show:!0,clear:function(){e.selectedRowKeys=[]}},rowSelection:{selectedRowKeys:this.selectedRowKeys,onChange:this.onSelectChange},delete:{show:!0,delete:function(){}}},optionAlertShow:!1}},filters:{statusFilter:function(e){return L[e].text},statusTypeFilter:function(e){return L[e].status}},mounted:function(){var e=this;Object(w["d"])().then((function(t){"null"!=t.response&&(e.agenterList=t.response)})),Object(w["z"])().then((function(t){"null"!=t.response&&(e.supporterList=t.response)})),Object(w["y"])().then((function(t){"null"!=t.response&&(e.serviceType=t.response)}))},methods:{loadDataCondition:function(){var e=this;this.loadData=function(t){return Object(w["s"])(Object.assign(t,e.queryParam)).then((function(e){return e.response?e.response.customerList:{pageIndex:1,pageSize:10,dataCount:0,pageCount:0,data:[]}}))}},getDom:function(e){},handlePanelChange:function(e){e&&setTimeout(void 0,2e3)},saveUser:function(e){var t=this;this.userInfo.oid=e.oid,this.userInfo.userName=e.userName,this.userInfo.sex=parseInt(e.sex),this.userInfo.age=parseInt(e.age),this.userInfo.province=null==e.province?"":e.province,this.userInfo.city=e.city,this.userInfo.initHeight=parseFloat(e.initHeight),this.userInfo.initWeight=parseFloat(e.initWeight),this.userInfo.supporterOid=e.service,this.userInfo.jobName=e.job,this.userInfo.jobStrength=e.jobStrength,this.userInfo.serviceOid=e.type,this.userInfo.agenterOid=e.agency,Object(w["a"])(this.userInfo).then((function(e){e.success?(setTimeout((function(){t.$notification.success({message:"新增成功"})}),1500),t.loadDataCondition(),t.$refs.addUserModal.closeModal()):t.$notification["error"]({message:"错误",description:e.message+":"+e.response,duration:4})}))},CreateUser:function(e){this.$refs.addUserModal.openModal()},handleEdit:function(e){this.$router.push("/customer/detail/".concat(e))},handleOk:function(e){this.$refs.modal.closeModal()},onSelectChange:function(e,t){this.selectedRowKeys=e,this.selectedRows=t},toggleAdvanced:function(){this.advanced=!this.advanced},resetSearchForm:function(){this.queryParam={date:i()(new Date)}}}},$=z,R=(a("e017"),Object(S["a"])($,r,n,!1,null,"1f95d9b6",null));t["default"]=R.exports}}]);