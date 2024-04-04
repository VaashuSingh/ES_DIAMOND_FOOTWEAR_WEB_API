using Diamond_Footwear_Services.DBContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond_Footwear_Services.Services
{
    public class Repository : IRepository
    {
        public readonly DiamondFootwearWebContext _db;

        public readonly string busyDb  = "";
        private DateTime date;

        public Repository(DiamondFootwearWebContext db, IConfiguration config) 
        { 
            this._db = db;
#pragma warning disable CS8601 // Possible null reference assignment.
            this.busyDb = config.GetSection("BusyInfo").GetSection("BusyDbName").Value;
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        public async Task<dynamic> DeleteMaster(int TranType, int MasterType, int Id)
        {
            try
            {
                string sql = "";
                if (Id > 0) { 

                    if (TranType == 1)
                    {   if (! ValidRoleMasterIfExistsInUsers(Id))
                        {
                            sql = $"Delete From ESMASTER1 Where MasterType = {MasterType} And Code = {Id}";
                        }
                        else
                        {
                            return new { Status = 0, Msg = "This Role Name Tag In Some User Master !!!" };
                        }
                    }
                    else if (TranType == 2)
                    {
                        sql = $"Delete From ESUserMaster Where UserType = {MasterType} And Code = {Id}";
                    }
                    else if (TranType == 3)
                    {
                        sql = $"Delete From ESMASTER1 Where MasterType = {MasterType} And Code = {Id}";
                    }
                } 
                var Result = await _db.Database.ExecuteSqlRawAsync(sql);

                if (Result == 0) return new { Status = 0, Msg = "Master Not Exists ...." };
            }
            catch(Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString() };
            }
            return new { Status = 1, Msg = "Success" };
        }

        public async Task<dynamic> GetUserRoleMaster(int MasterType, int RoleId)
        {
            List<GetUserRoleMaster> RList = new List<GetUserRoleMaster>();
            try
            {
                string sql = "";
                if (RoleId == 0)
                {
                    sql = $"Select Code as RoleId,[Name],CONVERT(VARCHAR(10), CreatedOn, 105) as CreatedOn From ESMaster1 Where MasterType = {MasterType} Order By Name";
                }
                else
                {
                    sql = $"Select Code as RoleId,[Name],CONVERT(VARCHAR(10), CreatedOn, 105) From ESMaster1 Where MasterType = {MasterType} And Code = {RoleId} Order By Name";
                }

                RList = await _db.GetUserRoleMasters.FromSqlRaw(sql).ToListAsync();

                // Check if RList is empty and return appropriate message
                if (RList == null || RList.Count == 0) return new { Status = 0, Msg = "Data Not Found ....", Data = RList };
            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString(), Data = RList };
            }
            return new { Status = 1, Msg = "Success", Data = RList };
        }

        public async Task<dynamic> SaveUserRoleMaster(SaveUserRoleMaster Obj)
        {
            int Status = 0; string? StatusStr = ""; 
            try
            {
                DateTime dateTime = DateTime.Now; int MasterType = 1;

                SqlParameter role0 = new SqlParameter("@R0", Obj.Id);
                SqlParameter role1 = new SqlParameter("@R1", Obj.Name);
                SqlParameter role2 = new SqlParameter("@R2", MasterType);
                SqlParameter role3 = new SqlParameter("@R3", 'A');
                SqlParameter role4 = new SqlParameter("@R4", dateTime);
                
                var DT = await _db.Responses.FromSqlRaw("EXEC [Sp_SaveUserRoleMaster] @R0, @R1, @R2, @R3, @R4", role0, role1, role2, role3, role4).ToListAsync();

                Status = DT[0].Status;
                StatusStr = DT[0].Msg;
            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString() };
            }
            return new { Status = Status, Msg = StatusStr };
        }

        public async Task<dynamic> SaveUserMastDetails(SaveUsersMastDetail obj)
        {   
            int Status = 0; string? StatusStr = "";

            try
            {
                DateTime dateTime = DateTime.Now;
                string CreatedBy = ""; int UserType = 1;

                SqlParameter param0 = new SqlParameter("@p0", obj.UserId);
                SqlParameter param1 = new SqlParameter("@p1", obj.Username);
                SqlParameter param2 = new SqlParameter("@p2", obj.MobileNo);
                SqlParameter param3 = new SqlParameter("@p3", obj.EmailId);
                SqlParameter param4 = new SqlParameter("@p4", obj.PWD);
                SqlParameter param5 = new SqlParameter("@p5", obj.Desc);
                SqlParameter param6 = new SqlParameter("@p6", obj.RoleId);
                SqlParameter param7 = new SqlParameter("@p7", UserType);
                SqlParameter param8 = new SqlParameter("@p8", obj.Deactivate);
                SqlParameter param9 = new SqlParameter("@p9", obj.Base64);
                SqlParameter param10 = new SqlParameter("@p10", CreatedBy);
                SqlParameter param11 = new SqlParameter("@p11", dateTime);

                var DT1 = await _db.Responses.FromSqlRaw("EXEC [Sp_SaveUserMaster] @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11", param0, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11).ToListAsync();

                Status = DT1[0].Status;
                StatusStr = DT1[0].Msg;
            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString() };
            }
            return new { Status = Status, Msg = StatusStr };
        }

        public async Task<dynamic> GetUserMastDetails(int UserType, int UserId)
        {
            List<GetUserMasterDetail> UList = new List<GetUserMasterDetail>();
            try
            {
                string sql = "";
                if (UserId > 0)
                {
                    sql = $"select A.[UID] as UserId, IsNull(A.[UserName], '') as Username, IsNull(A.[Mobile],'') as MobileNo, IsNull(A.[Email], '') as EmailId, IsNull(A.[Password], '') as PWD, IsNull(A.[Role], 0) as RoleId,IsNull(B.[Name], '') as RoleName, IsNull(A.[Description], '') as [Desc], IsNull([DocAttach], '') as Doc1, IsNull([Base64], '') as [Image], IsNull(Active, 0) as Active, CONVERT(VARCHAR(10), A.CreatedOn, 105) as CreatedOn From ESUserMaster A Left Join ESMaster1 B On A.[Role] = B.[Code] And B.[MasterType] = 1 Where A.[UserType] = {UserType} And A.[UID] = {UserId} Order By A.[CreatedOn] ";
                }
                else
                {
                    sql = $"select A.[UID] as UserId, IsNull(A.[UserName], '') as Username, IsNull(A.[Mobile],'') as MobileNo, IsNull(A.[Email], '') as EmailId, IsNull(A.[Password], '') as PWD, IsNull(A.[Role], 0) as RoleId,IsNull(B.[Name], '') as RoleName, IsNull(A.[Description], '') as [Desc], IsNull([DocAttach], '') as Doc1, IsNull([Base64], '') as [Image], IsNull(Active, 0) as Active, CONVERT(VARCHAR(10), A.CreatedOn, 105) as CreatedOn From ESUserMaster A Left Join ESMaster1 B On A.[Role] = B.[Code] And B.[MasterType] = 1 Where A.[UserType] = {UserType} Order By A.[CreatedOn] ";
                }
                UList = await _db.GetUserMasterDetails.FromSqlRaw(sql).ToListAsync();

                if (UList == null || UList.Count == 0) return new { Status = 0, Msg = "Data Not Found !!!", Data = UList };

            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString(), Data = UList };
            }
            return new { Status = 1, Msg = "Success", Data = UList };
        }

        public bool ValidRoleMasterIfExistsInUsers(int id)
        {
            try
            {
                string sql = $"select Top 1 IsNull([Role],0) as Result, '' as Msg, 0 as Status From ESUserMaster Where [Role] = {id}";
                var Result = _db.Responses.FromSqlRaw(sql).FirstOrDefault();

                if (Result != null) 
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public Task<dynamic> ValidateUsers(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetOrderReceivedsDetails(int Series, int Party, string? StartDate, string? EndDate)
        {
            List<GetOrderReceivedDetail> RList = new List<GetOrderReceivedDetail>();
            try
            {
                string? busyComp = busyDb; string sql = "";
                string formattedStartDate = DiamondHelper.FormatDate(StartDate);
                string formattedEndDate = DiamondHelper.FormatDate(EndDate);

                if (busyComp.Length > 0)
                {
                    sql = $"Select Distinct A.[Code] as VchCode,IsNull(A.[Series],0) as SCode,IsNull(B.[Name],'') as SName,CONVERT(VARCHAR(10), A.[SoDate], 105) as VchDate,IsNull(A.[Prefix], '') as VchNo,IsNull(A.[AccCode], 0) as AccCode, IsNull(C.[Name], '') as AccName,IsNull(A.[MatCode], 0) as MCCode, IsNull(D.[Name], '') as MCName, IsNull([TotalQty],0) as TotQty, IsNull([TotalAltQty], 0) as TotAltQty, IsNull([TotalNetAmount],0) as TotAmt  From SoHeader A Left Join VchSeries B On A.Series = B.Code And B.TranType = 12 inner Join {busyComp}.dbo.Master1 C On A.AccCode = C.Code And C.MasterType = 2 Left Join {busyComp}.dbo.Master1 D On A.MatCode = D.Code And D.MasterType = 11 Where (A.Flag Is Null Or A.Flag = 0)";
                    if (Series > 0) sql += $" AND A.Series = {Series}";
                    if (Party > 0) sql += $" AND A.AccCode = {Party}";
                    if (StartDate?.Length > 0 && EndDate?.Length > 0) sql += $" AND A.SoDate >= '{formattedStartDate}' And A.SoDate <= '{formattedEndDate}' ";
                }
                RList = await _db.GetOrderReceivedDetails.FromSqlRaw(sql).ToListAsync();

                if (RList == null || RList.Count == 0) return new { Status = 0, Msg = "Data Not Found ....", Data = RList };
            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString(), Data = RList };
            }
            return new { Status = 1, Msg = "Success", Data = RList };
        }

        public async Task<dynamic> GetOrderReceivedItemsDetails(int VchCode)
        {
            List<GetOrderReceivedItemDetail> RList1 = new  List<GetOrderReceivedItemDetail>();
            try
            {
                string sql = $"Select IsNull([Item], 0) as ItemCode, B.[Name] as ItemName, IsNull([Para1], '') as Para1, IsNull([Para2], 0) as Para2, IsNull([Qty], 0) as Qty, IsNull([AltQty], 0) as AltQty, 0 as ClQty, IsNull(C.[Name], '') as Unit, IsNull(D.[Name], '') AltUnit, IsNull([Price], 0) as Price, IsNull([MRP], 0) as MRP, IsNull([Amount], 0) as Amount, IsNull(A.[IsCancel],0) as Status From SoDetails A Inner Join BusyComp0020_db12023.dbo.Master1 B On A.item = B.Code And B.MasterType = 6 Left Join BusyComp0020_db12023.dbo.Master1 C On A.Unit = C.Code And C.MasterType = 8 \r\nLeft Join BusyComp0020_db12023.dbo.Master1 D On A.AltUnit = D.Code And D.MasterType = 8 Where A.Code = {VchCode} Order By A.SrNo";
                RList1 = await _db.GetOrderReceivedItemDetails.FromSqlRaw(sql).ToListAsync();

                if (RList1 == null || RList1.Count == 0) return new { Status = 0, Msg = "Data Not Found ....", Data = RList1 };
            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString(), Data = RList1 };
            }
            return new { Status = 1, Msg = "Success", Data = RList1 };
        }

        public async Task<dynamic> UpdateOrderReceivedApproval(UpdateOrderReceivedApproval obj)
        {
            try
            {
                UpdateOrderReceivedApproval AData = new UpdateOrderReceivedApproval();
                AData.Remarks = obj.Remarks;
                AData.VchCode = obj.VchCode;

                string sql = $"update soheader Set [Flag] = 1, [Remarks] = '{AData.Remarks}'  Where Code = {AData.VchCode}";
                int result = await _db.Database.ExecuteSqlRawAsync(sql);

                if (result == 0) return new { Status = 0, Msg = "Unable to find order. Please verify the order details.!" };
            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString() };
            }
            return new { Status = 1, Msg = "Your request has been successfully approved. Thank you for your cooperation!" };
        }
    }
}
