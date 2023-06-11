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
    public class BranchController : ControllerBase
    {
        private readonly MainBranchLocatorDbContext _BContext;
        public BranchController(MainBranchLocatorDbContext BContext)
        {
            _BContext = BContext;
        }

        public class BranchCredentials
        {
            public int BranchCode { get; set; }
            public string BranchPassword { get; set; }
        }

        private static BranchTable[] GetClosestBranches(GeoCoordinate definedLocation,
            List<BranchTable> branches, int numberOfClosestBranches)
        {
            var closestBranches = branches
                .Select(b => new
                {
                    Branch = b,
                    Distance = new GeoCoordinate(b.BranchGpsXCoordinate, b.BranchGpsYCoordinate).GetDistanceTo(definedLocation)
                })
                .OrderBy(d => d.Distance)
                .Take(numberOfClosestBranches)
                .Select(d => d.Branch)
                .ToArray();

            return closestBranches;
        }


        //Get All Branches
        [HttpGet()]
        public async Task<IActionResult> GetBranches()
        {
            return Ok(await _BContext.BranchTable.ToListAsync());
        }


        //Get All Branches by branch Code
        [HttpGet("{Code:int}")]
        public async Task<IActionResult> GetBranchesByCode(int Code)
        {
            var ArrayOfBranchesWithServiceAvailable = await _BContext.BranchTable
                        .FirstOrDefaultAsync(u => u.BranchCode == Code);
            return Ok(ArrayOfBranchesWithServiceAvailable);
        }




        //Get services by location and service availability
        [HttpGet("Service/{LocationX:double}/{LocationY:double}")]
        public async Task<IActionResult> GetBranchesByLocation(string Service, double LocationX, double LocationY)
        {
            var numberOfClosestBranches = 5;
            var definedLocation = new GeoCoordinate(LocationX, LocationY);

            switch (Service)
            {
                case "Funds Transfer":
                    var ArrayOfBranchesWithServiceAvailable = await _BContext.BranchTable
                        .Where(u => u.FundsTransfer == "Available").ToListAsync();
                   var ClosestBranches = GetClosestBranches(definedLocation, ArrayOfBranchesWithServiceAvailable, numberOfClosestBranches);
                    return Ok(ClosestBranches);
                case "Foreign Transfer":
                    var ArrayOfBranchesWithFTRAvailable = await _BContext.BranchTable
                        .Where(u => u.FxTransfer == "Available").ToListAsync();
                    var ClosestBranchesFTR = GetClosestBranches(definedLocation, ArrayOfBranchesWithFTRAvailable, numberOfClosestBranches);
                    return Ok(ClosestBranchesFTR);
                case "Foreign Transaction":
                    var ArrayOfBranchesWithFTAvailable = await _BContext.BranchTable
                        .Where(u => u.FxTrnx == "Available").ToListAsync();
                    var ClosestBranchesFT = GetClosestBranches(definedLocation, ArrayOfBranchesWithFTAvailable, numberOfClosestBranches);
                    return Ok(ClosestBranchesFT);
                case "Atm Collection":
                    var ArrayOfBranchesWithAtmAvailable = await _BContext.BranchTable
                        .Where(u => u.AtmCollection == "Available").ToListAsync();
                    var ClosestBranchesAtm = GetClosestBranches(definedLocation, ArrayOfBranchesWithAtmAvailable, numberOfClosestBranches);
                    return Ok(ClosestBranchesAtm);
                case "Atm Cash Withdrawal":
                    var ArrayOfBranchesWithCashWAvailable = await _BContext.BranchTable
                        .Where(u => u.AtmCashWithdrawal == "Available").ToListAsync();
                    var ClosestBranchesCashW = GetClosestBranches(definedLocation, ArrayOfBranchesWithCashWAvailable, numberOfClosestBranches);
                    return Ok(ClosestBranchesCashW);
                case "Cash Deposit":
                    var ArrayOfBranchesWithCashDAvailable = await _BContext.BranchTable
                        .Where(u => u.CashDeposit == "Available").ToListAsync();
                    var ClosestBranchesCashd = GetClosestBranches(definedLocation, ArrayOfBranchesWithCashDAvailable, numberOfClosestBranches);
                    return Ok(ClosestBranchesCashd);
                case "Remitta Transaction":
                    var ArrayOfBranchesWithRemittaAvailable = await _BContext.BranchTable
                        .Where(u => u.RemittaTrnx == "Available").ToListAsync();
                    var ClosestBranchesR = GetClosestBranches(definedLocation, ArrayOfBranchesWithRemittaAvailable, numberOfClosestBranches);
                    return Ok(ClosestBranchesR);
                case "Pay Direct":
                    var ArrayOfBranchesWithPayAvailable = await _BContext.BranchTable
                        .Where(u => u.PayDirect == "Available").ToListAsync();
                    var ClosestBranchesP = GetClosestBranches(definedLocation, ArrayOfBranchesWithPayAvailable, numberOfClosestBranches);
                    return Ok(ClosestBranchesP);
                case "Account Opening":
                    var ArrayOfBranchesWithPayAvailableA = await _BContext.BranchTable
                        .Where(u => u.PayDirect == "Available").ToListAsync();
                    var ClosestBranchesPA = GetClosestBranches(definedLocation, ArrayOfBranchesWithPayAvailableA, numberOfClosestBranches);
                    return Ok(ClosestBranchesPA);
                case "Form A Transaction":
                    var ArrayOfBranchesWithPayAvailableAB = await _BContext.BranchTable
                        .Where(u => u.FormAtrnx == "Available").ToListAsync();
                    var ClosestBranchesPAB = GetClosestBranches(definedLocation, ArrayOfBranchesWithPayAvailableAB, numberOfClosestBranches);
                    return Ok(ClosestBranchesPAB);
                default:
                    return NotFound();
            }

        }

       
        //Create a new Branch by Admin
        [HttpPost]
        public async Task<IActionResult> CreateBranch([FromBody] BranchTable branchModel)
        {
            

            if (await _BContext.BranchTable.FirstOrDefaultAsync(u => u.BranchGpsXCoordinate == branchModel.BranchGpsXCoordinate) != null &&
                await _BContext.BranchTable.FirstOrDefaultAsync(u => u.BranchGpsYCoordinate == branchModel.BranchGpsYCoordinate) != null ||
               await _BContext.BranchTable.FirstOrDefaultAsync(u => u.BranchCode == branchModel.BranchCode) != null
                )
            {
                ModelState.AddModelError("CustomError", "Branch with that Branch Cordinates Or Branch Code already exists");
                return BadRequest(ModelState);
            }
            if (branchModel == null)
            {
                return BadRequest(branchModel);
            }
            
            //var highestBranchCode = _BContext.BranchTable.OrderByDescending(u => u.BranchCode).FirstOrDefault()?.BranchCode;
            //branchModel.BranchCode = highestBranchCode.HasValue ? highestBranchCode.Value + 1 : 1;

            
           await _BContext.BranchTable.AddAsync(branchModel);
           await _BContext.SaveChangesAsync();
            

            return Ok( branchModel);
        }

        //Authenticate User
        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateBranch([FromBody] BranchCredentials branchModel)
        {
            var Branch = await _BContext.BranchTable.FirstOrDefaultAsync(u=>u.BranchCode == branchModel.BranchCode);

            if(Branch == null)
            {
                ModelState.AddModelError("Branch_LogInError", "Invalid Branch");
                return BadRequest(ModelState);
                //return NotFound();
            }
            else
            {
                if(Branch.BranchPassword != branchModel.BranchPassword)
                {
                    ModelState.AddModelError("Branch_LogInError", "Wrong Password");
                    return BadRequest(ModelState);
                }  
                return Ok(Branch);
            }
        }


        //Edit the info for a particular branch
        [HttpPut("id")]
        [ProducesResponseType(200, Type = typeof(BranchTable))]
        [ProducesResponseType(404, Type = typeof(BranchTable))]
        [ProducesResponseType(400, Type = typeof(BranchTable))]
        public async Task<IActionResult> EditBranchInfoAsync(int id, [FromBody] BranchTable branchDTO)
        {

            if (branchDTO == null || id != branchDTO.BranchCode)
            {
                return BadRequest();
            }
            var BranchToEdit = _BContext.BranchTable.FirstOrDefault(u => u.BranchCode == branchDTO.BranchCode);
            BranchToEdit.BranchName = branchDTO.BranchName;
            BranchToEdit.BranchManagerTel = branchDTO.BranchManagerTel;
            BranchToEdit.BranchManagerEmail = branchDTO.BranchManagerEmail;
            BranchToEdit.BranchGpsXCoordinate = branchDTO.BranchGpsXCoordinate;
            BranchToEdit.BranchGpsYCoordinate = branchDTO.BranchGpsYCoordinate;
            BranchToEdit.BranchAtmOperatorEmail = branchDTO.BranchAtmOperatorEmail;
            BranchToEdit.BranchAtmOperatorTel = branchDTO.BranchAtmOperatorTel;
            BranchToEdit.BranchManagerName = branchDTO.BranchManagerName;
            BranchToEdit.BranchPassword = branchDTO.BranchPassword;
            BranchToEdit.BranchCode = branchDTO.BranchCode;
            BranchToEdit.CashDeposit = branchDTO.CashDeposit;
            BranchToEdit.FundsTransfer = branchDTO.FundsTransfer;
            BranchToEdit.PayDirect = branchDTO.PayDirect;
            BranchToEdit.TokenCollection = branchDTO.TokenCollection;
            BranchToEdit.FxTransfer = branchDTO.FxTransfer;
            BranchToEdit.AtmCashWithdrawal = branchDTO.AtmCashWithdrawal;
            BranchToEdit.AtmCollection = branchDTO.AtmCollection;
            BranchToEdit.FxTrnx = branchDTO.FxTrnx;
            BranchToEdit.RemittaTrnx = branchDTO.RemittaTrnx;
            BranchToEdit.BranchCity = branchDTO.BranchCity;
            BranchToEdit.BranchAddress = branchDTO.BranchAddress;


            _BContext.BranchTable.Update(BranchToEdit);
            await _BContext.SaveChangesAsync();


            return NoContent();
        }



        //Update the status of an available service by the local admin
        [HttpPatch("id")]
        [ProducesResponseType(200, Type = typeof(BranchTable))]
        [ProducesResponseType(404, Type = typeof(BranchTable))]
        [ProducesResponseType(400, Type = typeof(BranchTable))]
        public async Task<IActionResult> UpdateBranchService(int id, JsonPatchDocument<BranchTable> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var BranchToUpdate = _BContext.BranchTable.FirstOrDefault(u => u.BranchCode == id);
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
            var BranchUpdated = await _BContext.BranchTable.FirstOrDefaultAsync(u => u.BranchCode == id);
            return Ok(BranchUpdated);

        }

        //Delete a branch using branch code 
        [HttpDelete("BranchCode")]
        [ProducesResponseType(200, Type = typeof(BranchTable))]
        [ProducesResponseType(404, Type = typeof(BranchTable))]
        [ProducesResponseType(400, Type = typeof(BranchTable))]
        public async Task<IActionResult> DeleteBranch(int BranchCode)
        {
            //BranchStore.Branches.FirstOrDefault(u => u.Branch_Code == Branch_Code)
            //return Ok();
            if (BranchCode == 0)
            {
                return BadRequest();
            }
            var BranchToDelete = _BContext.BranchTable.FirstOrDefault(u => u.BranchCode == BranchCode);
            if (BranchToDelete == null)
            {
                //ModelState.AddModelError("CustomError", "Branch with such Branch Code does not exist ");
                return NotFound();
            }
            _BContext.BranchTable.Remove(BranchToDelete);
            await _BContext.SaveChangesAsync();
            return NoContent();

        }
    }
}
