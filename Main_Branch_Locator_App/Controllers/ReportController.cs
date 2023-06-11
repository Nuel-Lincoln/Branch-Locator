using Main_Branch_Locator_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using GeoCoordinatePortable;
using System.Linq;
//using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Data;


namespace Another_Branch_Locator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly MainBranchLocatorDbContext _BContext;
        public ReportController(MainBranchLocatorDbContext BContext)
        {
            _BContext = BContext;
        }



        //Get Reports by Services
        [HttpGet("Report")]
        public async Task<IActionResult> GetAllReport(string Report)
        {
            var ArrayOfBranchesWithReportAvailable = await _BContext.ReportTable
                        .Where(u => u.ReportSubject == Report).ToListAsync();
            var OrderedList = ArrayOfBranchesWithReportAvailable.OrderByDescending(item => item.ReportDateTime);
            return Ok(OrderedList);
        }


        //Get reports by Branch
        [HttpGet("BranchCode")]
        public async Task<IActionResult> GetAllReportByBranch(int BranchCode)
        {
            var ArrayOfBranchesWithReportAvailable = await _BContext.ReportTable
                        .Where(u => u.FkBranchCode == BranchCode).ToListAsync();
            var OrderedList = ArrayOfBranchesWithReportAvailable.OrderByDescending(item => item.ReportId);
            return Ok(OrderedList);
        }

        //Get reports by Branch
        [HttpGet("{BranchCode:int}/Report")]
        public async Task<IActionResult> GetAllReportByBranchAndService(int BranchCode, string Report)
        {
            var ArrayOfBranchesWithReportAvailable = await _BContext.ReportTable
                        .Where(u => u.FkBranchCode == BranchCode && u.ReportSubject == Report).ToListAsync();
            return Ok(ArrayOfBranchesWithReportAvailable);
        }

        //Get reports by Branch
        [HttpGet("{ReportId:long}")]
        public async Task<IActionResult> GetAllReportByReportId(long ReportId)
        {

            var ArrayOfBranchesWithReportAvailable = await _BContext.ReportTable
                        .Where(u => u.ReportId == ReportId).ToListAsync();
            var TOReturn = ArrayOfBranchesWithReportAvailable.Select(Report =>
            {
                return Report.ReportId == ReportId;
            });
            return Ok(ArrayOfBranchesWithReportAvailable);
        }


        //Create a new Report by User
        [HttpPost]
        public async Task<IActionResult> CreateBranch([FromBody] ReportTable branchModel)
        {


            if (await _BContext.ReportTable.FirstOrDefaultAsync(u => u.ReportSubject == branchModel.ReportSubject) != null &&
                await _BContext.ReportTable.FirstOrDefaultAsync(u => u.ReportDescription == branchModel.ReportDescription) != null)
            {
                ModelState.AddModelError("CustomError", "Services with that Services Id and Subject Name already exists");
                return BadRequest(ModelState);
            }
            if (branchModel == null)
            {
                return BadRequest(branchModel);
            }

            await _BContext.ReportTable.AddAsync(branchModel);
            await _BContext.SaveChangesAsync();


            return Ok(branchModel);
        }

        //Update the status of an available service by the local admin
        [HttpPatch("{ReportId:long}")]
        [ProducesResponseType(200, Type = typeof(ReportTable))]
        [ProducesResponseType(404, Type = typeof(ReportTable))]
        [ProducesResponseType(400, Type = typeof(ReportTable))]
        public async Task<IActionResult> UpdateReportStatus(long ReportId, JsonPatchDocument<ReportTable> patchDTO)
        {
            if (patchDTO == null || ReportId == 0)
            {
                return BadRequest();
            }
            var ReportToUpdate =  await _BContext.ReportTable.FirstOrDefaultAsync(u => u.ReportId == ReportId);
            if (ReportToUpdate == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(ReportToUpdate, ModelState);
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            await _BContext.SaveChangesAsync();
            return NoContent();

        }

    }        

}
