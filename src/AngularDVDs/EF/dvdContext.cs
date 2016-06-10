using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AngularDVDs.EF
{
    public partial class dvdContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ////#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            // optionsBuilder.UseSqlServer(@"Data Source=.\SQL2014;Initial Catalog=DVDCollection;Integrated Security=False;User ID=dvduser;Password=dvdpassword;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public dvdContext(DbContextOptions<dvdContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_IdentityRoleClaim<string>_IdentityRole_RoleId");
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_IdentityUserClaim<string>_ApplicationUser_UserId");
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_IdentityUserLogin<string>");

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.ProviderKey).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_IdentityUserLogin<string>_ApplicationUser_UserId");
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_IdentityUserRole<string>");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_IdentityUserRole<string>_IdentityRole_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_IdentityUserRole<string>_ApplicationUser_UserId");
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<DIRECTOR>(entity =>
            {
                entity.HasKey(e => e.DIRECTOR_ID)
                    .HasName("PK_DIRECTOR");

                entity.Property(e => e.DIRECTOR_ID)
                    .HasDefaultValueSql("newid()")
                    .ValueGeneratedNever();

                entity.Property(e => e.DIRECTOR_ADDMOD_Datetime).HasColumnType("datetime");

                entity.Property(e => e.DIRECTOR_NAME)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<DVD>(entity =>
            {
                entity.HasKey(e => e.DVD_ID)
                    .HasName("PK_DVD");

                entity.Property(e => e.DVD_ID)
                    .HasDefaultValueSql("newid()")
                    .ValueGeneratedNever();

                entity.Property(e => e.DVD_ADDMOD_Datetime).HasColumnType("datetime");

                entity.Property(e => e.DVD_TITLE)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.DVD_DIRECTOR)
                    .WithMany(p => p.DVD)
                    .HasForeignKey(d => d.DVD_DIRECTOR_ID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_DVD_DIRECTOR");

                entity.HasOne(d => d.DVD_GENRE)
                    .WithMany(p => p.DVD)
                    .HasForeignKey(d => d.DVD_GENRE_ID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_DVD_GENRE");
            });

            modelBuilder.Entity<GENRE>(entity =>
            {
                entity.HasKey(e => e.GENRE_ID)
                    .HasName("PK_GENRE");

                entity.Property(e => e.GENRE_ID)
                    .HasDefaultValueSql("newid()")
                    .ValueGeneratedNever();

                entity.Property(e => e.GENRE_ADDMOD_Datetime).HasColumnType("datetime");

                entity.Property(e => e.GENRE_DESC)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.GENRE_NAME)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<DIRECTOR> DIRECTOR { get; set; }
        public virtual DbSet<DVD> DVD { get; set; }
        public virtual DbSet<GENRE> GENRE { get; set; }
    }
}