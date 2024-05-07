using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public int Status {  get; set; }
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
        public string? VchDate{ get; set; }
        public string? VchNo { get; set; }
        public int vchSeries { get; set; }
        public string? PoNo {  get; set; }
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
        public int Status {  get; set; }
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
    }

    public partial class GetApprovelHoldDet
    {
        public int Action { get; set; }
        public string? Remark { get; set; }
    }
}
