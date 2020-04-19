using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Recommender.Api
{
        [Route("/ch3")]
        public class Channel3Controller : ControllerBase
        {
            private readonly Channel3Service service;

            public Channel3Controller(Channel3Service service)
            {
                this.service = service;
            }

            [HttpGet("predict/{workOfArt}")]
            public async Task<IActionResult> Predict([FromRoute] string workOfArt)
            {
                string[] peopleAlsoSearch;
                try
                {
                    peopleAlsoSearch = await service.getPeopleAlsoSearchForAsync(workOfArt);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                return Ok(peopleAlsoSearch);
                /*
                 * [
                    "Francisco Goya",
                    "Vincent van Gogh",
                    "Salvador Dalí",
                    "Leonardo da Vinci",
                    "Claude Monet"
                   ]
                */
            }
        }
    }

