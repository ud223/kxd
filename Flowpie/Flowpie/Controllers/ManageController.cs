using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Collections;

namespace Flowpie.Controllers
{
    public class ManageController : Controller
    {
        [Flowpie.Models.MyAuth(Roles = "系统用户")]//这里配置角色切忌不能有多余的空格
        public ActionResult Index()
        {
            this.init();

            return View();
        }
        //*********************************业务后台action*******************************************************

        #region 客户列表action
        public ActionResult CustomerList(int page = 1)
        {
            this.init();

            KxdLib.UserController userController = new KxdLib.UserController();

            List<System.Collections.Hashtable> list = userController.getAll();

            ViewData["data"] = list;

            ViewData["open_menu"] = "快递业务管理";

            return View();
        }

        #endregion;

        #region 快递员action
        public ActionResult CourierList(int page = 1)
        {
            this.init();

            KxdLib.CourierController courierController = new KxdLib.CourierController();

            List<System.Collections.Hashtable> list = courierController.getAll();

            ViewData["data"] = list;

            ViewData["open_menu"] = "快递业务管理";

            return View();
        }

        #endregion;

        #region 订单action
        public ActionResult OrderList(int page = 1)
        {
            this.init();

            KxdLib.OrderController orderController = new KxdLib.OrderController();

            List<System.Collections.Hashtable> list = orderController.getAll();

            ViewData["data"] = list;

            ViewData["open_menu"] = "快递业务管理";

            return View();
        }

        #endregion;

        #region 快递公司action
        public ActionResult CompanyList(int page = 1)
        {
            this.init();

            KxdLib.CompanyController companyController = new KxdLib.CompanyController();

            List<System.Collections.Hashtable> list = companyController.getAll();

            ViewData["companys"] = list;

            ViewData["open_menu"] = "快递业务管理";

            return View();
        }

        public ActionResult CompanyEdit(string id = null)
        {
            KxdLib.CompanyController companyController = new KxdLib.CompanyController();

            this.init();

            if (id == null)
            {
                ViewData["title"] = "新增快递公司";
            }
            else
            {
                System.Collections.Hashtable company = companyController.load(id);

                if (company != null)
                {
                    ViewData["company"] = company;
                }

                ViewData["title"] = "编辑快递公司";
            }

            ViewData["open_menu"] = "快递业务管理";

            return View();
        }

        [HttpPost]
        public ActionResult CompanySave()
        {
            KxdLib.CompanyController companyController = new KxdLib.CompanyController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            string strParam = Request.Form.ToString();

            System.Collections.Hashtable data = tools.paramToData(strParam);

            string company_id = CommonLib.Common.Validate.IsNullString(Request.Params["CompanyID"]);

            if (company_id == "")
            {
                company_id = companyController.add(data);

                if (company_id == null)
                {
                    return RedirectToRoute("company-add");
                }
            }
            else
            {
                companyController.save(data);
            }

            return RedirectToRoute("company-add");
        }

        public ActionResult CompanyDelete(string id = null, int page = 1)
        {
            KxdLib.CompanyController companyController = new KxdLib.CompanyController();

            this.init();

            if (id == null)
            {
                return RedirectToRoute("company-list-home");
            }
            else
            {
                companyController.delete(id);

                return RedirectToRoute("company-list", new { page = page });
            }

        }

        #endregion;

        #region 快递站点action
        public ActionResult SiteList(int page = 1)
        {
            this.init();

            KxdLib.SiteController siteyController = new KxdLib.SiteController();

            List<System.Collections.Hashtable> list = siteyController.getAll();

            ViewData["sites"] = list;

            ViewData["open_menu"] = "快递业务管理";

            return View();
        }

        public ActionResult SiteEdit(string id = null)
        {
            KxdLib.CompanyController companyController = new KxdLib.CompanyController();
            KxdLib.SiteController siteyController = new KxdLib.SiteController();

            this.init();

            if (id == null)
            {
                ViewData["title"] = "新增快递站点";
            }
            else
            {
                System.Collections.Hashtable site = siteyController.load(id);

                if (site != null)
                {
                    ViewData["site"] = site;
                }

                ViewData["title"] = "编辑快递站点";
            }

            List<System.Collections.Hashtable> list = companyController.getAll();

            ViewData["companys"] = list;

            ViewData["open_menu"] = "快递业务管理";

            return View();
        }

