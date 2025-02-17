﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond_Footwear_Services.DBContext
{
    public class Response
    {
        public int Status { get; set; }
        public string? Msg { get; set; }
    }

    public partial class Results
    {
        public int Result { get; set; }
    }

    public class SaveUserRoleMaster
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    public class GetUserRoleMaster
    {
        public int RoleId { get; set; }
        public string? Name { get; set; }
        public string? CreatedOn { get; set; }
    }
    public class GetOrderReceivedDetail
    {
        public int VchCode { get; set; }
        public int SCode { get; set; }
        public string? SName { get; set; }
        public string? VchDate { get; set; }
        public string? VchNo { get; set; }
        public int AccCode { get; set; }
        public string? AccName { get; set; }
        public int MCCode { get; set; }
        public string? MCName { get; set; }
        public double TotQty { get; set; }
        public double TotAltQty { get; set; }
        public double TotAmt { get; set; }

    }
    public class GetOrderReceivedItemDetail
    {
        public int VchCode { get; set; }
        public string? VchDate { get; set; }
        public string? VchNo { get; set; }
        public int vchSeries { get; set; }
        public string? PoNo { get; set; }
        public int AccCode { get; set; } = 0;
        public int ItemCode { get; set; }
        public string? ItemName { get; set; }
        public int MCode1 { get; set; }
        public int MCode2 { get; set; }
        public int MCode3 { get; set; }
        public string? MName1 { get; set; }
        public string? MName3 { get; set; }
        public string? MName2 { get; set; }
        public int UCode { get; set; }
        public string? Unit { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public double Qty { get; set; }
        public string? AltUnit { get; set; }
        public double AltQty { get; set; }
        public double ClQty { get; set; }
        public double Price { get; set; }
        public double MRP { get; set; }
        public double Amount { get; set; }
        public int Status { get; set; }
    }

    public partial class OrderAcceptTask
    {
        public int VchCode { get; set; }
        public string? VchDate { get; set; }
        public string? VchNo { get; set; }
        public int VchSeries { get; set; }
        public string? PoNo { get; set; }
        public int AccCode { get; set; }
        public int ItemCode { get; set; }
        public int MCode1 { get; set; }
        public int MCode2 { get; set; }
        public int MCode3 { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int UCode { get; set; }
        public float Qty { get; set; } = 0;
        public double AltQty { get; set; } = 0;
        public double ClQty { get; set; } = 0;
        public double Price { get; set; } = 0;
        public double MRP { get; set; } = 0;
        public double Amount { get; set; } = 0;

        public string? Mrp_Date { get; set; }
        public string? Purc_Date { get; set; }
        public string? Prod_Date { get; set; }
        public string? Deli_Date { get; set; }
        public string? Person { get; set; }
        public string? Remark { get; set; }
        public string? Users { get; set; }
    }

    public partial class SaveOrderAcceptTaskHead
    {
        [NotMapped]
        public List<OrderAcceptTask> OrderAcceptTask { get; set; }

    }

    public class GetOrderApprovelItems
    {
        public int VchCode { get; set; }
        public string? VchDate { get; set; }
        public string? VchNo { get; set; }
        public string? VchSeries { get; set; }
        public string? PoNo { get; set; }
        public string? AccName { get; set; }
        public int TaskCode { get; set; }
        public int TaskId { get; set; }
        public string? TaskDesc { get; set; }
        public string? TaskDate { get; set; }
        public int TaskStatus { get; set; }
        public string? Person { get; set; }
        public string? Remark { get; set; }
        public int ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? Unit { get; set; }
        public string? MName1 { get; set; }
        public string? MName3 { get; set; }
        public string? MName2 { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public double Qty { get; set; }
        public double AltQty { get; set; }
        public double Price { get; set; }
        public double MRP { get; set; }
        public double Amount { get; set; }
    }

    public class SaveOrderTaskApproval
    {
        public int VchCode { get; set; }
        public int TaskCode { get; set; }
        public int TaskId { get; set; } = 0;
        public int Status { get; set; } = 0;
        public string? Remark { get; set; }
        public string? Users { get; set; }
        public string? CreatedOn { get; set; }
    }

    public partial class GetApprovelHoldDet
    {
        public int Action { get; set; }
        public string? Remark { get; set; }
        public string? Date { get; set; }
    }

    //**************************************************** Busy Report ****************************************************//

    public partial class GetOrderStatusRpt
    {
        public int VchCode { get; set; }
        public int TaskCode { get; set; }
        public string? VchDate { get; set; }
        public string? VchNo { get; set; }
        public int Acccode { get; set; }
        public string? AccName { get; set; }
        public int? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public double Qty { get; set; }
        public double AltQty { get; set; }
        public double MRP { get; set; }
        public int Status { get; set; }
        public string? Person
        {
            get; set;
        }
    }

    //**************************************************** Busy Master ****************************************************//

    public partial class GetBusyMasterList
    {
        public int Value { get; set; }
        public string? Label { get; set; }
    }

    //**************************************************** Role Permission And User Right Classes ****************************************************//

    public partial class GetUserRolePermissionMenu
    {
        public int MenuId { get; set; }
        public string? Menu { get; set; }
        public int Create { get; set; }
        public int Edit { get; set;}
        public int? View { get; set; }
        public int? Delete { get; set; }

    }

    public class Permission
    {
        public int MenuId { get; set; }
        public int Create { get; set; }
        public int Edit { get; set; }
        public int View { get; set; }
        public int Delete { get; set; }

    }

    public class SaveRolePermissionResponse
    {
        public int RoleId { get; set; }

        [NotMapped]
        public List<Permission> PermissionData { get; set; }
    }

    public class UserMenus
    {
        public int MenuId { get; set; }
        public string? Label { get; set; }
        public int ParentId { get; set; }
        public string? Link { get; set; }
        public string? Icon { get; set; }
        public bool SubmenuOpen { get; set; }
        public bool ShowSubRoute { get; set; }
        public bool Submenu { get; set; }
        public string? SubmenuHdr { get; set; }
        public int SubMenuNo { get; set; }
        public int PMenuOrd { get; set; }
        public int TranType { get; set; }
        //public int Right1 { get; set; }
        //public int Right2 { get; set; }
        //public int Right3 { get; set; }
        //public int Right4 { get; set; }
        //public int Right5 { get; set; }
        public List<UserMenus> SubmenuItems { get; set; }
    }
}
