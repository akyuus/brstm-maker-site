using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using brstm_maker_site.Interfaces;
using brstm_maker_site.Models;

namespace brstm_maker_site.Controllers
{
    [Route("api/brstms")]
    [ApiController]
    public class BrstmController : ControllerBase
    {
        private readonly IBrstmService _brstmService;

        public BrstmController(IBrstmService brstmService)
        {
            _brstmService = brstmService;
        }

        [HttpGet("create")]
        public async Task<IActionResult> makeBrstm(string url, string courseAbbreviation, int start, int end, double speedFactor, int decibelIncrease, bool finalLap)
        {
            byte[] data;
            CourseInfo courseInfo = new CourseInfo(courseAbbreviation);
            BrstmOptions options = new BrstmOptions
            {
                courseInfo = courseInfo,
                start = start,
                end = end,
                speedFactor = speedFactor,
                decibelIncrease = decibelIncrease,
                finalLap = finalLap
            };

            data = await _brstmService.makeBrstm(options, url);
            string filename = courseInfo.fileName + '_';
            string norf = options.finalLap ? "f" : "n";
            filename += courseInfo.allCaps ? norf.ToUpper() : norf;
            filename += ".brstm";
            return File(data, "application/octet-stream", filename);
        }
    }
}
