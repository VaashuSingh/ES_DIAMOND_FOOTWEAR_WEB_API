using Diamond_Footwear_Services.DBContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Diamond_Footwear_Services.Services
{
    public class Repository : IRepository
    {
        public readonly DiamondFootwearWebContext _db;

        public readonly string busyDb = "";
        private DateTime date;

        public Repository(DiamondFootwearWebContext db, IConfiguration config)
        {
            this._db = db;
#pragma warning disable CS8601 // Possible null reference assignment.
            this.busyDb = config.GetSection("BusyInfo").GetSection("BusyDbName").Value;
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        private string GenerateToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<dynamic> ValidateUsers(ValidateUser obj)
        {
            ValidateUserDet UserDT = new ValidateUserDet(); int Status = 0; string StatusStr = ""; string token = null;
            try
            {
                ValidateUser User1 = new ValidateUser(); string sql = "";
                User1.UserName = obj.UserName; User1.Password = obj.Password;

                sql = $"SELECT TOP 1 [UID] as [UserId],ISNULL([UserName],'') as [Name], ISNULL([Base64],'') as [Images], ISNULL([UserType],0) as [UserType], ISNULL([Active],0) as [Active], ISNULL([Admin], 0) as [Admin] FROM ESUSERMASTER WHERE ([EMAIL] = '{User1.UserName}' OR MOBILE = '{User1.UserName}') AND [PASSWORD] = '{User1.Password}' And [IsDeleted] <> 1 ";
                var DT1 = await _db.ValidateUserDets.FromSqlRaw(sql).ToListAsync();

                if (DT1.Count == 0)
                {
                    sql = $"SELECT Top 1  0 as Status, '' as Msg, * FROM ESUSERMASTER WHERE ([EMAIL] = '{User1.UserName}' OR [MOBILE] = '{User1.UserName}') And [IsDeleted] <> 1";
                    var DT2 = await _db.Responses.FromSqlRaw(sql).FirstOrDefaultAsync();
                    if (DT2 != null)
                    {
                        sql = $"SELECT Top 1 0 as Status, '' as Msg, * FROM ESUSERMASTER WHERE ([EMAIL] = '{User1.UserName}' OR [MOBILE] = '{User1.UserName}') AND [PASSWORD] = '{User1.Password}' And [IsDeleted] <> 1";
                        var DT3 = await _db.Responses.FromSqlRaw(sql).FirstOrDefaultAsync();
                        if (DT3 == null)
                        {
                            Status = 0; StatusStr = "Wrong password. Try again or click ‘Forgot password’ to reset it. !";
                        }
                    }
                    else
                    {
                        Status = 0; StatusStr = "Couldn't find your username and password. !";
                    }
                }
                else
                {
                    if (DT1[0].Active == 1)
                    {
                        token = GenerateToken(User1.UserName); // Generate a JWT token
                        Status = 1; StatusStr = "Valid";
                    }
                    else
                    {
                        Status = 0; StatusStr = "Your user is currently inactive. !";
                    }

                    if (DT1[0].Active == 1)
                    {
                        UserDT.UserId = DT1[0].UserId;
                        UserDT.Name = DT1[0].Name;
                        UserDT.Images = DT1[0].Images;
                        UserDT.UserType = DT1[0].UserType;
                        UserDT.Active = DT1[0].Active;
                        UserDT.Admin = DT1[0].Admin;
                    }
                }
            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString(), Data = UserDT, Token = token };
            }
            return new { Status = Status, Msg = StatusStr, Data = UserDT, Token = token };
        }

        //public async Task<dynamic> DeleteMaster(int TranType, int MasterType, int Id)
        //{
        //    int Status = 0; string? StatusStr = "";
        //    try
        //    {
        //        SqlParameter param0 = new SqlParameter("@p0", TranType);
        //        SqlParameter param1 = new SqlParameter("@p1", MasterType);
        //        SqlParameter param2 = new SqlParameter("@p2", Id);


        //        if (Id > 0 && TranType > 0)
        //        {
        //            var DT1 = await _db.Responses.FromSqlRaw("EXEC [Sp_DeleteMaster] @p0, @p1, @p2", param0, param1, param2).ToListAsync();

        //            Status = DT1[0].Status;
        //            StatusStr = DT1[0].Msg;

        //            if (DT1[0].Status == 0)
        //            {
        //                return new { Status = 0, Msg = "Master Not Exists. !!!" };
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new { Status = 0, Msg = ex.Message.ToString() };
        //    }
        //    return new { Status = Status, Msg = StatusStr };
        //}

        public async Task<dynamic> DeleteMaster(int TranType, int MasterType, int Id)
        {
            try
            {
                string sql = "";
                if (Id > 0)
                {

                    if (TranType == 1)
                    {
                        if (!ValidRoleMasterIfExistsInUsers(Id))
                        {
                            sql = $"Delete From ESMASTER1 Where MasterType = {MasterType} And Code = {Id}";
                        }
                        else
                        {
                            return new { Status = 0, Msg = "This Role Name Tag In Some Users. !" };
                        }
                    }
                    else if (TranType == 2)
                    {
                        sql = $"Delete From ESUserMaster Where UserType = {MasterType} And UID = {Id}";
                    }
                    else if (TranType == 3)
                    {
                        sql = $"Delete From ESMASTER1 Where MasterType = {MasterType} And Code = {Id}";
                    }
                }
                var Result = await _db.Database.ExecuteSqlRawAsync(sql);

                if (Result == 0) return new { Status = 0, Msg = "Master Not Exists .!" };
            }
            catch (Exception ex)
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
                    sql = $"Select Code as RoleId,[Name],CONVERT(VARCHAR(10), CreatedOn, 105) as CreatedOn From ESMaster1 Where MasterType = {MasterType} And [IsDeleted] <> 1 Order By Name";
                }
                else
                {
                    sql = $"Select Code as RoleId,[Name],CONVERT(VARCHAR(10), CreatedOn, 105) From ESMaster1 Where MasterType = {MasterType} And Code = {RoleId} And [IsDeleted] <> 1 Order By Name";
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
                    sql = $"select A.[UID] as UserId, IsNull(A.[UserName], '') as Username, IsNull(A.[Mobile],'') as MobileNo, IsNull(A.[Email], '') as EmailId, IsNull(A.[Password], '') as PWD, IsNull(A.[Role], 0) as RoleId,IsNull(B.[Name], '') as RoleName, IsNull(A.[Description], '') as [Desc], IsNull([DocAttach], '') as Doc1, IsNull([Base64], '') as [Image], IsNull(Active, 0) as Active, CONVERT(VARCHAR(10), A.CreatedOn, 105) as CreatedOn From ESUserMaster A Left Join ESMaster1 B On A.[Role] = B.[Code] And B.[MasterType] = 1 Where A.[UserType] = {UserType} And A.[UID] = {UserId} And A.[IsDeleted] <> 1 Order By A.[CreatedOn] ";
                }
                else
                {
                    sql = $"select A.[UID] as UserId, IsNull(A.[UserName], '') as Username, IsNull(A.[Mobile],'') as MobileNo, IsNull(A.[Email], '') as EmailId, IsNull(A.[Password], '') as PWD, IsNull(A.[Role], 0) as RoleId,IsNull(B.[Name], '') as RoleName, IsNull(A.[Description], '') as [Desc], IsNull([DocAttach], '') as Doc1, IsNull([Base64], '') as [Image], IsNull(Active, 0) as Active, CONVERT(VARCHAR(10), A.CreatedOn, 105) as CreatedOn From ESUserMaster A Left Join ESMaster1 B On A.[Role] = B.[Code] And B.[MasterType] = 1 Where A.[UserType] = {UserType} And A.[IsDeleted] <> 1 Order By A.[CreatedOn] ";
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
                string sql = $"select Top 1 IsNull([Role],0) as Result, '' as Msg, 0 as Status From ESUserMaster Where [Role] = {id} And IsDeleted <> 1";
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
                    sql = $"Select Distinct A.[Code] as VchCode,IsNull(A.[Series],0) as SCode,IsNull(B.[Name],'') as SName,CONVERT(VARCHAR(10), A.[SoDate], 105) as VchDate,IsNull(A.[Prefix], '') as VchNo,IsNull(A.[AccCode], 0) as AccCode, IsNull(C.[Name], '') as AccName,IsNull(A.[MatCode], 0) as MCCode, IsNull(D.[Name], '') as MCName, IsNull([TotalQty],0) as TotQty, IsNull([TotalAltQty], 0) as TotAltQty, IsNull([TotalNetAmount],0) as TotAmt  From SoHeader A Left Join VchSeries B On A.Series = B.Code And B.TranType = 12 inner Join {busyComp}.dbo.Master1 C On A.AccCode = C.Code Left Join {busyComp}.dbo.Master1 D On A.MatCode = D.Code Where (A.Flag Is Null Or A.Flag = 0)";
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
            List<GetOrderReceivedItemDetail> RList1 = new List<GetOrderReceivedItemDetail>();
            try
            {
                string? busyComp = busyDb;
                //string sql = $"Select A.[Code] as VchCode, IsNull([Item], 0) as ItemCode, B.[Name] as ItemName, IsNull([Para1], '') as Color, IsNull([Para2], 0) as Size, IsNull([Qty], 0) as Qty, IsNull([AltQty], 0) as AltQty,CONVERT(FLOAT, 0) as ClQty, IsNull(C.[Name], '') as Unit, IsNull(D.[Name], '') AltUnit, IsNull([Price], 0) as Price, IsNull([MRP], 0) as MRP, IsNull([Amount], 0) as Amount, IsNull(A.[IsCancel],0) as Status From SoDetails A Left Join BusyComp0020_db12023.dbo.Master1 B On A.item = B.Code Left Join BusyComp0020_db12023.dbo.Master1 C On A.Unit = C.Code Left Join BusyComp0020_db12023.dbo.Master1 D On A.AltUnit = D.Code Where A.Code = {VchCode} Order By A.[SrNo],A.[Code]";
                string sql = $"Select A.[Code] as VchCode, CONVERT(VARCHAR, A.[SODate], 105) as [VchDate], IsNull(A.[Prefix], '') as VchNo, IsNull(A.[Series], 0) as VchSeries, IsNull(A.[CustPo], '') as PoNo, IsNull(A.[AccCode],0) as AccCode, IsNull(B.[Item], 0) as ItemCode, C.[Name] as ItemName, IsNull(B.[CM1], 0) as MCode1,IsNull(F.[Name],'') as MName1, IsNull(B.[CM2], 0) as MCode2, IsNull(G.[Name],'') as MName2, IsNull(B.[CM3], 0) as MCode3, IsNull(H.[Name],'') as MName3, IsNull(B.[Para1], '') as Color, IsNull(B.[Para2], 0) as Size, IsNull(B.[Qty], 0) as Qty, IsNull(B.[AltQty], 0) as AltQty, IsNull(B.[GrQty], 0) as ClQty, IsNull(B.[Unit], 0) as UCode, IsNull(D.[Name], '') as Unit, IsNull(E.[Name], '') AltUnit, IsNull(B.[Price], 0) as Price,IsNull(B.[MRP], 0) as MRP, IsNull(B.[Amount], 0) as Amount, 0 as [Status] From SoHeader A Inner Join SoDetails B On A.Code = B.Code Left Join {busyComp}.dbo.Master1 C On B.item = C.Code Left Join {busyComp}.dbo.Master1 D On B.Unit = D.Code Left Join {busyComp}.dbo.Master1 E On B.AltUnit = E.Code Left Join ESTechMaster F On B.[CM1] = F.Code Left Join ESTechMaster G On B.[CM2] = G.Code Left Join ESTechMaster H On B.[CM3] = H.Code Where A.Code = {VchCode} Group By A.[Code], A.[SODate], A.[Prefix], A.[Series], A.[CustPo], A.[AccCode], B.[Item], C.[Name],B.[CM1], B.[CM2], B.[CM3], F.[Name], G.[Name], H.[Name],B.[Para1], B.[Para2], B.[Qty], B.[AltQty], B.[GrQty], B.[Unit], D.[Name], E.[Name], B.[Price], B.[MRP], B.[Amount], B.[SrNo] ";
                RList1 = await _db.GetOrderReceivedItemDetails.FromSqlRaw(sql).ToListAsync();

                if (RList1 == null || RList1.Count == 0) return new { Status = 0, Msg = "Data Not Found ....", Data = RList1 };
            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString(), Data = RList1 };
            }
            return new { Status = 1, Msg = "Success", Data = RList1 };
        }

        public async Task<dynamic> SaveOrderAcceptTasks(SaveOrderAcceptTaskHead obj)
        {
            int Status = 0; string StatusStr = string.Empty;
            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                string TaskXml = DiamondHelper.CreateXml(obj.OrderAcceptTask);
#pragma warning restore CS8604 // Possible null reference argument.

                SqlParameter param0 = new SqlParameter("@p0", TaskXml.ToString());

                var TD1 = await _db.Responses.FromSqlRaw("EXEC SP_SaveOrderAcceptTask @p0", param0).ToListAsync();

                Status = TD1[0].Status;
                StatusStr = TD1[0].Msg;
            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString() };
            }
            return new { Status = Status, Msg = StatusStr };
        }

        public async Task<dynamic> GetTaskApprovelVchDetails(int TaskType)
        {
            try
            {
                string busyComp = busyDb;
                string sql = $"Select A.[VchCode], CONVERT(VARCHAR, A.[VchDate], 105) as VchDate, IsNull(A.[VchNo], '') as VchNo,IsNull(F.[Name], '') as VchSeries, IsNull(C.[Name], '') As AccName, IsNull(B.[PoNo], '') as PoNo, IsNull(A.[Person], '') as Person, IsNull(B.Code,0) as TaskCode, IsNull(B.[TaskId], 0) as TaskId, IsNull(B.[TaskDesc], '') as TaskDesc, CONVERT(VARCHAR, B.[TaskDate], 105) as TaskDate, IsNull(B.[Remark], '') as Remark,IsNull(B.[ItemCode], 0) as ItemCode, IsNull(D.[Name], '') as ItemName, IsNull(E.[Name], '') as Unit,IsNull(G.[Name],'') as MName1, IsNull(H.[Name],'') as MName2, IsNull(I.[Name], '') as MName3, IsNull(B.[Param1], '') as Color, IsNull(B.[Param2], '') as Size, IsNull(B.[Qty], 0) as Qty, IsNull(B.[AltQty], 0) as AltQty, IsNull(A.[Value4], 0) as MRP, IsNull(A.[Value5], 0) as Price, IsNull(A.[Value6], 0) as Amount, IsNull(B.[Status],0) as TaskStatus From ESOrderTask A Inner Join ESOrderTaskDetails B On A.Code = B.Code And A.VchCode = B.VchCode Left Join {busyComp}.dbo.Master1 C On A.MasterCode1 = C.Code Left Join {busyComp}.dbo.Master1 D On B.ItemCode = D.Code Left Join {busyComp}.dbo.Master1 E On A.Unit = E.Code Left Join Vchseries F On A.VchSeries = F.Code Left Join ESTechmaster G On A.CM1 = G.Code Left Join ESTechmaster H On A.CM2 = H.Code Left Join ESTechmaster I On A.CM3 = I.Code Where (B.[Status] Is Null Or B.[Status] In (0,2)) And B.TaskId = {TaskType} Order By A.VchDate, A.VchCode";

                var data = await _db.GetOrderApprovelVches.FromSqlRaw(sql).ToListAsync();

                if (data == null || data.Count == 0)
                {
                    return new { Status = 0, Msg = "Data Not Found. !!!", Data = data };
                }
                else
                {
                    return new { Status = 1, Msg = "Success", Data = data };
                }

            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString() };
            }
        }

        public async Task<dynamic> UpdateOrderTaskApproval(SaveOrderTaskApproval obj)
        {
            try
            {
                SaveOrderTaskApproval task = new SaveOrderTaskApproval();
                string date = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
                task.VchCode = obj.VchCode; task.TaskCode = obj.TaskCode; task.Status = obj.Status; task.Remark = obj.Remark; task.TaskId = obj.TaskId; task.Users = obj.Users; task.CreatedOn = obj.CreatedOn;

                string sql = $"UPDATE [ESORDERTASKDETAILS] Set [Status] = {task.Status}, [CompletedNarr] = '{task.Remark}', [CompletedBy] = '{task.Users}', [CompletedOn] = '{task.CreatedOn}' Where VchCode = {task.VchCode} And Code = {task.TaskCode} And TaskId = {task.TaskId}";
                int result = await _db.Database.ExecuteSqlRawAsync(sql);


                if (result == 0)
                {
                    return new { Status = 0, Msg = "Unable to find order. Please verify the order details.!" };

                }
                else
                {
                    bool validate = await ValidateTaskStatusCompletion(task.VchCode, task.TaskCode);
                    if (validate)
                    {
                        sql = $"Update [ESORDERTASK] Set [Status] = 1 Where VchCode = {task.VchCode} And Code = {task.TaskCode} ";
                        int res = await _db.Database.ExecuteSqlRawAsync(sql);
                    }
                    return new { Status = 1, Msg = "Your request has been successfully approved. Thank you for your cooperation!" };
                }
            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString() };
            }
        }

        public async Task<bool> ValidateTaskStatusCompletion(int VchCode, int TaskCode)
        {
            try
            {
                var results = new List<Results>();
                string sql = $"Select IsNull([Status], 0) as [Result] From ESOrderTaskDetails Where VchCode = {VchCode} And Code = {TaskCode}";
                results = await _db.Resultss.FromSqlRaw(sql).ToListAsync();

                if (results.Count == 0)
                {
                    return false;
                }
                else
                {
                    foreach (var result in results)
                    {
                        if (result.Result != 1 && result.Result != 3)
                        {
                            // If any result is not equal to 1, return true
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<dynamic> GetOrderApprovelItemHoldDetails(int TaskType, int TaskCode, int VchCode, int ItemCode)
        {
            try
            {
                string sql = $"Select IsNull([Status], 0) as [Action],IsNull([CompletedNarr], '') as Remark, CONVERT(VARCHAR, CompletedOn, 105) as Date From ESOrderTaskDetails Where VchCode = {VchCode} And Code = {TaskCode} And TaskId = {TaskType} And Status = 2 And ItemCode = {ItemCode}";
                var DT1 = await _db.GetApprovelHoldDets.FromSqlRaw(sql).ToListAsync();

                if (DT1 != null && DT1.Count == 0)
                {
                    return new { Status = 0, Msg = "Data Not Found", Data = DT1 };
                }
                else
                {
                    return new { Status = 1, Msg = "Success", Data = DT1 };
                }

            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString() };
            }

        }

        public async Task<dynamic> GetOrderStatusReports(int AccCode, int ItemCode, string? OrderNo, int Status, string? StartDate, string? EndDate)
        {
            try
            {
                string sql = string.Empty;
                string busyComp = busyDb;
                int ActualStatus = 0;
                if (Status > 0)
                {
                    if (Status == 1)
                    {
                        ActualStatus = 1;
                    }
                    else if (Status == 2)
                    {
                        ActualStatus = 0;
                    }
                }

                sql = $"Select A.[VchCode], A.[Code] as TaskCode, CONVERT(VARCHAR, A.VchDate, 105) as VchDate, IsNull(A.[VchNo], '') as VchNo, IsNull(A.[MasterCode1], 0) as AccCode,IsNull(B.[Name], '') as AccName, IsNull(A.[MasterCode2], 0) as ItemCode, IsNull(C.[Name], '') as ItemName, IsNull(A.[Param1], '') as Color, IsNull(A.[Param2], '') as Size, IsNull(A.[Value1], 0) as Qty, IsNull(A.[Value2], 0) as AltQty, IsNull(A.[Value4], 0) as MRP, IsNull(A.[Status], 0) as [Status], IsNull(A.[Person], '') as Person From ESOrderTask A Left Join {busyComp}.Dbo.Master1 B On A.[MasterCode1] = B.[Code] Left Join {busyComp}.Dbo.Master1 C On A.[MasterCode2] = C.[Code] Where 1 = 1 ";
                if (AccCode > 0) sql += $" And A.[MasterCode1] = {AccCode} ";
                if (ItemCode > 0) sql += $" And A.[MasterCode2] = {ItemCode} ";
                if (OrderNo?.Length > 0) sql += $" And A.[VchNo] = '{OrderNo}'";
                if (ActualStatus > 0) sql += $" And A.Status= {ActualStatus}";

                if (StartDate?.Length > 0 && EndDate?.Length > 0) sql += $" And A.VchDate >= '{StartDate}' And A.VchDate <= '{EndDate}'";
                sql += "Order By A.VchCode, A.VchDate";

                var DT1 = await _db.GetOrderStatusRpts.FromSqlRaw(sql).ToListAsync();

                if (DT1 != null && DT1.Count == 0)
                {
                    return new { Status = 0, Msg = "Data Not Found. ", Data = DT1 };
                }
                else
                {
                    return new { Status = 1, Msg = "Success", Data = DT1 };
                }
            }
            catch (Exception err)
            {
                return new { Status = 0, Msg = err.Message.ToString() };
            }

        }

        public async Task<dynamic> GetBusyMasterLists(int TranType, int MasterType)
        {
            try
            {
                string busyComp = busyDb; string sql = string.Empty;
                if (TranType == 1)
                {
                    sql = $"select CONVERT(INT, ROW_NUMBER() OVER (ORDER BY [VchNo])) As Value, IsNull([VchNo], '') as [Label] From ESOrderTask Group By [VchNo] Order By [VchNo]";
                }
                else if (TranType == 2)
                {
                    sql = $"select CONVERT(INT, ROW_NUMBER() OVER (ORDER BY [VchNo])) As Value, IsNull([VchNo], '') as [Label] From ESOrderTask Group By [VchNo] Order By [VchNo]";
                }
                else
                {
                    sql = $"select Top 70 IsNull([Code], 0) as Value, IsNull([Name], '') as Label From {busyComp}.Dbo.Master1 Where MasterType = {MasterType} Group By [Code], [Name] Order By [Name]";
                }

                var Res1 = await _db.GetBusyMasterLists.FromSqlRaw(sql).ToListAsync();

                if (Res1 != null && Res1.Count == 0)
                {
                    return new { Status = 0, Msg = "Data Not Found. ", Data = Res1 };
                }
                else
                {
                    return new { Status = 1, Msg = "Success", Data = Res1 };
                }

            }
            catch (Exception err)
            {
                return new { Status = 0, Msg = err.Message.ToString() };
            }
        }

        //public async Task<dynamic> GetRolePermissionResponses(int RoleId)
        //{
        //    try
        //    {
        //        string sql = $"Select IsNull([MenuId], 0) as [Key], IsNull([I1], 0) as [Create], IsNull([I2], 0) as [Edit], IsNull([I3], 0) as [View], IsNull([I4], 0) as [Delete] From ESUserRolePermission Where MasterCode = {RoleId}";
        //        var DT1 = await _db.PermissionResponses.FromSqlRaw(sql).ToListAsync();

        //        if (DT1 == null || DT1.Count == 0)
        //        {
        //            return new { Status = 0, Msg = "Data Not Found. ", Data = DT1 };
        //        }
        //        else
        //        {
        //            List<GetRolePermissionResponse> ABH = new List<GetRolePermissionResponse>();

        //            foreach (var DT2 in DT1)
        //            {
        //                GetRolePermissionResponse RList = new GetRolePermissionResponse();
        //                RList.Key = DT2.Key;
        //                RList.Permissions = new List<Permission> { new Permission { Create = DT2.Create, Edit = DT2.Edit,  View = DT2.View, Delete = DT2.Delete } };
        //                ABH.Add(RList);
        //            }

        //            return new { Status = 1, Msg = "Success", Data = ABH };
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        return new { Status = 0, Msg = err.Message.ToString() };
        //    }
        //}

        public async Task<dynamic> GetUserRolePermissionResponses(int RoleId)
        {
            try
            {
                string sql = string.Empty;

                sql = $"Select DISTINCT IsNull(A.[Code], 0) as MenuId, IsNull(A.[Name], '') as Menu, IsNull(B.[I1], 0) as [Create], IsNull(B.[I2], 0) as Edit, IsNull(B.[I3], 0) as [View], IsNull(B.[I4], 0) as [Delete] From [ESMENUMASTER] A Left Join [ESUserRights] B On A.[Code] = B.[MasterCode2] Where B.[MasterCode1] = {RoleId} And TranType = 2 ";
                var DT1 = await _db.GetUserRolePermissionMenus.FromSqlRaw(sql).ToListAsync();

                if (DT1 != null && DT1.Count == 0)
                {
                    sql = "Select IsNull(A.[Code], 0) as MenuId, IsNull(A.[Name], '') as Menu, 0 as [Create], 0 as Edit, 0 as [View], 0 as [Delete] From [ESMENUMASTER] A Left Join [ESUserRights] B On A.[Code] = B.MasterCode2 Where A.[TranType] = 2 Group By A.[Code], A.[Name] Order By A.[Code]";
                    var DT2 = await _db.GetUserRolePermissionMenus.FromSqlRaw(sql).ToListAsync();

                    if (DT2.Count == 0)
                    {
                        return new { Status = 0, Msg = "Sorry, no results found! ", Data = DT2 };
                    }
                    else
                    {
                        return new { Status = 1, Msg = "Success ", Data = DT2 };
                    }
                }
                else
                {
                    return new { Status = 1, Msg = "Success", Data = DT1 };
                }
            }
            catch (Exception err)
            {
                return new { Status = 0, Msg = err.Message.ToString() };

            }
        }

        public async Task<dynamic> SaveRolePermissionResponse(SaveRolePermissionResponse obj)
        {
            try
            {
                string xml = DiamondHelper.CreateXml(obj.PermissionData);
                SqlParameter param0 = new SqlParameter("@p0", obj.RoleId);
                SqlParameter param1 = new SqlParameter("@p1", xml.ToString());

                var RT1 = await _db.Responses.FromSqlRaw("EXEC Dbo.[Sp_SaveMenuMasterPermission] @p0, @p1", param0, param1).ToListAsync();

                int Status = RT1[0].Status;
                string StatusStr = RT1[0].Msg;

                if (Status == 0)
                {
                    return new { Status = 0, Msg = "Some Error Please Check Your JSON ...." };
                }
                else
                {
                    return new { Status = 1, Msg = StatusStr };
                }
            }
            catch (Exception err)
            {
                return new { Status = 0, Msg = err.Message.ToString() };
            }
        }

        public async Task<dynamic> GetUserMenusResponse(int userId)
        {
            try
            {
                List<UserMenus> menus = await GetSubUserMenu(userId, 0, 0, 0, 1);
                return new { Status = 1, Msg = "Success", Data = menus };
            }
            catch (Exception ex)
            {
                return new { Status = 0, Msg = ex.Message.ToString(), Data = new List<UserMenus>() };
            }
        }

        public async Task<List<UserMenus>> GetSubUserMenu(int UserId, int SubMenu, int ParentId, int PMenuOrd, int TranType)
        {
            List<UserMenus> Menus = new List<UserMenus>();
            string sql = string.Empty;
            if (TranType == 1)
            {
                sql = $"Select CAST(1 AS BIT) as SubmenuOpen, CAST(0 AS BIT) as showSubRoute, CAST(0 AS BIT) as submenu, 0 as TranType, 0 as PMenuOrd, M.[Ordering2] as SubMenuNo, 0 as MenuId, (Case When M.Ordering2 = 1 then 'Main' When M.Ordering2 = 2 then 'Transaction' When M.Ordering2 = 3 then 'Report' When M.Ordering2 = 4 then 'User Management' else '' End) as Label,(Case When M.Ordering2 = 1 then 'Main' When M.Ordering2 = 2 then 'Transaction' When M.Ordering2 = 3 then 'Report' When M.Ordering2 = 4 then 'User Management' else '' End) as SubmenuHdr,  0 ParentID,'' as [Link], '' as Icon, 0 as [Right1], 0 as [Right2], 0 as [Right3], 0 as [Right4], 0 as [Right5] From ESUserMaster A Inner join ESUserRights U On A.[Role] = U.[MasterCode1] Inner Join ESMenuMaster M On U.[MasterCode2] = M.[Code] Where A.[UID] = {UserId} And (IsNull(I1,0) = 1 Or IsNull(I2,0) = 1 Or IsNull(I3,0) = 1 Or IsNull(I4,0) = 1 Or IsNull(I5,0) = 1) Group by M.Ordering2 Order By M.Ordering2";
            }
            else if (TranType == 2)
            {
                sql = $"Select CAST(1 AS BIT) as SubmenuOpen, CAST(0 AS BIT) as showSubRoute, CAST(0 AS BIT) as submenu,IsNull(M1.[TranType],0) as TranType, M.[Ordering3] as PMenuOrd, M.[Ordering2] as SubMenuNo, M.[ParentId] as MenuId, IsNull(M1.Name,'') as Label, IsNull(M1.Name,'') as SubmenuHdr, 0 ParentID, '' as [Link], '' as Icon, 0 as [Right1], 0 as [Right2], 0 as [Right3], 0 as [Right4], 0 as [Right5] From ESUSERMASTER A INNER JOIN ESUSERRIGHTS U ON A.[ROLE] = U.[MASTERCODE1] INNER JOIN ESMENUMASTER M On U.[MASTERCODE2] = M.[CODE] LEFT JOIN ESMENUMASTER M1 On M.PARENTID = M1.[CODE] WHERE A.[UID] = {UserId} And M.[Ordering2] = {SubMenu} And (IsNull(I1,0) = 1 Or IsNull(I2,0) = 1 Or IsNull(I3,0) = 1 Or IsNull(I4,0) = 1 Or IsNull(I5,0) = 1) Group By M.[ORDERING3],M.[PARENTID],M1.[Name],M1.[TranType],M.[ORDERING2] Order By M.[ORDERING3]";
            }
            else if (TranType == 3)
            {
                sql = $"Select CAST(1 AS BIT) as SubmenuOpen, CAST(0 AS BIT) as showSubRoute, CAST(0 AS BIT) as submenu, IsNull(M.[TranType], 0) as TranType, M.[Ordering3] as PMenuOrd, M.[Ordering2] as SubMenuNo, U.[MasterCode2] as MenuId, IsNull(M.[Name], '') as Label, IsNull(M.[Name], '') as SubmenuHdr,M.ParentID, IsNull(M.[Path], '') as Link, IsNull(M.[MenuIcon], '') as Icon, IsNull(I1,0) as Right1, IsNull(I2,0) as Right2, IsNull(I3,0) as Right3, IsNull(I4,0) as Right4, IsNull(I5,0) as Right5 From ESUSERMASTER A INNER JOIN ESUSERRIGHTS U On A.[Role] = U.[MasterCode1] INNER JOIN ESMENUMASTER M ON U.MASTERCODE2 = M.CODE Where A.[UID] = {UserId} And M.[Ordering2] = {SubMenu} And M.[Ordering3] = {PMenuOrd} And M.[ParentId] = {ParentId} And (IsNull(I1,0) = 1 Or IsNull(I2,0) = 1 Or IsNull(I3,0) = 1 Or IsNull(I4,0) = 1 Or IsNull(I5,0) = 1) Order By M.[ORDERING3],M.[Ordering1]";
            }
            var DT1 = await _db.UserMenuss.FromSqlRaw(sql).ToListAsync();

            UserMenus menu = new UserMenus();
            foreach (var item in DT1)
            {
                if (TranType == 2 && item.MenuId > 0)
                {
                    menu = new UserMenus();
                    menu.MenuId = item.MenuId;
                    menu.Label = item.Label;
                    menu.Link = item.Link;
                    menu.Icon = DiamondHelper.StringManipulatorExtractFirstTag(item.Icon);
                    menu.SubmenuOpen = item.TranType == 1 ? false: item.SubmenuOpen;
                    menu.ShowSubRoute = item.ShowSubRoute;
                    menu.Submenu = item.TranType == 1 ? true : item.Submenu;
                    menu.SubmenuHdr = item.SubmenuHdr;
                    menu.ParentId = item.ParentId;
                    menu.PMenuOrd = item.PMenuOrd;
                    menu.SubMenuNo = item.SubMenuNo;
                    menu.TranType = item.TranType;
                    menu.SubmenuItems = await GetSubUserMenu(UserId, item.SubMenuNo, item.MenuId, item.PMenuOrd, TranType + 1);
                    Menus.Add(menu);
                }
                else
                {
                    if (TranType == 1 || TranType == 3)
                    {
                        menu = new UserMenus();
                        menu.MenuId = item.MenuId;
                        menu.Label = item.Label;
                        menu.Link = item.Link;
                        menu.Icon = DiamondHelper.StringManipulatorExtractFirstTag(item.Icon);
                        menu.SubmenuOpen = item.TranType == 1 ? false : item.SubmenuOpen;
                        menu.ShowSubRoute = item.ShowSubRoute;
                        menu.Submenu = item.TranType == 1 ? true : item.Submenu;
                        menu.SubmenuHdr = item.SubmenuHdr;
                        menu.ParentId = item.ParentId;
                        menu.PMenuOrd = item.PMenuOrd;
                        menu.SubMenuNo = item.SubMenuNo;
                        menu.TranType = item.TranType;
                        menu.SubmenuItems = new List<UserMenus>();
                        if (TranType != 3) menu.SubmenuItems = await GetSubUserMenu(UserId, item.SubMenuNo, item.ParentId, item.PMenuOrd, TranType + 1);
                        Menus.Add(menu);
                    }
                    else
                    {
                        List<UserMenus> SubM = await GetSubUserMenu(UserId, item.SubMenuNo, 0, item.PMenuOrd, TranType + 1);
                        foreach (var U in SubM) { Menus.Add(U); }
                    }
                }
            }
            return Menus;
        }
    }

}
