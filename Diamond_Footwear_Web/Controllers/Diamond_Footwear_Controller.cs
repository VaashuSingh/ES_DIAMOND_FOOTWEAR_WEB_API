using Diamond_Footwear_Services.DBContext;
using Diamond_Footwear_Services.Services;
using Diamond_Footwear_Web.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Diamond_Footwear_Web.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[action]")]
    [ApiController]
    public class Diamond_Footwear_Controller : ControllerBase
    {
        public readonly IRepository _service;
        private readonly TokenService _tokenService;

        public Diamond_Footwear_Controller(IRepository service, TokenService tokenService)
        {
            this._service = service;
            this._tokenService = tokenService;
        }

        [HttpPost("generate-token")]
        public IActionResult GenerateToken()
        {
            var tokenString = _tokenService.GenerateTokenAuto();
            return Ok(new { Token = tokenString });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateUser(ValidateUser obj)
        {
            return Ok(await _service.ValidateUsers(obj));
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserRoleMaster(SaveUserRoleMaster obj)
        {
            return Ok(await _service.SaveUserRoleMaster(obj));
        }

        [HttpGet("{MasterType}")]
        public async Task<IActionResult> GetUserRoleMasters(int MasterType, int RoleId)
        {
            return Ok(await _service.GetUserRoleMaster(MasterType, RoleId));
        }

        [HttpGet("{TranType}/{MasterType}/{Id}")]
        public async Task<IActionResult> DeleteMasters(int TranType, int MasterType, int Id)
        {
            return Ok(await _service.DeleteMaster(TranType, MasterType, Id));
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserMastersDetails(SaveUsersMastDetail obj)
        {
            return Ok(await _service.SaveUserMastDetails(obj));
        }

        [HttpGet("{UserType:int}")]
        public async Task<IActionResult> GetUserMasterDetails(int UserType, int UserId)
        {
            return Ok(await _service.GetUserMastDetails(UserType, UserId));
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderReceivedDetails(int Series, int Party, string? StartDate, string? EndDate)
        {
            return Ok(await _service.GetOrderReceivedsDetails(Series, Party, StartDate, EndDate));
        }

        [HttpGet("{VchCode:int}")]
        public async Task<IActionResult> GetOrderReceivedItemsDetails(int VchCode)
        {
            return Ok(await _service.GetOrderReceivedItemsDetails(VchCode));
        }

        [HttpPut]
        public async Task<IActionResult>UpdateOrderReceivedApprovals(UpdateOrderReceivedApproval obj)
        {
            return Ok(await _service.UpdateOrderReceivedApproval(obj));
        }
    }
}
