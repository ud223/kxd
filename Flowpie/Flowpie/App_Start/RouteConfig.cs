using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Flowpie
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "index",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "home",
                url: "home",
                defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );

            /************************************************************
            *业务操作路由
            *
            *************************************************************/

            routes.MapRoute(
                name: "order-save",
                url: "order/save",
                defaults: new { controller = "Home", action = "OrderSave", id = UrlParameter.Optional }
            );

            /************************************************************
            *后台路由
            *
            *************************************************************/

            /************************************************************
            *登陆及权限路由
            *
            *************************************************************/
            routes.MapRoute(
               name: "login",
               url: "login",
               defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "manage-index",
                url: "{controller}/{action}",
                defaults: new { controller = "Manage", action = "Index", id = UrlParameter.Optional }
            );

            //****************************业务数据管理****************************************
            #region 查看客户列表

            routes.MapRoute(
                name: " customer-list-home",
                url: "{controller}/customer/List",
                defaults: new { controller = "Manage", action = "CustomerList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "customer-list",
                url: "{controller}/customer/List/{page}",
                defaults: new { controller = "Manage", action = "CustomerList", page = UrlParameter.Optional }
            );

            #endregion 

            #region 快递员管理

            routes.MapRoute(
                name: " courier-list-home",
                url: "{controller}/courier/List",
                defaults: new { controller = "Manage", action = "CourierList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "courier-list",
                url: "{controller}/courier/List/{page}",
                defaults: new { controller = "Manage", action = "CourierList", page = UrlParameter.Optional }
            );

            #endregion 

            #region 订单管理

            routes.MapRoute(
                name: " order-list-home",
                url: "{controller}/order/List",
                defaults: new { controller = "Manage", action = "OrderList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "order-list",
                url: "{controller}/order/List/{page}",
                defaults: new { controller = "Manage", action = "OrderList", page = UrlParameter.Optional }
            );

            #endregion 


            #region 快递公司管理

            routes.MapRoute(
                name: " company-list-home",
                url: "{controller}/Company/List",
                defaults: new { controller = "Manage", action = "CompanyList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "company-list",
                url: "{controller}/Company/List/{page}",
                defaults: new { controller = "Manage", action = "CompanyList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "company-add",
                url: "{controller}/Company/Add",
                defaults: new { controller = "Manage", action = "CompanyEdit" }
            );

            routes.MapRoute(
                name: "company-edit",
                url: "{controller}/Company/Edit/{id}",
                defaults: new { controller = "Manage", action = "CompanyEdit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "company-edit-post",
                url: "{controller}/Company/Save/{id}",
                defaults: new { controller = "Manage", action = "CompanySave", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "company-delete",
                url: "{controller}/Company/Delete/{id}/{page}",
                defaults: new { controller = "Manage", action = "CompanyDelete", id = UrlParameter.Optional, page = UrlParameter.Optional }
            );

            #endregion 

            #region 快递站点管理

            routes.MapRoute(
                name: " site-list-home",
                url: "{controller}/Site/List",
                defaults: new { controller = "Manage", action = "SiteList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "site-list",
                url: "{controller}/Site/List/{page}",
                defaults: new { controller = "Manage", action = "SiteList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "site-add",
                url: "{controller}/Site/Add",
                defaults: new { controller = "Manage", action = "SiteEdit" }
            );

            routes.MapRoute(
                name: "site-edit",
                url: "{controller}/Site/Edit/{id}",
                defaults: new { controller = "Manage", action = "SiteEdit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "site-edit-post",
                url: "{controller}/Site/Save/{id}",
                defaults: new { controller = "Manage", action = "SiteSave", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "site-delete",
                url: "{controller}/Site/Delete/{id}/{page}",
                defaults: new { controller = "Manage", action = "SiteDelete", id = UrlParameter.Optional, page = UrlParameter.Optional }
            );

            #endregion 

            //****************************系统管理********************************************

            #region 管理员管理

            routes.MapRoute(
                name: "user-list-home",
                url: "{controller}/User/List",
                defaults: new { controller = "Manage", action = "UserList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "user-list",
                url: "{controller}/User/List/{page}",
                defaults: new { controller = "Manage", action = "UserList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "user-add",
                url: "{controller}/User/Add",
                defaults: new { controller = "Manage", action = "UserEdit" }
            );

            routes.MapRoute(
                name: "user-edit",
                url: "{controller}/User/Edit/{id}",
                defaults: new { controller = "Manage", action = "UserEdit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "user-edit-post",
                url: "{controller}/User/Save/{id}",
                defaults: new { controller = "Manage", action = "UserSave", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "user-delete",
                url: "{controller}/User/Delete/{id}/{page}",
                defaults: new { controller = "Manage", action = "UserDelete", id = UrlParameter.Optional, page = UrlParameter.Optional }
            );

            #endregion 

            #region 部门管理

            routes.MapRoute(
                name: "dept-list-home",
                url: "{controller}/Dept/List",
                defaults: new { controller = "Manage", action = "DeptList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "dept-list",
                url: "{controller}/Dept/List/{page}",
                defaults: new { controller = "Manage", action = "DeptList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "dept-add",
                url: "{controller}/Dept/Add",
                defaults: new { controller = "Manage", action = "DeptEdit" }
            );

            routes.MapRoute(
                name: "dept-edit",
                url: "{controller}/Dept/Edit/{id}",
                defaults: new { controller = "Manage", action = "DeptEdit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "dept-edit-post",
                url: "{controller}/Dept/Save/{id}",
                defaults: new { controller = "Manage", action = "DeptSave", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "dept-delete",
                url: "{controller}/Dept/Delete/{id}/{page}",
                defaults: new { controller = "Manage", action = "DeptDelete", id = UrlParameter.Optional, page = UrlParameter.Optional }
            );

            #endregion 

            #region 角色管理

            routes.MapRoute(
                name: "usertype-list-home",
                url: "{controller}/UserType/List",
                defaults: new { controller = "Manage", action = "UserTypeList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "usertype-list",
                url: "{controller}/UserType/List/{page}",
                defaults: new { controller = "Manage", action = "UserTypeList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "usertype-add",
                url: "{controller}/UserType/Add",
                defaults: new { controller = "Manage", action = "UserTypeEdit" }
            );

            routes.MapRoute(
                name: "usertype-edit",
                url: "{controller}/UserType/Edit/{id}",
                defaults: new { controller = "Manage", action = "UserTypeEdit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "usertype-edit-post",
                url: "{controller}/UserType/Save/{id}",
                defaults: new { controller = "Manage", action = "UserTypeSave", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "usertype-delete",
                url: "{controller}/UserType/Delete/{id}/{page}",
                defaults: new { controller = "Manage", action = "UserTypeDelete", id = UrlParameter.Optional, page = UrlParameter.Optional }
            );

            #endregion 

            #region 角色管理

            routes.MapRoute(
                name: "accesstype-list-home",
                url: "{controller}/AccessType/List",
                defaults: new { controller = "Manage", action = "AccessTypeList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "accesstype-list",
                url: "{controller}/AccessType/List/{page}",
                defaults: new { controller = "Manage", action = "AccessTypeList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "accesstype-add",
                url: "{controller}/AccessType/Add",
                defaults: new { controller = "Manage", action = "AccessTypeEdit" }
            );

            routes.MapRoute(
                name: "accesstype-edit",
                url: "{controller}/AccessType/Edit/{id}",
                defaults: new { controller = "Manage", action = "AccessTypeEdit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "accesstype-edit-post",
                url: "{controller}/AccessType/Save/{id}",
                defaults: new { controller = "Manage", action = "AccessTypeSave", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "accesstype-delete",
                url: "{controller}/AccessType/Delete/{id}/{page}",
                defaults: new { controller = "Manage", action = "AccessTypeDelete", id = UrlParameter.Optional, page = UrlParameter.Optional }
            );

            #endregion 

            #region 菜单路由

            routes.MapRoute(
                name: "menu-list-home",
                url: "{controller}/Menu/List",
                defaults: new { controller = "Manage", action = "MenuList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "menu-list",
                url: "{controller}/Menu/List/{page}",
                defaults: new { controller = "Manage", action = "MenuList", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "menu-add",
                url: "{controller}/Menu/Add",
                defaults: new { controller = "Manage", action = "MenuEdit" }
            );

            routes.MapRoute(
                name: "menu-edit",
                url: "{controller}/Menu/Edit/{id}",
                defaults: new { controller = "Manage", action = "MenuEdit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "menu-edit-post",
                url: "{controller}/Menu/Save/{id}",
                defaults: new { controller = "Manage", action = "MenuSave", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "menu-delete",
                url: "{controller}/Menu/Delete/{id}/{page}",
                defaults: new { controller = "Manage", action = "MenuDelete", id = UrlParameter.Optional, page = UrlParameter.Optional }
            );

            #endregion;

            //默认路由配置放在最后 ,避免提前匹配
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
