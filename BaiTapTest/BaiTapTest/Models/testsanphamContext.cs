using Microsoft.EntityFrameworkCore;

namespace BaiTapTest.Models
{
    public partial class testsanphamContext : DbContext
    {
        public testsanphamContext()
        {
        }

        public testsanphamContext(DbContextOptions<testsanphamContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; } = null!;
        public virtual DbSet<SanPham> SanPhams { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
            var strConn = config["ConnectionStrings:MyDatabase"];
            optionsBuilder.UseSqlServer(strConn);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoaiSanPham>(entity =>
            {
                entity.ToTable("LoaiSanPham");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NgayNhap)
                    .HasColumnType("date")
                    .HasColumnName("ngay_nhap");

                entity.Property(e => e.Ten)
                    .HasMaxLength(255)
                    .HasColumnName("ten");
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.ToTable("SanPham");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Gia)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("gia");

                entity.Property(e => e.IdLoaiSanPham).HasColumnName("id_loai_san_pham");

                entity.Property(e => e.NgayNhap)
                    .HasColumnType("date")
                    .HasColumnName("ngay_nhap");

                entity.Property(e => e.Tensanpham)
                    .HasMaxLength(255)
                    .HasColumnName("tensanpham");

                entity.HasOne(d => d.IdLoaiSanPhamNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.IdLoaiSanPham)
                    .HasConstraintName("FK__SanPham__id_loai__398D8EEE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
