using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Diamond_Footwear_Services.DBContext
{
    public class UserDetail
    {
        public int UserId { get; set; }
        public int UserType { get; set; }
        public int Admin { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public int Active { get; set; }
    }

    public partial class SaveUsersMastDetail
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? MobileNo {  get; set; }
        public string? EmailId { get; set; }
        public string? PWD {  get; set; }
        public string? Desc {  get; set; }
        //public int UserType { get; set; }
        public int RoleId { get; set; }
        public string? Base64 {  get; set; }
        public int Deactivate {  get; set; }
    }

    public partial class GetUserMasterDetail
    {
        public int UserId { get; set; }
        public string? Username {  get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public string? PWD { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Desc { get; set; }
        public int Doc1 { get; set; }
        public string? Image { get; set; }
        public int Active { set; get; }
        public string? CreatedOn {  get; set; }
    }

    //public partial class UserAttachDt
    //{
    //    public int SrNo { get; set; }
    //    public string? Name { get; set; }
    //    public string? Base64 { get; set; }
    //    public string? FileName { get; set; }
    //    public string? FileExt { get; set; }
    //    public int FileSize { get; set; }
    //};

    //public partial class SaveUserMast
    //{
    //    public int UserId { get; set; }
    //    public string? FName { get; set; }
    //    public string? LName { get; set; }
    //    public string? Username { get; set; }
    //    public string? Pwd { get; set; }
    //    public string? Email { get; set; }
    //    public string? Mobile { get; set; }
    //    public int Active { get; set; }
    //    public int Role { get; set; }
    //    public int Doc { get; set; }

    //    [NotMapped]
    //    public UserAttachDt? UserImg { get; set; }
    //    public string? SensitiveField { get; set; }
    //}

    //public partial class LoadUserMast
    //{
    //    public int UserId { get; set; }
    //    public string? Name { get; set; }
    //    public string? FName { get; set; }
    //    public string? LName { get; set; }
    //    public string? Username { get; set; }
    //    public string? Pwd { get; set; }
    //    public string? Email { get; set; }
    //    public string? Mobile { get; set; }
    //    public int Active { get; set; }
    //    public int Role { get; set; }
    //    public int Doc { get; set; }
    //    public int SrNo { get; set; }
    //    public string? IName { get; set; }
    //    public string? Image { get; set; }
    //    public string? FileName { get; set; }
    //    public string? FileExt { get; set; }
    //    public double FileSize { get; set; }
    //}

    //public partial class UserMaster
    //{
    //    public int UserId { get; set; }

    //    public string? Name { get; set; }

    //    public string? Fname { get; set; }

    //    public string? Lname { get; set; }

    //    public string? UserName { get; set; }

    //    public string? Pwd { get; set; }

    //    public string? Email { get; set; }

    //    public string? Mobile { get; set; }

    //    public int? Active { get; set; }

    //    public int? UserType { get; set; }

    //    public int? Admin { get; set; }

    //    public int? DocAttach { get; set; }

    //    public DateTime? CreatedOn { get; set; }
    //}

}
