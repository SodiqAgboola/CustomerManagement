using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.ViewModels;
using CustomerManagementSystem.ViewModels.Banks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Controllers.Banks
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly IBankService _bankService;

        public BanksController(IBankService bankService)
        {
            this._bankService = bankService;
        }

        [HttpGet("GetExistingBanks")]
        [ProducesResponseType(typeof(GenericResponse<GetBanksResponseVm>), 200)]
        public async Task<IActionResult> GetExistingBanks()
        {
            var result = await _bankService.GetExistingBanks();

            return Ok(result);
        }

    }
}
