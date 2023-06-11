using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Main_Branch_Locator_App.Models;

public partial class MainBranchLocatorDbContext : DbContext
{
    public MainBranchLocatorDbContext()
    {
    }

    public MainBranchLocatorDbContext(DbContextOptions<MainBranchLocatorDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BranchTable> BranchTable { get; set; }

    public virtual DbSet<ReportTable> ReportTable { get; set; }

    public virtual DbSet<ServicesTable> ServicesTable { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=MAIN_BRANCH_LOCATOR_DB;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BranchTable>(entity =>
        {
            entity.HasKey(e => e.BranchCode);

            entity.ToTable("Branch_Table");

            entity.Property(e => e.BranchCode)
                .ValueGeneratedNever()
                .HasColumnName("Branch_Code");
            entity.Property(e => e.AccountOpening)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Available')");
            entity.Property(e => e.AtmCashWithdrawal)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Available')")
                .HasColumnName("ATM_CASH_WITHDRAWAL");
            entity.Property(e => e.AtmCollection)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Available')")
                .HasColumnName("ATM_COLLECTION");
            entity.Property(e => e.BranchAddress).HasColumnName("Branch_Address");
            entity.Property(e => e.BranchAtmOperatorEmail).HasColumnName("Branch_Atm_Operator_Email");
            entity.Property(e => e.BranchAtmOperatorName).HasColumnName("Branch_Atm_Operator_Name");
            entity.Property(e => e.BranchAtmOperatorTel).HasColumnName("Branch_Atm_Operator_Tel");
            entity.Property(e => e.BranchCity).HasColumnName("Branch_City");
            entity.Property(e => e.BranchGpsXCoordinate).HasColumnName("Branch_GPS_X_Coordinate");
            entity.Property(e => e.BranchGpsYCoordinate).HasColumnName("Branch_GPS_Y_Coordinate");
            entity.Property(e => e.BranchManagerEmail).HasColumnName("Branch_Manager_Email");
            entity.Property(e => e.BranchManagerName).HasColumnName("Branch_Manager_Name");
            entity.Property(e => e.BranchManagerTel).HasColumnName("Branch_Manager_Tel");
            entity.Property(e => e.BranchName).HasColumnName("Branch_Name");
            entity.Property(e => e.BranchPassword).HasColumnName("Branch_Password");
            entity.Property(e => e.CashDeposit)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Available')")
                .HasColumnName("CASH_DEPOSIT");
            entity.Property(e => e.FormAtrnx)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Available')")
                .HasColumnName("FormATrnx");
            entity.Property(e => e.FundsTransfer)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Available')")
                .HasColumnName("FUNDS_TRANSFER");
            entity.Property(e => e.FxTransfer)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Available')")
                .HasColumnName("FX_TRANSFER");
            entity.Property(e => e.FxTrnx)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Available')")
                .HasColumnName("FX_TRNX");
            entity.Property(e => e.PayDirect)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Available')")
                .HasColumnName("PAY_DIRECT");
            entity.Property(e => e.RemittaTrnx)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Available')")
                .HasColumnName("REMITTA_TRNX");
            entity.Property(e => e.TokenCollection)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Available')")
                .HasColumnName("TOKEN_COLLECTION");
        });

        modelBuilder.Entity<ReportTable>(entity =>
        {
            entity.HasKey(e => e.ReportId);

            entity.ToTable("Report_Table");

            entity.Property(e => e.ReportId)
                .ValueGeneratedNever()
                .HasColumnName("Report_Id");
            entity.Property(e => e.AdminRead)
                .HasMaxLength(6)
                .HasColumnName("Admin_Read");
            entity.Property(e => e.FkBranchCode).HasColumnName("FK_Branch_Code");
            entity.Property(e => e.ReportDateTime).HasColumnName("Report_Date_Time");
            entity.Property(e => e.ReportDescription).HasColumnName("Report_Description");
            entity.Property(e => e.ReportSubject)
                .HasMaxLength(50)
                .HasColumnName("Report_Subject");
        });

        modelBuilder.Entity<ServicesTable>(entity =>
        {
            entity.HasKey(e => e.ServiceId);

            entity.ToTable("Services_Table");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("Service_Id");
            entity.Property(e => e.AvailableOnline)
                .HasMaxLength(50)
                .HasColumnName("AVAILABLE_ONLINE");
            entity.Property(e => e.LinkToService).HasColumnName("LINK_TO_SERVICE");
            entity.Property(e => e.ServiceDescription).HasColumnName("Service_Description");
            entity.Property(e => e.ServiceRequiredDocuments).HasColumnName("Service_Required_Documents");
            entity.Property(e => e.ServiceSubject)
                .HasMaxLength(50)
                .HasColumnName("Service_Subject");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
