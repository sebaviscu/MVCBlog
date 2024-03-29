// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace MVCBlog.Website.Controllers
{
    public partial class ServiceInfController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ServiceInfController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult Export()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Export);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.FileResult Download()
        {
            return new T4MVC_System_Web_Mvc_FileResult(Area, Name, ActionNames.Download);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ServiceInfController Actions { get { return MVC.ServiceInf; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "ServiceInf";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "ServiceInf";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string Export = "Export";
            public readonly string Download = "Download";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Export = "Export";
            public const string Download = "Download";
        }


        static readonly ActionParamsClass_Export s_params_Export = new ActionParamsClass_Export();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Export ExportParams { get { return s_params_Export; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Export
        {
            public readonly string viewModel = "viewModel";
        }
        static readonly ActionParamsClass_Download s_params_Download = new ActionParamsClass_Download();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Download DownloadParams { get { return s_params_Download; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Download
        {
            public readonly string file = "file";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string Index = "Index";
            }
            public readonly string Index = "~/Views/ServiceInf/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ServiceInfController : MVCBlog.Website.Controllers.ServiceInfController
    {
        public T4MVC_ServiceInfController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ExportOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, MVCBlog.Website.Models.OutputModels.Society.ServiceInfViewModels viewModel);

        [NonAction]
        public override System.Web.Mvc.JsonResult Export(MVCBlog.Website.Models.OutputModels.Society.ServiceInfViewModels viewModel)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Export);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "viewModel", viewModel);
            ExportOverride(callInfo, viewModel);
            return callInfo;
        }

        [NonAction]
        partial void DownloadOverride(T4MVC_System_Web_Mvc_FileResult callInfo, string file);

        [NonAction]
        public override System.Web.Mvc.FileResult Download(string file)
        {
            var callInfo = new T4MVC_System_Web_Mvc_FileResult(Area, Name, ActionNames.Download);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "file", file);
            DownloadOverride(callInfo, file);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
