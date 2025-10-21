﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniStore.Models;

public partial class MiniStoreContext : DbContext
{
    public MiniStoreContext()
    {
    }

    public MiniStoreContext(DbContextOptions<MiniStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CHITIETHDBAN> CHITIETHDBANs { get; set; }

    public virtual DbSet<CHITIETHDNHAP> CHITIETHDNHAPs { get; set; }

    public virtual DbSet<CONGNO> CONGNOs { get; set; }

    public virtual DbSet<HDBAN> HDBANs { get; set; }

    public virtual DbSet<HDNHAP> HDNHAPs { get; set; }

    public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }

    public virtual DbSet<LOAISANPHAM> LOAISANPHAMs { get; set; }

    public virtual DbSet<NHACUNGCAP> NHACUNGCAPs { get; set; }

    public virtual DbSet<NHANVIEN> NHANVIENs { get; set; }

    public virtual DbSet<PHIEUTHANHTOAN> PHIEUTHANHTOANs { get; set; }

    public virtual DbSet<SANPHAM> SANPHAMs { get; set; }

    public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }

    public virtual DbSet<VAITRO> VAITROs { get; set; }

    public virtual DbSet<VIEW_ThongKeDoanhThu> VIEW_ThongKeDoanhThus { get; set; }

    public virtual DbSet<V_CONGNO_PHAITRA> V_CONGNO_PHAITRAs { get; set; }

    public virtual DbSet<V_NHANVIEN_TAIKHOAN> V_NHANVIEN_TAIKHOANs { get; set; }

    public virtual DbSet<V_SANPHAM_HSD> V_SANPHAM_HSDs { get; set; }

    public virtual DbSet<V_SANPHAM_NHACUNGCAP> V_SANPHAM_NHACUNGCAPs { get; set; }

    public virtual DbSet<V_TONKHO> V_TONKHOs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KHANHDUY\\SQLEXPRESS;Database=QL_SIEUTHIMINI_TIEMTAPHOA;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CHITIETHDBAN>(entity =>
        {
            entity.ToTable("CHITIETHDBAN", tb => tb.HasTrigger("trg_UpdateSoLuongBan"));

            entity.Property(e => e.THANHTIEN).HasComputedColumnSql("([SOLUONG]*[DONGIA])", true);

            entity.HasOne(d => d.MAHDNavigation).WithMany(p => p.CHITIETHDBANs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETHDBAN");

            entity.HasOne(d => d.MASPNavigation).WithMany(p => p.CHITIETHDBANs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETHDBAN_SANPHAM");
        });

        modelBuilder.Entity<CHITIETHDNHAP>(entity =>
        {
            entity.ToTable("CHITIETHDNHAP", tb =>
                {
                    tb.HasTrigger("trg_InsertCongNo_FromNhapHang");
                    tb.HasTrigger("trg_UpdateSoLuongNhap");
                });

            entity.Property(e => e.THANHTIENN).HasComputedColumnSql("([SOLUONGTN]*[DONGIANHAP])", true);

            entity.HasOne(d => d.MAHDNHAPNavigation).WithMany(p => p.CHITIETHDNHAPs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETHDNHAP_HDNHAP");

            entity.HasOne(d => d.MASPNavigation).WithMany(p => p.CHITIETHDNHAPs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETHDNHAP_SANPHAM");
        });

        modelBuilder.Entity<CONGNO>(entity =>
        {
            entity.Property(e => e.CONLAI).HasComputedColumnSql("([SOTIENPHAITRA]-[DATHANHTOAN])", true);
            entity.Property(e => e.NGAYPHATSINH).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TRANGTHAI).HasDefaultValue("Chưa thanh toán");

            entity.HasOne(d => d.MAHD_NHAPNavigation).WithMany(p => p.CONGNOs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CONGNO_HDNHAP");

            entity.HasOne(d => d.MANCCNavigation).WithMany(p => p.CONGNOs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CONGNO_NHACUNGCAP");
        });

        modelBuilder.Entity<HDBAN>(entity =>
        {
            entity.HasOne(d => d.MAKHNavigation).WithMany(p => p.HDBANs).HasConstraintName("FK_HDBAN_KHACHHANG");

            entity.HasOne(d => d.USERNAMENavigation).WithMany(p => p.HDBANs).HasConstraintName("FK_HDBAN_NHANVIEN");
        });

        modelBuilder.Entity<HDNHAP>(entity =>
        {
            entity.HasOne(d => d.MANCCNavigation).WithMany(p => p.HDNHAPs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HDNHAP_NHACC");

            entity.HasOne(d => d.USERNAMENavigation).WithMany(p => p.HDNHAPs).HasConstraintName("FK_HDNHAP_NHANVIEN");
        });

        modelBuilder.Entity<KHACHHANG>(entity =>
        {
            entity.HasOne(d => d.USERNAMENavigation).WithOne(p => p.KHACHHANG).HasConstraintName("FK__KHACHHANG__USERN__628FA481");
        });

        modelBuilder.Entity<NHANVIEN>(entity =>
        {
            entity.HasOne(d => d.USERNAMENavigation).WithOne(p => p.NHANVIEN)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NHANVIEN_TAIKHOAN");
        });

        modelBuilder.Entity<PHIEUTHANHTOAN>(entity =>
        {
            entity.ToTable("PHIEUTHANHTOAN", tb => tb.HasTrigger("trg_UpdateCongNo_AfterPTT"));

            entity.Property(e => e.NGAYTRA).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MACONGNONavigation).WithMany(p => p.PHIEUTHANHTOANs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PHIEUTHANHTOAN_CONGNO");
        });

        modelBuilder.Entity<SANPHAM>(entity =>
        {
            entity.HasKey(e => e.MASP).HasName("PK_MASP");

            entity.HasOne(d => d.MALOAINavigation).WithMany(p => p.SANPHAMs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MASP_LOAISANPHAM");

            entity.HasOne(d => d.MANCCNavigation).WithMany(p => p.SANPHAMs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MAS_NHACUNGCAP");
        });

        modelBuilder.Entity<TAIKHOAN>(entity =>
        {
            entity.HasOne(d => d.MAROLENavigation).WithMany(p => p.TAIKHOANs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TAIKHOAN_VAITRO");
        });

        modelBuilder.Entity<VAITRO>(entity =>
        {
            entity.Property(e => e.MAROLE).ValueGeneratedNever();
        });

        modelBuilder.Entity<VIEW_ThongKeDoanhThu>(entity =>
        {
            entity.ToView("VIEW_ThongKeDoanhThu");
        });

        modelBuilder.Entity<V_CONGNO_PHAITRA>(entity =>
        {
            entity.ToView("V_CONGNO_PHAITRA");
        });

        modelBuilder.Entity<V_NHANVIEN_TAIKHOAN>(entity =>
        {
            entity.ToView("V_NHANVIEN_TAIKHOAN");
        });

        modelBuilder.Entity<V_SANPHAM_HSD>(entity =>
        {
            entity.ToView("V_SANPHAM_HSD");
        });

        modelBuilder.Entity<V_SANPHAM_NHACUNGCAP>(entity =>
        {
            entity.ToView("V_SANPHAM_NHACUNGCAP");
        });

        modelBuilder.Entity<V_TONKHO>(entity =>
        {
            entity.ToView("V_TONKHO");
        });
        modelBuilder.HasSequence<int>("seq_MACONGNO");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
