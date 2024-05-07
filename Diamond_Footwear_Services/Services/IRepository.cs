using Diamond_Footwear_Services.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond_Footwear_Services.Services
{
    public interface IRepository
    {
        public Task<dynamic> ValidateUsers(ValidateUser obj);
        public Task<dynamic> SaveUserRoleMaster(SaveUserRoleMaster Obj);
        public Task<dynamic> GetUserRoleMaster(int MasterType, int RoleId);
        public Task<dynamic> DeleteMaster(int TranType, int MasterType, int Id);
        public Task<dynamic> SaveUserMastDetails(SaveUsersMastDetail obj);
        public Task<dynamic> GetUserMastDetails(int UserType, int UserId);
        public Task<dynamic> GetOrderReceivedsDetails(int Series, int Party, string? StartDate, string? EndDate);
        public Task<dynamic> GetOrderReceivedItemsDetails(int VchCode);
        public Task<dynamic> UpdateOrderTaskApproval(SaveOrderTaskApproval obj);
        public Task<dynamic> SaveOrderAcceptTasks(SaveOrderAcceptTaskHead obj);
        public Task<dynamic> GetTaskApprovelVchDetails(int TaskType);
        public Task<dynamic> GetOrderApprovelItemHoldDetails(int TaskType, int TaskCode, int VchCode, int ItemCode);

    }
}
