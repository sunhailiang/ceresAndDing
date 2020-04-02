using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.Application.Interfaces;
using Ceres.Application.ViewModels;
using Ceres.Domain.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Ceres.UI.Web.Controllers
{
    public class SupporterController : Controller
    {
        private readonly ISupporterAppService _sptSupporterMainAppService;
        public SupporterController(ISupporterAppService sptSupporterMainAppService)
        {
            _sptSupporterMainAppService = sptSupporterMainAppService;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: Supporter/Create
        // 页面
        public IActionResult Create()
        {
            return View();
        }

        // POST: Supporter/Create
        // 方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupporterViewModel supporterViewModel)
        {
            try
            {
                ViewBag.ErrorData = null;

                // 视图模型验证
                //if (!ModelState.IsValid)
                //{
                //    return View(sptSupporterViewModel);
                //}

                //添加命令验证，采用构造函数方法实例
                UpdateSupporterPasswordCommand updateSupporterPasswordCommand = new UpdateSupporterPasswordCommand(supporterViewModel.OID,supporterViewModel.Password);
                //如果命令无效，证明有错误
                if (!updateSupporterPasswordCommand.IsValid())
                {
                    List<string> errorInfo = new List<string>();
                    //获取到错误，请思考这个Result从哪里来的 
                    foreach (var error in updateSupporterPasswordCommand.ValidationResult.Errors)
                    {
                        errorInfo.Add(error.ErrorMessage);
                    }
                    //对错误进行记录，还需要抛给前台
                    ViewBag.ErrorData = errorInfo;
                    return View(supporterViewModel);
                }
                // 执行添加方法
                //_sptSupporterMainAppService.UpdateSupporterPassword(supporterViewModel.OID, supporterViewModel.Password);
                // 执行添加方法
                //_sptSupporterMainAppService.UpdateSupporterPassword(supporterViewModel);

                ViewBag.success = "密码修改成功!";

                return View(supporterViewModel);
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }
    }
}