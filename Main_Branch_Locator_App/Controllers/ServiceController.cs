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
    public class ServicesController : ControllerBase
    {
        private readonly MainBranchLocatorDbContext _BContext;
        public ServicesController(MainBranchLocatorDbContext BContext)
        {
            _BContext = BContext;
        }

        


        //Get All Branches
       




        //Get services by location and service availability
        [HttpGet("Service")]
        public async Task<IActionResult> GetService(string Service)
        {
            
            switch (Service)
            {
                case "Funds Transfer":
                    var ArrayOfBranchesWithServiceAvailable = await _BContext.ServicesTable
                        .Where(u => u.ServiceSubject == Service).ToListAsync();
                    
                    return Ok(ArrayOfBranchesWithServiceAvailable);
                case "Foreign Transfer":
                    var ArrayOfBranchesWithServiceAvailableFX = await _BContext.ServicesTable
                        .Where(u => u.ServiceSubject == Service).ToListAsync();

                    return Ok(ArrayOfBranchesWithServiceAvailableFX);
                case "Foreign Transactions":
                    var ArrayOfBranchesWithServiceAvailableFXT = await _BContext.ServicesTable
                         .Where(u => u.ServiceSubject == Service).ToListAsync();

                    return Ok(ArrayOfBranchesWithServiceAvailableFXT);
                case "Account Opening":
                    var ArrayOfBranchesWithServiceAvailableAC = await _BContext.ServicesTable
                         .Where(u => u.ServiceSubject == Service).ToListAsync();

                    return Ok(ArrayOfBranchesWithServiceAvailableAC);
                case "Atm Collection":
                    var ArrayOfBranchesWithServiceAvailableATM = await _BContext.ServicesTable
                        .Where(u => u.ServiceSubject == Service).ToListAsync();

                    return Ok(ArrayOfBranchesWithServiceAvailableATM);
                case "Atm Cash Withdrawal":
                    var ArrayOfBranchesWithServiceAvailableAtm = await _BContext.ServicesTable
                        .Where(u => u.ServiceSubject == Service).ToListAsync();

                    return Ok(ArrayOfBranchesWithServiceAvailableAtm);
                case "Cash Deposit":
                    var ArrayOfBranchesWithServiceAvailableCD = await _BContext.ServicesTable
                        .Where(u => u.ServiceSubject == Service).ToListAsync();

                    return Ok(ArrayOfBranchesWithServiceAvailableCD);
                case "Remitta Transactions":
                    var ArrayOfBranchesWithServiceAvailableRT = await _BContext.ServicesTable
                        .Where(u => u.ServiceSubject == Service).ToListAsync();

                    return Ok(ArrayOfBranchesWithServiceAvailableRT);
                case "Pay Direct":
                    var ArrayOfBranchesWithServiceAvailablePD = await _BContext.ServicesTable
                        .Where(u => u.ServiceSubject == Service).ToListAsync();
                    return Ok(ArrayOfBranchesWithServiceAvailablePD);
                case "Token Collection":
                    var ArrayOfBranchesWithServiceAvailableTD = await _BContext.ServicesTable
                        .Where(u => u.ServiceSubject == Service).ToListAsync();

                    return Ok(ArrayOfBranchesWithServiceAvailableTD);
                default:
                    return NotFound();
            }

        }


        //Create a new Service by Admin
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServicesTable branchModel)
        {


            if (await _BContext.ServicesTable.FirstOrDefaultAsync(u => u.ServiceSubject == branchModel.ServiceSubject) != null &&
                await _BContext.ServicesTable.FirstOrDefaultAsync(u => u.ServiceId == branchModel.ServiceId) != null )
            {
                ModelState.AddModelError("CustomError", "Services with that Services Id and Subject Name already exists");
                return BadRequest(ModelState);
            }
            if (branchModel == null)
            {
                return BadRequest(branchModel);
            }

            //var highestBranchCode = _BContext.ServicesTable.OrderByDescending(u => u.ServiceId).FirstOrDefault()?.BranchCode;
            //branchModel.BranchCode = highestBranchCode.HasValue ? highestBranchCode.Value + 1 : 1;


            await _BContext.ServicesTable.AddAsync(branchModel);
            await _BContext.SaveChangesAsync();


            return Ok(branchModel);
        }

        //Authenticate User
        

        //Edit the info for a particular branch
        [HttpPut("Service")]
        [ProducesResponseType(200, Type = typeof(BranchTable))]
        [ProducesResponseType(404, Type = typeof(BranchTable))]
        [ProducesResponseType(400, Type = typeof(BranchTable))]
        public async Task<IActionResult> EditServiceInfoAsync(string Service, [FromBody] ServicesTable branchDTO)
        {

            if (branchDTO == null || Service != branchDTO.ServiceSubject)
            {
                return BadRequest();
            }
            var BranchToEdit = _BContext.ServicesTable.FirstOrDefault(u => u.ServiceSubject == branchDTO.ServiceSubject);
            BranchToEdit.AvailableOnline = branchDTO.AvailableOnline;
            BranchToEdit.ServiceSubject = branchDTO.ServiceSubject;
            BranchToEdit.ServiceRequiredDocuments = branchDTO.ServiceRequiredDocuments;
            BranchToEdit.ServiceDescription = branchDTO.ServiceDescription;
            BranchToEdit.LinkToService = branchDTO.LinkToService;
            
            _BContext.ServicesTable.Update(BranchToEdit);
            await _BContext.SaveChangesAsync();


            return NoContent();
        }



        //Update the documents of a service by the local admin
        [HttpPatch("Service")]
        [ProducesResponseType(200, Type = typeof(BranchTable))]
        [ProducesResponseType(404, Type = typeof(BranchTable))]
        [ProducesResponseType(400, Type = typeof(BranchTable))]
        public async Task<IActionResult> UpdateService( string Service, JsonPatchDocument<ServicesTable> patchDTO)
        {
            if (patchDTO == null || Service == "")
            {
                return BadRequest();
            }
            var BranchToUpdate = _BContext.ServicesTable.FirstOrDefault(u => u.ServiceSubject == Service);
            if (BranchToUpdate == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(BranchToUpdate, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _BContext.SaveChangesAsync();
            return NoContent();

        }

        //Delete a Service using Service name 
        [HttpDelete("Service")]
        [ProducesResponseType(200, Type = typeof(BranchTable))]
        [ProducesResponseType(404, Type = typeof(BranchTable))]
        [ProducesResponseType(400, Type = typeof(BranchTable))]
        public async Task<IActionResult> DeleteService(string Service)
        {
            //BranchStore.Branches.FirstOrDefault(u => u.Branch_Code == Branch_Code)
            //return Ok();
            if (Service == " ")
            {
                return BadRequest();
            }
            var BranchToDelete = _BContext.ServicesTable.FirstOrDefault(u => u.ServiceSubject == Service);
            if (BranchToDelete == null)
            {
                //ModelState.AddModelError("CustomError", "Branch with such Branch Code does not exist ");
                return NotFound();
            }
            _BContext.ServicesTable.Remove(BranchToDelete);
            await _BContext.SaveChangesAsync();
            return NoContent();

        }
    }
}