        [HttpPost]
        public ActionResult SiteSave()
        {
            KxdLib.SiteController siteyController = new KxdLib.SiteController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            string strParam = Request.Form.ToString();

            System.Collections.Hashtable data = tools.paramToData(strParam);

            string site_id = CommonLib.Common.Validate.IsNullString(Request.Params["SiteID"]);

            if (site_id == "")
            {
                site_id = siteyController.add(data);

                if (site_id == null)
                {
                    return RedirectToRoute("site-add");
                }
            }
            else
            {
                siteyController.save(data);
            }

            return RedirectToRoute("site-add");
        }

        public ActionResult SiteDelete(string id = null, int page = 1)
        {
            KxdLib.SiteController siteyController = new KxdLib.SiteController();

            this.init();

            if (id == null)
            {
                return RedirectToRoute("site-list-home");
            }
            else
            {
                siteyController.delete(id);

                return RedirectToRoute("site-list", new { page = page });
            }

        }

        #endregion;

        //*********************************系统action*********************************************************

        #region 管理员action

        [Flowpie.Models.MyAuth(Roles = "系统用户")]
        public ActionResult UserList(int page = 1)
        {
            this.init();

            SystemConfigureLib.UserController userController = new SystemConfigureLib.UserController();
            SystemConfigureLib.UserTypeController userTypeController = new SystemConfigureLib.UserTypeController();

            List<System.Collections.Hashtable> list = userController.getAll();

            ViewData["users"] = list;

            List<System.Collections.Hashtable> userType = userTypeController.getAll();

            ViewData["user_type"] = userType;

            ViewData["open_menu"] = "系统管理";

            return View();
        }

        [Flowpie.Models.MyAuth(Roles = "系统用户")]
        public ActionResult UserEdit(string id = null)
        {
            SystemConfigureLib.UserController userController = new SystemConfigureLib.UserController();
            SystemConfigureLib.UserTypeController userTypeController = new SystemConfigureLib.UserTypeController();
            SystemConfigureLib.DeptController deptController = new SystemConfigureLib.DeptController();

            this.init();

            if (id == null)
            {
                ViewData["title"] = "新增管理员";
            }
            else
            {
                System.Collections.Hashtable user = userController.load(id);

                if (user != null)
                {
                    ViewData["user"] = user;
                }

                ViewData["title"] = "编辑管理员";
            }

            ViewData["open_menu"] = "系统管理";

            List<System.Collections.Hashtable> depts = deptController.getAll();

            ViewData["depts"] = depts;

            List<System.Collections.Hashtable> userType = userTypeController.getAll();

            ViewData["user_type"] = userType;

            return View();
        }

        [Flowpie.Models.MyAuth(Roles = "系统用户")]
        [HttpPost]
        public ActionResult UserSave()
        {
            SystemConfigureLib.UserController userController = new SystemConfigureLib.UserController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            string strParam = Request.Form.ToString();

            System.Collections.Hashtable data = tools.paramToData(strParam);

            string user_id = CommonLib.Common.Validate.IsNullString(Request.Params["UserID"]);

            if (user_id == "")
            {
                user_id = userController.add(data);

                if (user_id == null)
                {
                    return RedirectToRoute("user-add");
                }
            }
            else
            {
                userController.save(data);
            }

            return RedirectToRoute("user-add");
        }

        [Flowpie.Models.MyAuth(Roles = "系统用户")]
        public ActionResult UserDelete(string id = null, int page = 1)
        {
            SystemConfigureLib.UserController userController = new SystemConfigureLib.UserController();

            this.init();

            if (id == null)
            {
                return RedirectToRoute("user-list-home");
            }
            else
            {
                userController.delete(id);

                return RedirectToRoute("user-list", new { page = page });
            }

        }

        #endregion;

        #region 部门action
        public ActionResult DeptList(int page = 1)
        {
            this.init();

            SystemConfigureLib.DeptController deptController = new SystemConfigureLib.DeptController();

            List<System.Collections.Hashtable> list = deptController.getAll();

            ViewData["depts"] = list;

            ViewData["open_menu"] = "系统管理";

            return View();
        }

