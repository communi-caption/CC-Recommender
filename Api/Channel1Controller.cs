using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Channel1;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Recommender.Api {

    [Route("/ch1")]
    public class Channel1Controller : ControllerBase {
        private readonly Channel1Service service;

        public Channel1Controller(Channel1Service service) {
            this.service = service;
        }

        [HttpPost("train")]
        public IActionResult Train([FromBody] int[][] data) {
            var dataset = new List<Rating>();
            foreach (var x in data) {
                dataset.Add(new Rating(x[0], x[1]));
            }
            service.Train(dataset);
            return Ok("trained");
        }

        [HttpGet("info")]
        public IActionResult Info() {
            return Ok(service.Info());
        }

        [HttpGet("predict/{userId}/{itemId}")]
        public IActionResult Predict([FromRoute] int userId, [FromRoute] int itemId) {
            float prediction;
            
            try {
                prediction = service.Predict(userId, itemId);
            } catch (PredictionException e) {
                return BadRequest(e.Message);
            }

            return Ok(prediction);
        }

        [HttpGet("recommend/{userId}/{count}")]
        public IActionResult Recommend([FromRoute] int userId, [FromRoute] int count) {
            int[] items;

            try {
                items = service.Recommend(userId, count);
            }
            catch (PredictionException e) {
                return BadRequest(e.Message);
            }

            return Ok(items);
        }
    }
}