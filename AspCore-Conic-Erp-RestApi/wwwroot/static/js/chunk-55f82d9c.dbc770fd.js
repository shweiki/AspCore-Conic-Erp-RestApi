(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-55f82d9c"],{"0797":function(e,t,r){"use strict";r.r(t);var o=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{staticClass:"app-container"},[r("el-card",{staticClass:"box-card"},[r("div",{staticClass:"clearfix",attrs:{slot:"header"},slot:"header"},[r("el-button",{staticStyle:{float:"left"},attrs:{type:"success",icon:"el-icon-plus"},on:{click:function(t){return e.handleCreate()}}},[e._v("إضافة مستخدم")]),e._v(" "),r("span",[e._v("بيانات المستخدمين و صلاحياتهم")])],1),e._v(" "),r("el-table",{directives:[{name:"loading",rawName:"v-loading",value:e.loading,expression:"loading"}],staticStyle:{width:"100%"},attrs:{data:e.tableData.filter((function(t){return!e.search||t.UserName.toLowerCase().includes(e.search.toLowerCase())})),"max-height":"750"}},[r("el-table-column",{attrs:{prop:"avatar",width:"120"},scopedSlots:e._u([{key:"header",fn:function(t){return[r("el-button",{attrs:{type:"primary",icon:"el-icon-refresh"},on:{click:function(t){return e.getdata()}}})]}},{key:"default",fn:function(e){return[r("pan-thumb",{staticStyle:{cursor:"pointer"},attrs:{width:"90px",height:"90px",image:e.row.avatar}},[r("div",{staticStyle:{padding:"25px 20px 20px 20px"}})])]}}])}),e._v(" "),r("el-table-column",{attrs:{prop:"UserName",width:"120"},scopedSlots:e._u([{key:"header",fn:function(t){return[r("el-input",{attrs:{placeholder:e.$t("Permission.UserName")},model:{value:e.search,callback:function(t){e.search=t},expression:"search"}})]}}])}),e._v(" "),r("el-table-column",{attrs:{label:"Email",prop:"Email"}}),e._v(" "),r("el-table-column",{attrs:{label:"Phone Number",prop:"PhoneNumber"}}),e._v(" "),r("el-table-column",{attrs:{align:"left"},scopedSlots:e._u([{key:"default",fn:function(t){return[r("add-user-router",{attrs:{UserID:t.row.Id,Router:t.row.router}}),e._v(" "),e._l(t.row.Roles,(function(o){return r("el-tag",{key:o.Name,attrs:{effect:"plain",closable:"","disable-transitions":!1},on:{close:function(r){return e.RemoveRole(t.row.UserName,o.Name)}}},[e._v(e._s(o.Name))])})),e._v(" "),r("el-button",{attrs:{type:"success",icon:"el-icon-plus",size:"mini"},on:{click:function(t){e.dialogAddRoleVisible=!0}}})]}}])})],1)],1),e._v(" "),r("el-dialog",{staticStyle:{"margin-top":"-13vh"},attrs:{width:"65%","show-close":!1,title:e.textMapForm[e.dialogFormStatus],visible:e.dialogFormVisible},on:{"update:visible":function(t){e.dialogFormVisible=t}}},[r("el-form",{ref:"dataForm",staticClass:"demo-form-inline",attrs:{model:e.tempForm,rules:e.rulesForm}},[r("el-form-item",{attrs:{label:"User name",prop:"UserName"}},[r("el-input",{attrs:{type:"text"},model:{value:e.tempForm.UserName,callback:function(t){e.$set(e.tempForm,"UserName",t)},expression:"tempForm.UserName"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"Number Phone",prop:"PhoneNumber",rules:[{required:!0,message:"Please input Number Phone ",trigger:"blur"}]}},[r("el-input",{attrs:{type:"text"},model:{value:e.tempForm.PhoneNumber,callback:function(t){e.$set(e.tempForm,"PhoneNumber",t)},expression:"tempForm.PhoneNumber"}})],1),e._v(" "),r("el-form-item",{attrs:{prop:"Email",label:"Email",rules:[{required:!0,message:"Please input email address",trigger:"blur"},{type:"email",message:"Please input correct email address",trigger:["blur","change"]}]}},[r("el-input",{model:{value:e.tempForm.Email,callback:function(t){e.$set(e.tempForm,"Email",t)},expression:"tempForm.Email"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"Password",prop:"Password"}},[r("el-input",{attrs:{type:"password",autocomplete:"off"},model:{value:e.tempForm.Password,callback:function(t){e.$set(e.tempForm,"Password",t)},expression:"tempForm.Password"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"Confirm",prop:"ConfirmPassword"}},[r("el-input",{attrs:{type:"password",autocomplete:"off"},model:{value:e.tempForm.ConfirmPassword,callback:function(t){e.$set(e.tempForm,"ConfirmPassword",t)},expression:"tempForm.ConfirmPassword"}})],1)],1),e._v(" "),r("div",{staticClass:"dialog-footer",attrs:{slot:"footer"},slot:"footer"},[r("el-button",{on:{click:function(t){e.dialogFormVisible=!1}}},[e._v(e._s(e.$t("AddVendors.Cancel")))]),e._v(" "),r("el-button",{attrs:{type:"primary"},on:{click:function(t){"create"===e.dialogFormStatus?e.createData():e.updateData()}}},[e._v(e._s(e.$t("AddVendors.Save")))])],1)],1),e._v(" "),r("el-dialog",{staticStyle:{"margin-top":"-13vh"},attrs:{"show-close":!1,title:"اضافة صلاحية",visible:e.dialogAddRoleVisible},on:{"update:visible":function(t){e.dialogAddRoleVisible=t}}},[r("el-select",{attrs:{placeholder:"المستخدم"},model:{value:e.UserName,callback:function(t){e.UserName=t},expression:"UserName"}},e._l(e.tableData,(function(e){return r("el-option",{key:e.Id,attrs:{label:e.UserName,value:e.UserName}})})),1),e._v(" "),r("el-select",{attrs:{placeholder:" صلاحية"},model:{value:e.RoleName,callback:function(t){e.RoleName=t},expression:"RoleName"}},e._l(e.Roles,(function(e){return r("el-option",{key:e.Id,attrs:{label:e.Name,value:e.Name}})})),1),e._v(" "),r("div",{staticClass:"dialog-footer",attrs:{slot:"footer"},slot:"footer"},[r("el-button",{attrs:{type:"success"},on:{click:function(t){return e.AddRole()}}},[e._v("Add")])],1)],1),e._v(" "),r("role")],1)},n=[],a=r("9fb8"),i=r("b7e2"),l=r("4328"),s=r.n(l),c=(r("4360"),r("a18c"));function u(){return Object(i["a"])({url:"/Role/GetRoles",method:"get"})}function d(e){return Object(i["a"])({url:"/Role/AddRole",method:"post",data:s.a.stringify(e)})}function m(e){return Object(i["a"])({url:"/Role/AddUserRouter",method:"post",data:s.a.stringify(e)})}function f(e){return Object(i["a"])({url:"/Role/Edit",method:"post",data:s.a.stringify(e)})}function h(e){return Object(i["a"])({url:"/Role/DeleteRole",method:"post",data:s.a.stringify(e)})}function p(){return c["a"]}var b=r("3cbc"),g=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{staticClass:"app-container"},[r("el-button",{attrs:{type:"primary"},on:{click:e.handleAddRole}},[e._v("New Role")]),e._v(" "),r("el-table",{staticStyle:{width:"100%","margin-top":"30px"},attrs:{data:e.rolesList,border:""}},[r("el-table-column",{attrs:{align:"center",label:"Role Id",width:"220"},scopedSlots:e._u([{key:"default",fn:function(t){return[e._v("\n        "+e._s(t.row.Id)+"\n      ")]}}])}),e._v(" "),r("el-table-column",{attrs:{align:"center",label:"Role Name",width:"220"},scopedSlots:e._u([{key:"default",fn:function(t){return[e._v("\n        "+e._s(t.row.Name)+"\n      ")]}}])}),e._v(" "),r("el-table-column",{attrs:{align:"header-center",label:"NormalizedName"},scopedSlots:e._u([{key:"default",fn:function(t){return[e._v("\n        "+e._s(t.row.NormalizedName)+"\n      ")]}}])}),e._v(" "),r("el-table-column",{attrs:{align:"center",label:"Operations"},scopedSlots:e._u([{key:"default",fn:function(t){return[r("el-button",{attrs:{type:"primary",size:"small"},on:{click:function(r){return e.handleEdit(t)}}},[e._v("Edit")]),e._v(" "),r("el-button",{attrs:{type:"danger",size:"small"},on:{click:function(r){return e.handleDelete(t)}}},[e._v("Delete")])]}}])})],1),e._v(" "),r("el-dialog",{attrs:{visible:e.dialogVisible,title:"edit"===e.dialogType?"Edit Role":"New Role"},on:{"update:visible":function(t){e.dialogVisible=t}}},[r("el-form",{attrs:{model:e.role,"label-width":"80px","label-position":"left"}},[r("el-form-item",{attrs:{label:"Name"}},[r("el-input",{attrs:{placeholder:"Role Name"},model:{value:e.role.Name,callback:function(t){e.$set(e.role,"Name",t)},expression:"role.Name"}})],1),e._v(" "),r("el-form-item",{attrs:{label:"Desc"}},[r("el-input",{attrs:{autosize:{minRows:2,maxRows:4},type:"textarea",placeholder:"Role Description"},model:{value:e.role.NormalizedName,callback:function(t){e.$set(e.role,"NormalizedName",t)},expression:"role.NormalizedName"}})],1)],1),e._v(" "),r("div",{staticStyle:{"text-align":"right"}},[r("el-button",{attrs:{type:"danger"},on:{click:function(t){e.dialogVisible=!1}}},[e._v("Cancel")]),e._v(" "),r("el-button",{attrs:{type:"primary"},on:{click:e.confirmRole}},[e._v("Confirm")])],1)],1)],1)},v=[],y=(r("96cf"),r("1da1")),w=r("df7c"),N=r.n(w),R=r("ed08"),_={Id:"",Name:"",NormalizedName:""},k={data:function(){return{role:Object.assign({},_),rolesList:[],dialogVisible:!1,dialogType:"new",checkStrictly:!1,defaultProps:{children:"children",label:"title"}}},created:function(){this.GetRoles()},methods:{GetRoles:function(){var e=Object(y["a"])(regeneratorRuntime.mark((function e(){var t;return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:return e.next=2,u();case 2:t=e.sent,this.rolesList=t;case 4:case"end":return e.stop()}}),e,this)})));function t(){return e.apply(this,arguments)}return t}(),handleAddRole:function(){this.role=Object.assign({},_),this.dialogType="new",this.dialogVisible=!0},handleEdit:function(e){var t=this;this.dialogType="edit",this.dialogVisible=!0,this.checkStrictly=!0,this.role=Object(R["b"])(e.row),this.$nextTick((function(){t.checkStrictly=!1}))},handleDelete:function(e){var t=this,r=e.$index,o=e.row;this.$confirm("Confirm to remove the role?","Warning",{confirmButtonText:"Confirm",cancelButtonText:"Cancel",type:"warning"}).then(Object(y["a"])(regeneratorRuntime.mark((function e(){return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:return e.next=2,h(o);case 2:t.rolesList.splice(r,1),t.$message({type:"success",message:"Delete succed!"});case 4:case"end":return e.stop()}}),e)})))).catch((function(e){console.error(e)}))},confirmRole:function(){var e=Object(y["a"])(regeneratorRuntime.mark((function e(){var t,r,o,n,a,i,l;return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:if(t="edit"===this.dialogType,!t){e.next=14;break}return e.next=4,f(this.role);case 4:r=0;case 5:if(!(r<this.rolesList.length)){e.next=12;break}if(this.rolesList[r].Id!==this.role.Id){e.next=9;break}return this.rolesList.splice(r,1,Object.assign({},this.role)),e.abrupt("break",12);case 9:r++,e.next=5;break;case 12:e.next=19;break;case 14:return e.next=16,d(this.role);case 16:o=e.sent,this.role.Id=o.Id,this.rolesList.push(this.role);case 19:n=this.role,a=n.NormalizedName,i=n.Id,l=n.Name,this.dialogVisible=!1,this.$notify({title:"Success",dangerouslyUseHTMLString:!0,message:"\n            <div>Role Id: ".concat(i,"</div>\n            <div>Role Name: ").concat(l,"</div>\n            <div>NormalizedName: ").concat(a,"</div>\n          "),type:"success"});case 22:case"end":return e.stop()}}),e,this)})));function t(){return e.apply(this,arguments)}return t}()}},x=k,F=(r("c426"),r("2877")),P=Object(F["a"])(x,g,v,!1,null,"360f18b7",null),O=P.exports,S=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",[r("el-button",{staticStyle:{width:"100px"},attrs:{type:"warning",icon:"el-icon-s-platform"},on:{click:function(t){return e.OpenDialog()}}},[e._v("\n    صلاحيات")]),e._v(" "),r("el-dialog",{staticStyle:{"margin-top":"-13vh"},attrs:{title:"صلاحيات توجه",visible:e.dialogVisible},on:{"update:visible":function(t){e.dialogVisible=t}}},[r("el-form",{attrs:{"label-width":"80px","label-position":"left"}},[r("el-form-item",{attrs:{label:"Menus"}},[r("el-tree",{ref:"tree",staticClass:"permission-tree",attrs:{"check-strictly":e.checkStrictly,data:e.routesData,props:e.defaultProps,"show-checkbox":"","node-key":"path"}})],1)],1),e._v(" "),r("div",{staticStyle:{"text-align":"right"}},[r("el-button",{attrs:{type:"danger"},on:{click:function(t){e.dialogVisible=!1}}},[e._v("Cancel")]),e._v(" "),r("el-button",{attrs:{type:"primary"},on:{click:e.confirmRouter}},[e._v("Confirm")])],1)],1)],1)},C=[],j=(r("8e6e"),r("456d"),r("ac4d"),r("8a81"),r("5df3"),r("1c4c"),r("7f7f"),r("6b54"),r("ade3")),U=(r("6762"),r("2fdb"),r("2909"));r("ac6a");function $(e,t){var r=Object.keys(e);if(Object.getOwnPropertySymbols){var o=Object.getOwnPropertySymbols(e);t&&(o=o.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),r.push.apply(r,o)}return r}function V(e){for(var t=1;t<arguments.length;t++){var r=null!=arguments[t]?arguments[t]:{};t%2?$(Object(r),!0).forEach((function(t){Object(j["a"])(e,t,r[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(r)):$(Object(r)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(r,t))}))}return e}function D(e,t){var r;if("undefined"===typeof Symbol||null==e[Symbol.iterator]){if(Array.isArray(e)||(r=E(e))||t&&e&&"number"===typeof e.length){r&&(e=r);var o=0,n=function(){};return{s:n,n:function(){return o>=e.length?{done:!0}:{done:!1,value:e[o++]}},e:function(e){throw e},f:n}}throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.")}var a,i=!0,l=!1;return{s:function(){r=e[Symbol.iterator]()},n:function(){var e=r.next();return i=e.done,e},e:function(e){l=!0,a=e},f:function(){try{i||null==r.return||r.return()}finally{if(l)throw a}}}}function E(e,t){if(e){if("string"===typeof e)return A(e,t);var r=Object.prototype.toString.call(e).slice(8,-1);return"Object"===r&&e.constructor&&(r=e.constructor.name),"Map"===r||"Set"===r?Array.from(e):"Arguments"===r||/^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(r)?A(e,t):void 0}}function A(e,t){(null==t||t>e.length)&&(t=e.length);for(var r=0,o=new Array(t);r<t;r++)o[r]=e[r];return o}var I={props:{UserID:{type:String,default:""},Router:{type:String,default:""}},data:function(){return{routes:[],dialogVisible:!1,checkStrictly:!1,defaultProps:{children:"children",label:"title"}}},computed:{routesData:function(){return this.routes}},created:function(){this.getRoutes()},methods:{OpenDialog:function(){var e=this;this.checkStrictly=!0,this.$nextTick((function(){console.log(e.Router),e.$refs.tree.setCheckedKeys(JSON.parse(e.Router)),e.checkStrictly=!1})),this.dialogVisible=!0},getRoutes:function(){var e=Object(y["a"])(regeneratorRuntime.mark((function e(){var t;return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:return e.next=2,p();case 2:t=e.sent,this.serviceRoutes=t,this.routes=this.generateRoutes(t);case 5:case"end":return e.stop()}}),e,this)})));function t(){return e.apply(this,arguments)}return t}(),confirmRouter:function(){var e=this;m({UserID:this.UserID,Router:JSON.stringify(this.$refs.tree.getCheckedKeys())}).then((function(t){e.dialogVisible=!1,e.$notify({title:"تم ",message:"تم الإضافة بنجاح",type:"success",duration:2e3})})).catch((function(e){console.log(e)}))},generateRoutes:function(e){var t,r=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"/",o=[],n=D(e);try{for(n.s();!(t=n.n()).done;){var a=t.value;if(!a.hidden){var i=this.onlyOneShowingChild(a.children,a);a.children&&i&&!a.alwaysShow&&(a=i);var l={path:N.a.resolve(r,a.path),title:a.meta&&a.meta.title};a.children&&(l.children=this.generateRoutes(a.children,l.path)),o.push(l)}}}catch(s){n.e(s)}finally{n.f()}return o},generateArr:function(e){var t=this,r=[];return e.forEach((function(e){if(r.push(e),e.children){var o=t.generateArr(e.children);o.length>0&&(r=[].concat(Object(U["a"])(r),Object(U["a"])(o)))}})),r},generateTree:function(e){var t,r=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"/",o=arguments.length>2?arguments[2]:void 0,n=[],a=D(e);try{for(a.s();!(t=a.n()).done;){var i=t.value,l=N.a.resolve(r,i.path);i.children&&(i.children=this.generateTree(i.children,l,o)),(o.includes(l)||i.children&&i.children.length>=1)&&n.push(i)}}catch(s){a.e(s)}finally{a.f()}return n},onlyOneShowingChild:function(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:[],t=arguments.length>1?arguments[1]:void 0,r=null,o=e.filter((function(e){return!e.hidden}));return 1===o.length?(r=o[0],r.path=N.a.resolve(t.path,r.path),r):0===o.length&&(r=V(V({},t),{},{path:"",noShowingChildren:!0}),r)}}},T=I,z=(r("1f8d"),Object(F["a"])(T,S,C,!1,null,"8a6ae814",null)),L=z.exports,M={name:"permission",components:{PanThumb:b["a"],Role:O,AddUserRouter:L},data:function(){var e=this,t=function(t,r,o){""===r?o(new Error("Please input the password")):(""!==e.tempForm.ConfirmPassword&&e.$refs["dataForm"].validateField("ConfirmPassword"),o())},r=function(t,r,o){""===r?o(new Error("Please input the password again")):r!==e.tempForm.Password?o(new Error("Two inputs don't match!")):o()};return{loading:!0,dialogFormVisible:!1,dialogAddRoleVisible:!1,dialogFormStatus:"",search:"",RoleName:"",UserName:"",tableData:[],Roles:[],textMapForm:{update:"تعديل",create:"إضافة"},tempForm:{UserName:"",Email:"",Password:"",PhoneNumber:"",ConfirmPassword:""},rulesForm:{UserName:[{required:!0,message:"يجب إدخال إسم ",trigger:"blur"},{minlength:3,maxlength:50,message:"الرجاء إدخال إسم لا يقل عن 3 أحرف و لا يزيد عن 50 حرف",trigger:"blur"}],Password:[{validator:t,trigger:"blur"}],ConfirmPassword:[{validator:r,trigger:"blur"}]}}},created:function(){this.getdata()},methods:{getdata:function(){var e=this;this.loading=!0,Object(a["d"])().then((function(t){console.log(t),e.tableData=t,u().then((function(t){console.log(t),e.Roles=t,e.loading=!1})).catch((function(e){console.log(e)}))})).catch((function(e){console.log(e)}))},RemoveRole:function(e,t){var r=this;console.log(e+t),Object(a["c"])({UserName:e,RoleName:t}).then((function(e){console.log(e),r.getdata()})).catch((function(e){console.log(e)}))},AddRole:function(){var e=this;Object(a["a"])({UserName:this.UserName,RoleName:this.RoleName}).then((function(t){console.log(t),e.dialogAddRoleVisible=!1,e.getdata()})).catch((function(e){console.log(e)}))},resetTempForm:function(){this.tempForm={UserName:"",Email:"",Password:"",PhoneNumber:"",ConfirmPassword:""}},handleCreate:function(){var e=this;this.resetTempForm(),this.dialogFormStatus="create",this.dialogFormVisible=!0,this.$nextTick((function(){e.$refs["dataForm"].clearValidate()}))},handleUpdate:function(e){var t=this;console.log(e),this.tempForm.UserName=e.UserName,this.tempForm.Email=e.Email,this.tempForm.PhoneNumber=e.PhoneNumber,this.tempForm.Password=e.Password,this.tempForm.ConfirmPassword=e.ConfirmPassword,this.dialogFormStatus="update",this.dialogFormVisible=!0,this.$nextTick((function(){t.$refs["dataForm"].clearValidate()}))},createData:function(){var e=this;this.$refs["dataForm"].validate((function(t){if(!t)return console.log("error submit!!"),!1;Object(a["e"])(e.tempForm).then((function(t){e.getdata(),e.dialogFormVisible=!1,e.$notify({title:"تم ",message:"تم الإضافة بنجاح",type:"success",duration:2e3})})).catch((function(e){console.log(e)}))}))},updateData:function(){var e=this;this.$refs["dataForm"].validate((function(t){if(!t)return console.log("error submit!!"),!1;f(e.tempForm).then((function(t){e.getdata(),e.dialogFormVisible=!1,e.$notify({title:"تم",message:"تم التعديل بنجاح",type:"success",duration:2e3})})).catch((function(e){console.log(e)}))}))}}},J=M,q=Object(F["a"])(J,o,n,!1,null,null,null);t["default"]=q.exports},"1f8d":function(e,t,r){"use strict";var o=r("355a"),n=r.n(o);n.a},"355a":function(e,t,r){},c426:function(e,t,r){"use strict";var o=r("f667"),n=r.n(o);n.a},f667:function(e,t,r){}}]);