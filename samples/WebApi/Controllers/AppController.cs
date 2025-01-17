﻿using CodEaisy.TinySaas.Samples.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodEaisy.TinySaas.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {
        private readonly AppSingleton _appSingleton;

        public AppController(AppSingleton appSingleton) => _appSingleton = appSingleton;

        [HttpGet]
        public ActionResult Index() => Ok(_appSingleton.GetValue());
    }
}
