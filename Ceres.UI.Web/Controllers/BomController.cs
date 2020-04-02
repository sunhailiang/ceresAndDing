using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceres.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ceres.Application.ViewModels;

namespace Ceres.UI.Web.Controllers
{
    public class BomController : Controller
    {
        private readonly IBomFoodMainAppService _bomFoodMainAppService;
        public BomController(IBomFoodMainAppService bomFoodMainAppService)
        {
            _bomFoodMainAppService = bomFoodMainAppService;
        }
        public IActionResult Index()
        {
            return View(_bomFoodMainAppService.QueryRandomFoodByClassify("C","1",1));
        }
    }
}