        public ActionResult DeptEdit(string id = null)
        {
            SystemConfigureLib.DeptController deptController = new SystemConfigureLib.DeptController();

            this.init();

            if (id == null)
            {
                ViewData["title"] = "新增部门";
            }
            else
            {
                System.Collections.Hashtable dept = deptController.load(id);

                if (dept != null)
                {
                    ViewData["dept"] = dept;
                }

                ViewData["title"] = "编辑部门";
            }

            ViewData["open_menu"] = "系统管理";

            return View();
        }

        [HttpPost]
        public ActionResult DeptSave()
        {
            SystemConfigureLib.DeptController deptController = new SystemConfigureLib.DeptController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            string strParam = Request.Form.ToString();

            System.Collections.Hashtable data = tools.paramToData(strParam);

            string dept_id = CommonLib.Common.Validate.IsNullString(Request.Params["DeptID"]);

            if (dept_id == "")
            {
                dept_id = deptController.add(data);

                if (dept_id == null)
                {
                    return RedirectToRoute("dept-add");
                }
            }
            else
            {
                deptController.save(data);
            }

            return RedirectToRoute("dept-add");
        }

        public ActionResult DeptDelete(string id = null, int page = 1)
        {
            SystemConfigureLib.DeptController deptController = new SystemConfigureLib.DeptController();

            this.init();

            if (id == null)
            {
                return RedirectToRoute("dept-list-home");
            }
            else
            {
                deptController.delete(id);

                return RedirectToRoute("dept-list", new { page = page });
            }

        }

        #endregion;

        #region 角色action

        public ActionResult UserTypeList(int page = 1)
        {
            this.init();

            SystemConfigureLib.UserTypeController usertypeController = new SystemConfigureLib.UserTypeController();

            List<System.Collections.Hashtable> list = usertypeController.getAll();

            ViewData["usertypes"] = list;

            ViewData["open_menu"] = "系统管理";

            return View();
        }

        public ActionResult UserTypeEdit(string id = null)
        {
            SystemConfigureLib.UserTypeController usertypeController = new SystemConfigureLib.UserTypeController();

            this.init();

            if (id == null)
            {
                ViewData["title"] = "新增角色";
            }
            else
            {
                System.Collections.Hashtable usertype = usertypeController.load(id);

                if (usertype != null)
                {
                    ViewData["usertype"] = usertype;
                }

                ViewData["title"] = "编辑角色";
            }

            ViewData["open_menu"] = "系统管理";

            return View();
        }

        [HttpPost]
        public ActionResult UserTypeSave()
        {
            SystemConfigureLib.UserTypeController usertypeController = new SystemConfigureLib.UserTypeController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            string strParam = Request.Form.ToString();

            System.Collections.Hashtable data = tools.paramToData(strParam);

            string uusertype_id = CommonLib.Common.Validate.IsNullString(Request.Params["UserTypeID"]);

            if (uusertype_id == "")
            {
                uusertype_id = usertypeController.add(data);

                if (uusertype_id == null)
                {
                    return RedirectToRoute("usertype-add");
                }
            }
            else
            {
                usertypeController.save(data);
            }

            return RedirectToRoute("usertype-add");
        }

        public ActionResult UserTypeDelete(string id = null, int page = 1)
        {
            SystemConfigureLib.UserTypeController usertypeController = new SystemConfigureLib.UserTypeController();

            this.init();

            if (id == null)
            {
                return RedirectToRoute("usertype-list-home");
            }
            else
            {
                usertypeController.delete(id);

                return RedirectToRoute("usertype-list", new { page = page });
            }

        }

        #endregion;

        #region 访问类型action

        public ActionResult AccessTypeList(int page = 1)
        {
            this.init();

            SystemConfigureLib.AccessTypeControllerr accessTypeControllerr = new SystemConfigureLib.AccessTypeControllerr();

            List<System.Collections.Hashtable> list = accessTypeControllerr.getAll();

            ViewData["accesstypes"] = list;

            ViewData["open_menu"] = "系统管理";

            return View();
        }

