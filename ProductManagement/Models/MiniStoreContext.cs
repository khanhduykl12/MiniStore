using System;
using System.Collections.Generic;
using System.Linq;
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

    public virtual DbSet<DONHANG_LOG> DONHANG_LOGs { get; set; }

    public virtual DbSet<HANGTRUNGBAY> HANGTRUNGBAYs { get; set; }

    public virtual DbSet<HDBAN> HDBANs { get; set; }

    public virtual DbSet<HDNHAP> HDNHAPs { get; set; }

    public virtual DbSet<LOAISANPHAM> LOAISANPHAMs { get; set; }

    public virtual DbSet<LogCTHDNhap> LogCTHDNhaps { get; set; }

    public virtual DbSet<NGUOIDUNG> NGUOIDUNGs { get; set; }

    public virtual DbSet<NHACUNGCAP> NHACUNGCAPs { get; set; }

    public virtual DbSet<OTP_LOG> OTP_LOGs { get; set; }

    public virtual DbSet<PHIEUTHANHTOAN> PHIEUTHANHTOANs { get; set; }

    public virtual DbSet<SANPHAM> SANPHAMs { get; set; }

    public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }

    public virtual DbSet<VAITRO> VAITROs { get; set; }

    public virtual DbSet<VIEW_ThongKeDoanhThu> VIEW_ThongKeDoanhThus { get; set; }

    public virtual DbSet<V_CONGNO_PHAITRA> V_CONGNO_PHAITRAs { get; set; }

    public virtual DbSet<V_SANPHAM_HSD> V_SANPHAM_HSDs { get; set; }

    public virtual DbSet<V_SANPHAM_NHACUNGCAP> V_SANPHAM_NHACUNGCAPs { get; set; }

    public virtual DbSet<V_TONKHO> V_TONKHOs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=laphuthu\\SQL2022DEV;Database=QL_MiniShop_Thuan;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CHITIETHDBAN>(entity =>
        {
            entity.ToTable("CHITIETHDBAN", tb =>
                {
                    tb.HasTrigger("TRG_DeleteEmptyInvoice");
                    tb.HasTrigger("trg_Update_SoLuongBan");
                });

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
            entity.ToTable("CHITIETHDNHAP", tb => tb.HasTrigger("trg_Update_SoLuongNhap"));

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

        modelBuilder.Entity<DONHANG_LOG>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__DONHANG___3214EC27DFC9F4EE");

            entity.Property(e => e.CREATED_AT).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TRANGTHAI).HasDefaultValue("Cho_xu_ly");
        });

        modelBuilder.Entity<HANGTRUNGBAY>(entity =>
        {
            entity.HasKey(e => e.MASP).HasName("PK__HANGTRUN__60228A32F76184A6");

            entity.ToTable("HANGTRUNGBAY", tb => tb.HasTrigger("trg_Update_Kho_From_Ke"));

            entity.Property(e => e.TRANGTHAI).HasDefaultValue("Đang bán");

            entity.HasOne(d => d.MASPNavigation).WithOne(p => p.HANGTRUNGBAY)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HANGTRUNGB__MASP__00200768");
        });

        modelBuilder.Entity<HDBAN>(entity =>
        {
            entity.HasKey(e => e.MAHD).HasName("PK__HDBAN__603F20CED18950D4");

            entity.ToTable("HDBAN", tb => tb.HasTrigger("TRG_DeleteEmptyInvoice_AfterInsert"));

            entity.HasOne(d => d.NGUOILAP).WithMany(p => p.HDBANNGUOILAPs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HDBAN__NGUOILAP___5AEE82B9");

            entity.HasOne(d => d.NGUOIMUA).WithMany(p => p.HDBANNGUOIMUAs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HDBAN__NGUOIMUA___5BE2A6F2");
        });

        modelBuilder.Entity<HDNHAP>(entity =>
        {
            entity.HasOne(d => d.MANCCNavigation).WithMany(p => p.HDNHAPs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HDNHAP_NHACC");

            entity.HasOne(d => d.USERNAMENavigation).WithMany(p => p.HDNHAPs).HasConstraintName("FK_HDNHAP_TAIKHOAN");
        });

        modelBuilder.Entity<LogCTHDNhap>(entity =>
        {
            entity.HasKey(e => e.LogID).HasName("PK__LogCTHDN__5E5499A8B74185F3");

            entity.Property(e => e.LoggedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.THANHTIENN).HasComputedColumnSql("([SOLUONGTN]*[DONGIANHAP])", true);
        });

        modelBuilder.Entity<NGUOIDUNG>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__NGUOIDUN__3214EC2746617FBF");

            entity.HasOne(d => d.MAROLENavigation).WithMany(p => p.NGUOIDUNGs).HasConstraintName("FK_NGUOIDUNG_VAITRO");

            entity.HasOne(d => d.USERNAMENavigation).WithOne(p => p.NGUOIDUNG).HasConstraintName("FK_NHANVIEN_TAIKHOAN");
        });

        modelBuilder.Entity<OTP_LOG>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__OTP_LOG__3214EC2781E5D052");

            entity.Property(e => e.CREATE_AT).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.USERNAMENavigation).WithMany(p => p.OTP_LOGs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OTP_LOG_TAIKHOAN");
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

            entity.ToTable("SANPHAM", tb => tb.HasTrigger("trg_DeleteSanPham_KeepKe"));

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

        modelBuilder.Entity<VIEW_ThongKeDoanhThu>(entity =>
        {
            entity.ToView("VIEW_ThongKeDoanhThu");
        });

        modelBuilder.Entity<V_CONGNO_PHAITRA>(entity =>
        {
            entity.ToView("V_CONGNO_PHAITRA");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    /// <summary>
    /// Xóa các hóa đơn bán không có chi tiết.
    /// </summary>
    /// <returns>Số lượng hóa đơn đã xóa.</returns>
    public int DeleteEmptyInvoices()
    {
        var emptyInvoices = HDBANs
            .Where(h => !CHITIETHDBANs.Any(ct => ct.MAHD == h.MAHD))
            .ToList();

        if (emptyInvoices.Count > 0)
        {
            HDBANs.RemoveRange(emptyInvoices);
            SaveChanges();
        }

        return emptyInvoices.Count;
    }
}
