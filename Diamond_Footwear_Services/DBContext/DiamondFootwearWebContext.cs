using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Azure;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Diamond_Footwear_Services.DBContext;

public partial class DiamondFootwearWebContext : DbContext
{
    public DiamondFootwearWebContext(DbContextOptions<DiamondFootwearWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Response> Responses { get; set; }
    public virtual DbSet<ValidateUserDet> ValidateUserDets { get; set; }
    public virtual DbSet<Status> Results { get; set; }
    public virtual DbSet<SaveUserRoleMaster> SaveUserRoleMasters { get; set; }
    public virtual DbSet<GetUserRoleMaster> GetUserRoleMasters { get; set; }
    public virtual DbSet<SaveUsersMastDetail> SaveUserMastDetails { get; set; }
    public virtual DbSet<GetUserMasterDetail> GetUserMasterDetails { get; set; }
    public virtual DbSet<GetOrderReceivedDetail> GetOrderReceivedDetails { get; set; }
    public virtual DbSet<GetOrderReceivedItemDetail> GetOrderReceivedItemDetails { get; set; }
    public virtual DbSet<UpdateOrderReceivedApproval>UpdateOrderReceivedApprovals { get; set; }
    public virtual DbSet<SaveOrderAcceptTaskHead>SaveOrderAcceptTaskHeads { get; set; }

    //public virtual DbSet<UserMaster> UserMasters { get; set; }

    //public virtual DbSet<UsersValidator> UsersValidate { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = 103.194.9.31, 7172; User ID=sa; password = Excellent#9499@; Database = Comp0020_2023ES015009; Trusted_Connection=false; Encrypt=false; TrustServerCertificate=False; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<UserMaster>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserMast__1788CC4CA97830C0");

            entity.ToTable("UserMaster");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FName");
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LName");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(90)
                .IsUnicode(false);
            entity.Property(e => e.Pwd)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("PWD");
            entity.Property(e => e.UserName)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);*/
        modelBuilder.Entity<ValidateUserDet>().HasNoKey();
        modelBuilder.Entity<Response>().HasNoKey();
        modelBuilder.Entity<Status>().HasNoKey();
        modelBuilder.Entity<SaveUserRoleMaster>().HasNoKey();
        modelBuilder.Entity<GetUserRoleMaster>().HasNoKey();
        modelBuilder.Entity<SaveUsersMastDetail>().HasNoKey();
        modelBuilder.Entity<GetUserMasterDetail>().HasNoKey();
        modelBuilder.Entity<GetOrderReceivedDetail>().HasNoKey();
        modelBuilder.Entity<GetOrderReceivedItemDetail>().HasNoKey();
        modelBuilder.Entity<UpdateOrderReceivedApproval>().HasNoKey();
        modelBuilder.Entity<SaveOrderAcceptTaskHead>().HasNoKey();

        //modelBuilder.Entity<UsersValidator>(entity =>
        //{
        //    entity.HasNoKey();
        //});

        //modelBuilder.Entity<UserMaster>().HasNoKey();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


}