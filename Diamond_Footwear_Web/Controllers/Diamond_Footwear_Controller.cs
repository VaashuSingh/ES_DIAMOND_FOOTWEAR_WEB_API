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

        [HttpPost]
        public async Task<IActionResult> SaveOrderAcceptTask(SaveOrderAcceptTaskHead obj)
        {
            return Ok(await _service.SaveOrderAcceptTasks(obj));
        }

        [HttpGet("{TaskType:int}")]
        public async Task<IActionResult> GetTaskApprovelVch(int TaskType)
        {
            return Ok(await _service.GetTaskApprovelVchDetails(TaskType));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderTaskApproval(SaveOrderTaskApproval obj)
        {
            return Ok(await _service.UpdateOrderTaskApproval(obj));
        }

        [HttpGet("{TaskType}/{TaskCode}/{VchCode}/{ItemCode}")]
        public async Task<IActionResult> GetOrderApprovelItemsHoldDetails(int TaskType, int TaskCode, int VchCode, int ItemCode)
        {
            return Ok(await _service.GetOrderApprovelItemHoldDetails(TaskType, TaskCode, VchCode, ItemCode));
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderStatusReport(int AccCode, int ItemCode, string? OrderNo, int Status, string? StartDate, string? EndDate)
        {
            return Ok(await _service.GetOrderStatusReports(AccCode, ItemCode, OrderNo, Status, StartDate, EndDate));
        }

        [HttpGet]
        public async Task<IActionResult> GetBusyMasterList(int TranType, int MasterType)
        {
            return Ok(await _service.GetBusyMasterLists(TranType, MasterType));
        }

        [HttpGet("{RoleId:int}")]
        public async Task<IActionResult> GetUserRolePermissionMenu(int RoleId)
        {
            return Ok(await _service.GetUserRolePermissionResponses(RoleId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserRolePermissionResponse(SaveRolePermissionResponse obj)
        {
            return Ok(await _service.SaveRolePermissionResponse(obj));
        }

        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetUserMenusResponse(int UserId)
        {
            return Ok(await _service.GetUserMenusResponse(UserId));
        }
    }
}
