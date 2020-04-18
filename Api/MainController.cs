using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Recommender.Api {

    [Route("/")]
    public class MainController : ControllerBase {
        private readonly MainService mainService;

        public MainController(MainService mainService) {
            this.mainService = mainService;
        }

        [HttpGet("")]
        public IActionResult Index() {
            return Ok("recommender");
        }
    }
}