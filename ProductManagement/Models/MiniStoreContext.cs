using System;
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

    public virtual DbSet<CHAMCONG> CHAMCONGs { get; set; }

    public virtual DbSet<CHITIETHDBAN> CHITIETHDBANs { get; set; }

    public virtual DbSet<CHITIETHDNHAP> CHITIETHDNHAPs { get; set; }

    public virtual DbSet<CONGNO> CONGNOs { get; set; }

    public virtual DbSet<HANGTRUNGBAY> HANGTRUNGBAYs { get; set; }

    public virtual DbSet<HDBAN> HDBANs { get; set; }

    public virtual DbSet<HDNHAP> HDNHAPs { get; set; }

    public virtual DbSet<LICHLAM> LICHLAMs { get; set; }

    public virtual DbSet<LOAISANPHAM> LOAISANPHAMs { get; set; }

    public virtual DbSet<LogCTHDNhap> LogCTHDNhaps { get; set; }

    public virtual DbSet<LogChiTietHDBan> LogChiTietHDBans { get; set; }

    public virtual DbSet<NGUOIDUNG> NGUOIDUNGs { get; set; }

    public virtual DbSet<NHACUNGCAP> NHACUNGCAPs { get; set; }

    public virtual DbSet<OTP_LOG> OTP_LOGs { get; set; }

    public virtual DbSet<PHIEUTHANHTOAN> PHIEUTHANHTOANs { get; set; }

    public virtual DbSet<SANPHAM> SANPHAMs { get; set; }

    public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }

    public virtual DbSet<VAITRO> VAITROs { get; set; }

    public virtual DbSet<VIEW_ThongKeDoanhThu> VIEW_ThongKeDoanhThus { get; set; }

    public virtual DbSet<V_CONGNO_PHAITRA> V_CONGNO_PHAITRAs { get; set; }

    public virtual DbSet<V_HOADON_CHITIET> V_HOADON_CHITIETs { get; set; }

    public virtual DbSet<V_SANPHAM_HSD> V_SANPHAM_HSDs { get; set; }

    public virtual DbSet<V_SANPHAM_NHACUNGCAP> V_SANPHAM_NHACUNGCAPs { get; set; }

    public virtual DbSet<V_TONKHO> V_TONKHOs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KHANHDUY\\SQLEXPRESS;Database=QL_SIEUTHIMINI_TIEMTAPHOA;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CHAMCONG>(entity =>
        {
            entity.HasKey(e => e.MACHAM).HasName("PK__CHAMCONG__F0D58BBCD9CFC9DE");

            entity.HasOne(d => d.LICHLAM).WithMany(p => p.CHAMCONGs).HasConstraintName("FK__CHAMCONG__LICHLA__06CD04F7");

            entity.HasOne(d => d.MANVNavigation).WithMany(p => p.CHAMCONGs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHAMCONG__MANV__05D8E0BE");
        });

        modelBuilder.Entity<CHITIETHDBAN>(entity =>
        {
            entity.ToTable("CHITIETHDBAN", tb =>
                {
                    tb.HasTrigger("TRG_DeleteEmptyInvoice");
                    tb.HasTrigger("trg_CHITIETHDBAN_Insert");
                    tb.HasTrigger("trg_Update_SoLuongBan");
                });

            entity.Property(e => e.THANHTIEN).HasComputedColumnSql("([SOLUONG]*[DONGIA])", true);

            entity.HasOne(d => d.MAHDNavigation).WithMany(p => p.CHITIETHDBANs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETHDBAN_HDBAN");

            entity.HasOne(d => d.MASPNavigation).WithMany(p => p.CHITIETHDBANs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETHDBAN_SANPHAM");
        });

        modelBuilder.Entity<CHITIETHDNHAP>(entity =>
        {
            entity.ToTable("CHITIETHDNHAP", tb =>
                {
                    tb.HasTrigger("trg_LogNewCTHDNhap");
                    tb.HasTrigger("trg_TaoCongNoTuChiTietHDNhap");
                    tb.HasTrigger("trg_Update_SoLuongNhap");
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
            entity.HasKey(e => e.MACONGNO).HasName("PK__CONGNO__6B97E3BA6C33F8B6");

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

        modelBuilder.Entity<HANGTRUNGBAY>(entity =>
        {
            entity.HasKey(e => e.MASP).HasName("PK__HANGTRUN__60228A32C831C354");

            entity.HasOne(d => d.MASPNavigation).WithOne(p => p.HANGTRUNGBAY)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HANGTRUNGB__MASP__7F2BE32F");
        });

        modelBuilder.Entity<HDBAN>(entity =>
        {
            entity.HasKey(e => e.MAHD).HasName("PK__HDBAN__603F20CEC963A313");

            entity.ToTable("HDBAN", tb => tb.HasTrigger("trg_CheckRole_HDBAN"));

            entity.HasOne(d => d.NGUOILAP).WithMany(p => p.HDBANNGUOILAPs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HDBAN__NGUOILAP___59FA5E80");

            entity.HasOne(d => d.NGUOIMUA).WithMany(p => p.HDBANNGUOIMUAs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HDBAN__NGUOIMUA___5AEE82B9");
        });

        modelBuilder.Entity<HDNHAP>(entity =>
        {
            entity.HasKey(e => e.MAHDNHAP).HasName("PK__HDNHAP__B020D33958A024B4");

            entity.HasOne(d => d.MANCCNavigation).WithMany(p => p.HDNHAPs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HDNHAP_NHACC");

            entity.HasOne(d => d.USERNAMENavigation).WithMany(p => p.HDNHAPs)
                .HasPrincipalKey(p => p.USERNAME)
                .HasForeignKey(d => d.USERNAME)
                .HasConstraintName("FK_HDNHAP_NGUOIDUNG");
        });

        modelBuilder.Entity<LICHLAM>(entity =>
        {
            entity.HasKey(e => e.MALICH).HasName("PK__LICHLAM__35F24F0ED524EE63");

            entity.ToTable("LICHLAM", tb =>
                {
                    tb.HasTrigger("TRG_LICHLAM_KHONG_THEM_QUA_KHU");
                    tb.HasTrigger("TRG_LICHLAM_KHONG_UPDATE_QUA_KHU");
                });

            entity.HasOne(d => d.NGUOIDUNG).WithMany(p => p.LICHLAMs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LICHLAM_NGUOIDUNG");
        });

        modelBuilder.Entity<LOAISANPHAM>(entity =>
        {
            entity.HasKey(e => e.MALOAI).HasName("PK__LOAISANP__2F633F2375C3EB40");
        });

        modelBuilder.Entity<LogCTHDNhap>(entity =>
        {
            entity.HasKey(e => e.LogID).HasName("PK__LogCTHDN__5E5499A827CD0264");

            entity.Property(e => e.LoggedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<LogChiTietHDBan>(entity =>
        {
            entity.HasKey(e => e.LogID).HasName("PK__LogChiTi__5E5499A81B96D310");

            entity.Property(e => e.LogDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<NGUOIDUNG>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__NGUOIDUN__3214EC278CCB931F");

            entity.HasOne(d => d.USERNAMENavigation).WithOne(p => p.NGUOIDUNG)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NGUOIDUNG_TAIKHOAN");
        });

        modelBuilder.Entity<NHACUNGCAP>(entity =>
        {
            entity.HasKey(e => e.MANCC).HasName("PK__NHACUNGC__7ABEA582B19EF3DB");
        });

        modelBuilder.Entity<OTP_LOG>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__OTP_LOG__3214EC2788B62EAD");

            entity.Property(e => e.CREATE_AT).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.USERNAMENavigation).WithMany(p => p.OTP_LOGs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OTP_LOG_TAIKHOAN");
        });

        modelBuilder.Entity<PHIEUTHANHTOAN>(entity =>
        {
            entity.HasKey(e => e.MAPTT).HasName("PK__PHIEUTHA__7B35DABDE439AE20");

            entity.ToTable("PHIEUTHANHTOAN", tb => tb.HasTrigger("trg_UpdateCongNo_AfterPTT"));

            entity.Property(e => e.NGAYTRA).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MACONGNONavigation).WithMany(p => p.PHIEUTHANHTOANs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PHIEUTHANHTOAN_CONGNO");
        });

        modelBuilder.Entity<SANPHAM>(entity =>
        {
            entity.HasKey(e => e.MASP).HasName("PK__SANPHAM__60228A321B1FE797");

            entity.HasOne(d => d.MALOAINavigation).WithMany(p => p.SANPHAMs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MASP_LOAISANPHAM");

            entity.HasOne(d => d.MANCCNavigation).WithMany(p => p.SANPHAMs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MAS_NHACUNGCAP");
        });

        modelBuilder.Entity<TAIKHOAN>(entity =>
        {
            entity.HasKey(e => e.USERNAME).HasName("PK__TAIKHOAN__B15BE12FBC47316E");

            entity.HasOne(d => d.MAROLENavigation).WithMany(p => p.TAIKHOANs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TAIKHOAN_VAITRO");
        });

        modelBuilder.Entity<VAITRO>(entity =>
        {
            entity.HasKey(e => e.MAROLE).HasName("PK__VAITRO__4641D60169C2204B");
        });

        modelBuilder.Entity<VIEW_ThongKeDoanhThu>(entity =>
        {
            entity.ToView("VIEW_ThongKeDoanhThu");
        });

        modelBuilder.Entity<V_CONGNO_PHAITRA>(entity =>
        {
            entity.ToView("V_CONGNO_PHAITRA");
        });

        modelBuilder.Entity<V_HOADON_CHITIET>(entity =>
        {
            entity.ToView("V_HOADON_CHITIET");
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
}
