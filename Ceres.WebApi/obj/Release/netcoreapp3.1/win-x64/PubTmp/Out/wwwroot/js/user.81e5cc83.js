(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["user"],{"00d8":function(e,t){(function(){var t="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/",r={rotl:function(e,t){return e<<t|e>>>32-t},rotr:function(e,t){return e<<32-t|e>>>t},endian:function(e){if(e.constructor==Number)return 16711935&r.rotl(e,8)|4278255360&r.rotl(e,24);for(var t=0;t<e.length;t++)e[t]=r.endian(e[t]);return e},randomBytes:function(e){for(var t=[];e>0;e--)t.push(Math.floor(256*Math.random()));return t},bytesToWords:function(e){for(var t=[],r=0,a=0;r<e.length;r++,a+=8)t[a>>>5]|=e[r]<<24-a%32;return t},wordsToBytes:function(e){for(var t=[],r=0;r<32*e.length;r+=8)t.push(e[r>>>5]>>>24-r%32&255);return t},bytesToHex:function(e){for(var t=[],r=0;r<e.length;r++)t.push((e[r]>>>4).toString(16)),t.push((15&e[r]).toString(16));return t.join("")},hexToBytes:function(e){for(var t=[],r=0;r<e.length;r+=2)t.push(parseInt(e.substr(r,2),16));return t},bytesToBase64:function(e){for(var r=[],a=0;a<e.length;a+=3)for(var s=e[a]<<16|e[a+1]<<8|e[a+2],n=0;n<4;n++)8*a+6*n<=8*e.length?r.push(t.charAt(s>>>6*(3-n)&63)):r.push("=");return r.join("")},base64ToBytes:function(e){e=e.replace(/[^A-Z0-9+\/]/gi,"");for(var r=[],a=0,s=0;a<e.length;s=++a%4)0!=s&&r.push((t.indexOf(e.charAt(a-1))&Math.pow(2,-2*s+8)-1)<<2*s|t.indexOf(e.charAt(a))>>>6-2*s);return r}};e.exports=r})()},1037:function(e,t,r){"use strict";r.r(t);var a=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("result",{attrs:{isSuccess:!0,content:!1,title:e.email,description:e.description}},[r("template",{slot:"action"},[r("a-button",{attrs:{size:"large",type:"primary"}},[e._v("查看邮箱")]),r("a-button",{staticStyle:{"margin-left":"8px"},attrs:{size:"large"},on:{click:e.goHomeHandle}},[e._v("返回首页")])],1)],2)},s=[],n=r("2af9"),o={name:"RegisterResult",components:{Result:n["p"]},data:function(){return{description:"激活邮件已发送到你的邮箱中，邮件有效期为24小时。请及时登录邮箱，点击邮件中的链接激活帐户。",form:{}}},computed:{email:function(){var e=this.form&&this.form.email||"xxx",t="你的账户：".concat(e," 注册成功");return t}},created:function(){this.form=this.$route.params},methods:{goHomeHandle:function(){this.$router.push({name:"login"})}}},i=o,l=r("2877"),c=Object(l["a"])(i,a,s,!1,null,"2618b3a4",null);t["default"]=c.exports},1348:function(e,t,r){"use strict";r.r(t);var a=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{staticClass:"main user-layout-register"},[e._m(0),r("a-form",{ref:"formRegister",attrs:{form:e.form,id:"formRegister"}},[r("a-form-item",[r("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["email",{rules:[{required:!0,type:"email",message:"请输入邮箱地址"}],validateTrigger:["change","blur"]}],expression:"['email', {rules: [{ required: true, type: 'email', message: '请输入邮箱地址' }], validateTrigger: ['change', 'blur']}]"}],attrs:{size:"large",type:"text",placeholder:"邮箱"}})],1),r("a-popover",{attrs:{placement:"rightTop",trigger:["focus"],getPopupContainer:function(e){return e.parentElement}},model:{value:e.state.passwordLevelChecked,callback:function(t){e.$set(e.state,"passwordLevelChecked",t)},expression:"state.passwordLevelChecked"}},[r("template",{slot:"content"},[r("div",{style:{width:"240px"}},[r("div",{class:["user-register",e.passwordLevelClass]},[e._v("强度："),r("span",[e._v(e._s(e.passwordLevelName))])]),r("a-progress",{attrs:{percent:e.state.percent,showInfo:!1,strokeColor:e.passwordLevelColor}}),r("div",{staticStyle:{"margin-top":"10px"}},[r("span",[e._v("请至少输入 6 个字符。请不要使用容易被猜到的密码。")])])],1)]),r("a-form-item",[r("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["password",{rules:[{required:!0,message:"至少6位密码，区分大小写"},{validator:this.handlePasswordLevel}],validateTrigger:["change","blur"]}],expression:"['password', {rules: [{ required: true, message: '至少6位密码，区分大小写'}, { validator: this.handlePasswordLevel }], validateTrigger: ['change', 'blur']}]"}],attrs:{size:"large",type:"password",autocomplete:"false",placeholder:"至少6位密码，区分大小写"},on:{click:e.handlePasswordInputClick}})],1)],2),r("a-form-item",[r("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["password2",{rules:[{required:!0,message:"至少6位密码，区分大小写"},{validator:this.handlePasswordCheck}],validateTrigger:["change","blur"]}],expression:"['password2', {rules: [{ required: true, message: '至少6位密码，区分大小写' }, { validator: this.handlePasswordCheck }], validateTrigger: ['change', 'blur']}]"}],attrs:{size:"large",type:"password",autocomplete:"false",placeholder:"确认密码"}})],1),r("a-form-item",[r("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["mobile",{rules:[{required:!0,message:"请输入正确的手机号",pattern:/^1[3456789]\d{9}$/},{validator:this.handlePhoneCheck}],validateTrigger:["change","blur"]}],expression:"['mobile', {rules: [{ required: true, message: '请输入正确的手机号', pattern: /^1[3456789]\\d{9}$/ }, { validator: this.handlePhoneCheck } ], validateTrigger: ['change', 'blur'] }]"}],attrs:{size:"large",placeholder:"11 位手机号"}},[r("a-select",{attrs:{slot:"addonBefore",size:"large",defaultValue:"+86"},slot:"addonBefore"},[r("a-select-option",{attrs:{value:"+86"}},[e._v("+86")]),r("a-select-option",{attrs:{value:"+87"}},[e._v("+87")])],1)],1)],1),r("a-row",{attrs:{gutter:16}},[r("a-col",{staticClass:"gutter-row",attrs:{span:16}},[r("a-form-item",[r("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["captcha",{rules:[{required:!0,message:"请输入验证码"}],validateTrigger:"blur"}],expression:"['captcha', {rules: [{ required: true, message: '请输入验证码' }], validateTrigger: 'blur'}]"}],attrs:{size:"large",type:"text",placeholder:"验证码"}},[r("a-icon",{style:{color:"rgba(0,0,0,.25)"},attrs:{slot:"prefix",type:"mail"},slot:"prefix"})],1)],1)],1),r("a-col",{staticClass:"gutter-row",attrs:{span:8}},[r("a-button",{staticClass:"getCaptcha",attrs:{size:"large",disabled:e.state.smsSendBtn},domProps:{textContent:e._s(e.state.smsSendBtn?e.state.time+" s":"获取验证码")},on:{click:function(t){return t.stopPropagation(),t.preventDefault(),e.getCaptcha(t)}}})],1)],1),r("a-form-item",[r("a-button",{staticClass:"register-button",attrs:{size:"large",type:"primary",htmlType:"submit",loading:e.registerBtn,disabled:e.registerBtn},on:{click:function(t){return t.stopPropagation(),t.preventDefault(),e.handleSubmit(t)}}},[e._v("注册 ")]),r("router-link",{staticClass:"login",attrs:{to:{name:"login"}}},[e._v("使用已有账户登录")])],1)],1)],1)},s=[function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("h3",[r("span",[e._v("注册")])])}],n=(r("a4d3"),r("4de4"),r("e439"),r("dbb4"),r("b64b"),r("498a"),r("159b"),r("ade3")),o=r("ac0d"),i=r("7ded");function l(e,t){var r=Object.keys(e);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);t&&(a=a.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),r.push.apply(r,a)}return r}function c(e){for(var t=1;t<arguments.length;t++){var r=null!=arguments[t]?arguments[t]:{};t%2?l(Object(r),!0).forEach((function(t){Object(n["a"])(e,t,r[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(r)):l(Object(r)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(r,t))}))}return e}var u={0:"低",1:"低",2:"中",3:"强"},d={0:"error",1:"error",2:"warning",3:"success"},p={0:"#ff0000",1:"#ff0000",2:"#ff7e05",3:"#52c41a"},f={name:"Register",components:{},mixins:[o["c"]],data:function(){return{form:this.$form.createForm(this),state:{time:60,smsSendBtn:!1,passwordLevel:0,passwordLevelChecked:!1,percent:10,progressColor:"#FF0000"},registerBtn:!1}},computed:{passwordLevelClass:function(){return d[this.state.passwordLevel]},passwordLevelName:function(){return u[this.state.passwordLevel]},passwordLevelColor:function(){return p[this.state.passwordLevel]}},methods:{handlePasswordLevel:function(e,t,r){var a=0;/[0-9]/.test(t)&&a++,/[a-zA-Z]/.test(t)&&a++,/[^0-9a-zA-Z_]/.test(t)&&a++,this.state.passwordLevel=a,this.state.percent=30*a,a>=2?(a>=3&&(this.state.percent=100),r()):(0===a&&(this.state.percent=10),r(new Error("密码强度不够")))},handlePasswordCheck:function(e,t,r){var a=this.form.getFieldValue("password");void 0===t&&r(new Error("请输入密码")),t&&a&&t.trim()!==a.trim()&&r(new Error("两次密码不一致")),r()},handlePhoneCheck:function(e,t,r){r()},handlePasswordInputClick:function(){this.isMobile()?this.state.passwordLevelChecked=!1:this.state.passwordLevelChecked=!0},handleSubmit:function(){var e=this.form.validateFields,t=this.state,r=this.$router;e({force:!0},(function(e,a){e||(t.passwordLevelChecked=!1,r.push({name:"registerResult",params:c({},a)}))}))},getCaptcha:function(e){var t=this;e.preventDefault();var r=this.form.validateFields,a=this.state,s=this.$message,n=this.$notification;r(["mobile"],{force:!0},(function(e,r){if(!e){a.smsSendBtn=!0;var o=window.setInterval((function(){a.time--<=0&&(a.time=60,a.smsSendBtn=!1,window.clearInterval(o))}),1e3),l=s.loading("验证码发送中..",0);Object(i["b"])({mobile:r.mobile}).then((function(e){setTimeout(l,2500),n["success"]({message:"提示",description:"验证码获取成功，您的验证码为："+e.result.captcha,duration:8})})).catch((function(e){setTimeout(l,1),clearInterval(o),a.time=60,a.smsSendBtn=!1,t.requestFailed(e)}))}}))},requestFailed:function(e){this.$notification["error"]({message:"错误",description:((e.response||{}).data||{}).message||"请求出现错误，请稍后再试",duration:4}),this.registerBtn=!1}},watch:{"state.passwordLevel":function(e){}}},g=f,m=(r("5d18"),r("88c2"),r("2877")),h=Object(m["a"])(g,a,s,!1,null,"54d9a6c2",null);t["default"]=h.exports},"5d18":function(e,t,r){"use strict";var a=r("eeab"),s=r.n(a);s.a},6821:function(e,t,r){(function(){var t=r("00d8"),a=r("9a634").utf8,s=r("8349"),n=r("9a634").bin,o=function(e,r){e.constructor==String?e=r&&"binary"===r.encoding?n.stringToBytes(e):a.stringToBytes(e):s(e)?e=Array.prototype.slice.call(e,0):Array.isArray(e)||(e=e.toString());for(var i=t.bytesToWords(e),l=8*e.length,c=1732584193,u=-271733879,d=-1732584194,p=271733878,f=0;f<i.length;f++)i[f]=16711935&(i[f]<<8|i[f]>>>24)|4278255360&(i[f]<<24|i[f]>>>8);i[l>>>5]|=128<<l%32,i[14+(l+64>>>9<<4)]=l;var g=o._ff,m=o._gg,h=o._hh,v=o._ii;for(f=0;f<i.length;f+=16){var b=c,y=u,w=d,C=p;c=g(c,u,d,p,i[f+0],7,-680876936),p=g(p,c,u,d,i[f+1],12,-389564586),d=g(d,p,c,u,i[f+2],17,606105819),u=g(u,d,p,c,i[f+3],22,-1044525330),c=g(c,u,d,p,i[f+4],7,-176418897),p=g(p,c,u,d,i[f+5],12,1200080426),d=g(d,p,c,u,i[f+6],17,-1473231341),u=g(u,d,p,c,i[f+7],22,-45705983),c=g(c,u,d,p,i[f+8],7,1770035416),p=g(p,c,u,d,i[f+9],12,-1958414417),d=g(d,p,c,u,i[f+10],17,-42063),u=g(u,d,p,c,i[f+11],22,-1990404162),c=g(c,u,d,p,i[f+12],7,1804603682),p=g(p,c,u,d,i[f+13],12,-40341101),d=g(d,p,c,u,i[f+14],17,-1502002290),u=g(u,d,p,c,i[f+15],22,1236535329),c=m(c,u,d,p,i[f+1],5,-165796510),p=m(p,c,u,d,i[f+6],9,-1069501632),d=m(d,p,c,u,i[f+11],14,643717713),u=m(u,d,p,c,i[f+0],20,-373897302),c=m(c,u,d,p,i[f+5],5,-701558691),p=m(p,c,u,d,i[f+10],9,38016083),d=m(d,p,c,u,i[f+15],14,-660478335),u=m(u,d,p,c,i[f+4],20,-405537848),c=m(c,u,d,p,i[f+9],5,568446438),p=m(p,c,u,d,i[f+14],9,-1019803690),d=m(d,p,c,u,i[f+3],14,-187363961),u=m(u,d,p,c,i[f+8],20,1163531501),c=m(c,u,d,p,i[f+13],5,-1444681467),p=m(p,c,u,d,i[f+2],9,-51403784),d=m(d,p,c,u,i[f+7],14,1735328473),u=m(u,d,p,c,i[f+12],20,-1926607734),c=h(c,u,d,p,i[f+5],4,-378558),p=h(p,c,u,d,i[f+8],11,-2022574463),d=h(d,p,c,u,i[f+11],16,1839030562),u=h(u,d,p,c,i[f+14],23,-35309556),c=h(c,u,d,p,i[f+1],4,-1530992060),p=h(p,c,u,d,i[f+4],11,1272893353),d=h(d,p,c,u,i[f+7],16,-155497632),u=h(u,d,p,c,i[f+10],23,-1094730640),c=h(c,u,d,p,i[f+13],4,681279174),p=h(p,c,u,d,i[f+0],11,-358537222),d=h(d,p,c,u,i[f+3],16,-722521979),u=h(u,d,p,c,i[f+6],23,76029189),c=h(c,u,d,p,i[f+9],4,-640364487),p=h(p,c,u,d,i[f+12],11,-421815835),d=h(d,p,c,u,i[f+15],16,530742520),u=h(u,d,p,c,i[f+2],23,-995338651),c=v(c,u,d,p,i[f+0],6,-198630844),p=v(p,c,u,d,i[f+7],10,1126891415),d=v(d,p,c,u,i[f+14],15,-1416354905),u=v(u,d,p,c,i[f+5],21,-57434055),c=v(c,u,d,p,i[f+12],6,1700485571),p=v(p,c,u,d,i[f+3],10,-1894986606),d=v(d,p,c,u,i[f+10],15,-1051523),u=v(u,d,p,c,i[f+1],21,-2054922799),c=v(c,u,d,p,i[f+8],6,1873313359),p=v(p,c,u,d,i[f+15],10,-30611744),d=v(d,p,c,u,i[f+6],15,-1560198380),u=v(u,d,p,c,i[f+13],21,1309151649),c=v(c,u,d,p,i[f+4],6,-145523070),p=v(p,c,u,d,i[f+11],10,-1120210379),d=v(d,p,c,u,i[f+2],15,718787259),u=v(u,d,p,c,i[f+9],21,-343485551),c=c+b>>>0,u=u+y>>>0,d=d+w>>>0,p=p+C>>>0}return t.endian([c,u,d,p])};o._ff=function(e,t,r,a,s,n,o){var i=e+(t&r|~t&a)+(s>>>0)+o;return(i<<n|i>>>32-n)+t},o._gg=function(e,t,r,a,s,n,o){var i=e+(t&a|r&~a)+(s>>>0)+o;return(i<<n|i>>>32-n)+t},o._hh=function(e,t,r,a,s,n,o){var i=e+(t^r^a)+(s>>>0)+o;return(i<<n|i>>>32-n)+t},o._ii=function(e,t,r,a,s,n,o){var i=e+(r^(t|~a))+(s>>>0)+o;return(i<<n|i>>>32-n)+t},o._blocksize=16,o._digestsize=16,e.exports=function(e,r){if(void 0===e||null===e)throw new Error("Illegal argument "+e);var a=t.wordsToBytes(o(e,r));return r&&r.asBytes?a:r&&r.asString?n.bytesToString(a):t.bytesToHex(a)}})()},8349:function(e,t){function r(e){return!!e.constructor&&"function"===typeof e.constructor.isBuffer&&e.constructor.isBuffer(e)}function a(e){return"function"===typeof e.readFloatLE&&"function"===typeof e.slice&&r(e.slice(0,0))}
/*!
 * Determine if an object is a Buffer
 *
 * @author   Feross Aboukhadijeh <https://feross.org>
 * @license  MIT
 */
e.exports=function(e){return null!=e&&(r(e)||a(e)||!!e._isBuffer)}},"88c2":function(e,t,r){"use strict";var a=r("d88b"),s=r.n(a);s.a},"90b4":function(e,t,r){},"9a634":function(e,t){var r={utf8:{stringToBytes:function(e){return r.bin.stringToBytes(unescape(encodeURIComponent(e)))},bytesToString:function(e){return decodeURIComponent(escape(r.bin.bytesToString(e)))}},bin:{stringToBytes:function(e){for(var t=[],r=0;r<e.length;r++)t.push(255&e.charCodeAt(r));return t},bytesToString:function(e){for(var t=[],r=0;r<e.length;r++)t.push(String.fromCharCode(e[r]));return t.join("")}}};e.exports=r},ac2a:function(e,t,r){"use strict";r.r(t);var a=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{staticClass:"main"},[r("a-form",{ref:"formLogin",staticClass:"user-layout-login",attrs:{id:"formLogin",form:e.form},on:{submit:e.handleSubmit}},[r("a-tabs",{attrs:{activeKey:e.customActiveKey,tabBarStyle:{textAlign:"center",borderBottom:"unset"}},on:{change:e.handleTabClick}},[r("a-tab-pane",{key:"tab1",attrs:{tab:"账号密码登录"}},[e.isLoginError?r("a-alert",{staticStyle:{"margin-bottom":"24px"},attrs:{type:"error",showIcon:"",message:"账户或密码错误"}}):e._e(),r("a-form-item",[r("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["loginID",{rules:[{required:!0,message:"请输入帐户名或邮箱地址"},{validator:e.handleUsernameOrEmail}],validateTrigger:"change"}],expression:"[\n              'loginID',\n              {\n                rules: [{ required: true, message: '请输入帐户名或邮箱地址' }, { validator: handleUsernameOrEmail }],\n                validateTrigger: 'change'\n              }\n            ]"}],attrs:{size:"large",type:"text",placeholder:"账户: supporter01"}},[r("a-icon",{style:{color:"rgba(0,0,0,.25)"},attrs:{slot:"prefix",type:"user"},slot:"prefix"})],1)],1),r("a-form-item",[r("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["password",{rules:[{required:!0,message:"请输入密码"}],validateTrigger:"blur"}],expression:"[\n              'password',\n              { rules: [{ required: true, message: '请输入密码' }], validateTrigger: 'blur' }\n            ]"}],attrs:{size:"large",type:"password",autocomplete:"false",placeholder:"密码: 123456"}},[r("a-icon",{style:{color:"rgba(0,0,0,.25)"},attrs:{slot:"prefix",type:"lock"},slot:"prefix"})],1)],1)],1)],1),r("a-form-item",[r("a-checkbox",{directives:[{name:"decorator",rawName:"v-decorator",value:["rememberMe"],expression:"['rememberMe']"}]},[e._v("自动登录")]),r("router-link",{staticClass:"forge-password",staticStyle:{float:"right"},attrs:{to:{name:"recover",params:{user:"aaa"}}}},[e._v("忘记密码")])],1),r("a-form-item",{staticStyle:{"margin-top":"24px"}},[r("a-button",{staticClass:"login-button",attrs:{size:"large",type:"primary",htmlType:"submit",loading:e.state.loginBtn,disabled:e.state.loginBtn}},[e._v("确定")])],1)],1),e.requiredTwoStepCaptcha?r("two-step-captcha",{attrs:{visible:e.stepCaptchaVisible},on:{success:e.stepCaptchaSuccess,cancel:e.stepCaptchaCancel}}):e._e()],1)},s=[],n=(r("a4d3"),r("4de4"),r("e439"),r("dbb4"),r("b64b"),r("d3b7"),r("159b"),r("ade3")),o=(r("6821"),function(){var e=this,t=this,r=t.$createElement,a=t._self._c||r;return a("a-modal",{attrs:{centered:"",maskClosable:!1},on:{cancel:t.handleCancel},model:{value:t.visible,callback:function(e){t.visible=e},expression:"visible"}},[a("div",{style:{textAlign:"center"},attrs:{slot:"title"},slot:"title"},[t._v("两步验证")]),a("template",{slot:"footer"},[a("div",{style:{textAlign:"center"}},[a("a-button",{key:"back",on:{click:t.handleCancel}},[t._v("返回")]),a("a-button",{key:"submit",attrs:{type:"primary",loading:t.stepLoading},on:{click:t.handleStepOk}},[t._v(" 继续 ")])],1)]),a("a-spin",{attrs:{spinning:t.stepLoading}},[a("a-form",{attrs:{layout:"vertical","auto-form-create":function(t){e.form=t}}},[a("div",{staticClass:"step-form-wrapper"},[t.stepLoading?a("p",{staticStyle:{"text-align":"center"}},[t._v("正在验证.."),a("br"),t._v("请稍后")]):a("p",{staticStyle:{"text-align":"center"}},[t._v("请在手机中打开 Google Authenticator 或两步验证 APP"),a("br"),t._v("输入 6 位动态码")]),a("a-form-item",{style:{textAlign:"center"},attrs:{hasFeedback:"",fieldDecoratorId:"stepCode",fieldDecoratorOptions:{rules:[{required:!0,message:"请输入 6 位动态码!",pattern:/^\d{6}$/,len:6}]}}},[a("a-input",{style:{textAlign:"center"},attrs:{placeholder:"000000"},nativeOn:{keyup:function(e){return!e.type.indexOf("key")&&t._k(e.keyCode,"enter",13,e.key,"Enter")?null:t.handleStepOk(e)}}})],1),a("p",{staticStyle:{"text-align":"center"}},[a("a",{on:{click:t.onForgeStepCode}},[t._v("遗失手机?")])])],1)])],1)],2)}),i=[],l={props:{visible:{type:Boolean,default:!1}},data:function(){return{stepLoading:!1,form:null}},methods:{handleStepOk:function(){var e=this,t=this;this.stepLoading=!0,this.form.validateFields((function(r,a){r?(e.stepLoading=!1,e.$emit("error",{err:r})):setTimeout((function(){t.stepLoading=!1,t.$emit("success",{values:a})}),2e3)}))},handleCancel:function(){this.visible=!1,this.$emit("cancel")},onForgeStepCode:function(){}}},c=l,u=(r("edd4"),r("2877")),d=Object(u["a"])(c,o,i,!1,null,"4a462ef6",null),p=d.exports,f=r("5880"),g=r("ca00");function m(e,t){var r=Object.keys(e);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);t&&(a=a.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),r.push.apply(r,a)}return r}function h(e){for(var t=1;t<arguments.length;t++){var r=null!=arguments[t]?arguments[t]:{};t%2?m(Object(r),!0).forEach((function(t){Object(n["a"])(e,t,r[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(r)):m(Object(r)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(r,t))}))}return e}var v={components:{TwoStepCaptcha:p},data:function(){return{customActiveKey:"tab1",loginBtn:!1,loginType:0,isLoginError:!1,requiredTwoStepCaptcha:!1,stepCaptchaVisible:!1,form:this.$form.createForm(this),state:{time:60,loginBtn:!1,loginType:0,smsSendBtn:!1}}},methods:h({},Object(f["mapActions"])(["Login"]),{handleUsernameOrEmail:function(e,t,r){var a=this.state,s=/^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$/;s.test(t)?a.loginType=0:a.loginType=1,r()},handleTabClick:function(e){this.customActiveKey=e},handleSubmit:function(e){var t=this;e.preventDefault();var r=this.form.validateFields,a=this.state,s=this.customActiveKey,n=this.Login;a.loginBtn=!0;var o="tab1"===s?["loginID","password"]:["mobile","captcha"];r(o,{force:!0},(function(e,r){if(e)setTimeout((function(){a.loginBtn=!1}),600);else{var s=h({},r);delete s.loginID,s[a.loginType?"loginID":"email"]=r.loginID,s.password=r.password,n(s).then((function(e){return t.loginSuccess(e)})).catch((function(e){return t.requestFailed(e)})).finally((function(){a.loginBtn=!1}))}}))},loginSuccess:function(e){var t=this;this.$store.state.userInfo=e,this.$router.push({path:"/"}),setTimeout((function(){t.$notification.success({message:"欢迎",description:"".concat(Object(g["a"])(),"，欢迎回来")})}),1e3),this.isLoginError=!1},requestFailed:function(e){this.isLoginError=!0,this.$notification["error"]({message:"错误",description:((e.response||{}).data||{}).message||"请求出现错误，请稍后再试",duration:4})}})},b=v,y=(r("ac72"),Object(u["a"])(b,a,s,!1,null,"1a7700d2",null));t["default"]=y.exports},ac72:function(e,t,r){"use strict";var a=r("e18a"),s=r.n(a);s.a},d88b:function(e,t,r){},e18a:function(e,t,r){},edd4:function(e,t,r){"use strict";var a=r("90b4"),s=r.n(a);s.a},eeab:function(e,t,r){}}]);