using System;
using System.Collections.Generic;
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

    public class Status
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
        public int ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? Unit { get; set; }
        public string? AltUnit { get; set; }
        public string? Para1 { get; set; }
        public string? Para2 { get; set; }
        public double Qty { get; set; }
        public double AltQty { get; set; }
        public int ClQty { get; set; }
        public double Price { get; set; }
        public double MRP { get; set; }
        public double Amount { get; set; }
        public int Status {  get; set; }
    }

    public class UpdateOrderReceivedApproval
    {
        public int VchCode { get; set; }
        public string? Remarks { get; set; }
    }

}
