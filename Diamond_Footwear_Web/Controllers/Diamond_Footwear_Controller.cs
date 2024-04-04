using Diamond_Footwear_Services.DBContext;
using Diamond_Footwear_Services.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diamond_Footwear_Web.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[action]")]
    [ApiController]
    public class Diamond_Footwear_Controller : ControllerBase
    {
        public readonly IRepository _service;

        public Diamond_Footwear_Controller(IRepository service)
        {
            this._service = service;
        }


        [HttpPost]
        public async Task<dynamic> SaveUserRoleMaster(SaveUserRoleMaster obj)
        {
            return Ok(await _service.SaveUserRoleMaster(obj));
        }

        [HttpGet("{MasterType}")]
        public async Task<dynamic> GetUserRoleMasters(int MasterType, int RoleId)
        {
            return Ok(await _service.GetUserRoleMaster(MasterType, RoleId));
        }

        [HttpDelete("{TranType:int}/{MasterType:int}/{Id:int}")]
        public async Task<dynamic> DeleteMasters(int TranType, int MasterType, int Id)
        {
            return Ok(await _service.DeleteMaster(TranType, MasterType, Id));
        }

        [HttpGet("{Username:required}/{Password:required}")]
        public async Task<IActionResult> ValidateUsers(string Username, string Password)
        {
            return Ok(await _service.ValidateUsers(Username, Password));
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