        public ActionResult AccessTypeEdit(string id = null)
        {
            SystemConfigureLib.AccessTypeControllerr accessTypeControllerr = new SystemConfigureLib.AccessTypeControllerr();

            this.init();

            if (id == null)
            {
                ViewData["title"] = "新增角色";
            }
            else
            {
                System.Collections.Hashtable accesstype = accessTypeControllerr.load(id);

                if (accesstype != null)
                {
                    ViewData["accesstype"] = accesstype;
                }

                ViewData["title"] = "编辑角色";
            }

            ViewData["open_menu"] = "系统管理";

            return View();
        }

        [HttpPost]
        public ActionResult AccessTypeSave()
        {
            SystemConfigureLib.AccessTypeControllerr accessTypeControllerr = new SystemConfigureLib.AccessTypeControllerr();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            string strParam = Request.Form.ToString();

            System.Collections.Hashtable data = tools.paramToData(strParam);

            string accesstype_id = CommonLib.Common.Validate.IsNullString(Request.Params["AccessTypeID"]);

            if (accesstype_id == "")
            {
                accesstype_id = accessTypeControllerr.add(data);

                if (accesstype_id == null)
                {
                    return RedirectToRoute("accesstype-add");
                }
            }
            else
            {
                accessTypeControllerr.save(data);
            }

            return RedirectToRoute("accesstype-add");
        }

        public ActionResult AccessTypeDelete(string id = null, int page = 1)
        {
            SystemConfigureLib.AccessTypeControllerr accessTypeControllerr = new SystemConfigureLib.AccessTypeControllerr();

            this.init();

            if (id == null)
            {
                return RedirectToRoute("accesstype-list-home");
            }
            else
            {
                accessTypeControllerr.delete(id);

                return RedirectToRoute("accesstype-list", new { page = page });
            }

        }

        #endregion;

        #region 菜单action

        public ActionResult MenuList(int page = 1)
        {
            this.init();

            ViewData["open_menu"] = "系统管理";

            return View();
        }

        public ActionResult MenuEdit(string id = null)
        {
            SystemConfigureLib.MenuController menuController = new SystemConfigureLib.MenuController();

            this.init();

            if (id == null)
            {
                ViewData["title"] = "新增菜单";
            }
            else
            {
                System.Collections.Hashtable menu = menuController.load(id);

                if (menu != null)
                {
                    ViewData["menu"] = menu;
                }

                ViewData["title"] = "编辑菜单";
            }

            ViewData["open_menu"] = "系统管理";

            

            List<System.Collections.Hashtable> menus = menuController.getTopMenu();

            ViewData["top_menus"] = menus;

            return View();
        }

        [HttpPost]
        public ActionResult MenuSave()
        {
            SystemConfigureLib.MenuController menuController = new SystemConfigureLib.MenuController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            string strParam = Request.Form.ToString();

            System.Collections.Hashtable menu_data = tools.paramToData(strParam);

            string menu_id = CommonLib.Common.Validate.IsNullString(Request.Params["MenuID"]);

            if (menu_id == "")
            {
                menu_id = menuController.add(menu_data);

                if (menu_id == null)
                {
                    return RedirectToRoute("menu-add");
                }
            }
            else
            {
                menuController.save(menu_data);
            }

            return RedirectToRoute("menu-add");
        }

        public ActionResult MenuDelete(string id = null, int page = 1)
        {
            SystemConfigureLib.MenuController menuController = new SystemConfigureLib.MenuController();

            this.init();

            if (id == null)
            {
                return RedirectToRoute("menu-list-home");
            }
            else
            {
                menuController.delete(id);

                return RedirectToRoute("menu-list", new { page = page });
            }

        }

        #endregion;

        #region 共用方法

        /// <summary>
        /// 初始化后台框架数据通用入口
        /// </summary>
        private void init()
        {
            initMenus();
        }

        /// <summary>
        /// 后台管理框架获取菜单通用方法
        /// </summary>
        private void initMenus()
        {
            SystemConfigureLib.MenuController menuController = new SystemConfigureLib.MenuController();

            List<System.Collections.Hashtable> menu = menuController.getAll();

            ViewData["menus"] = menu;
        }

        #endregion;
    }
}