using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NightClubTestCase.Models;

namespace NightClubTestCase.DBContext
{
    public partial class NightClubContext : DbContext
    {
        public NightClubContext()
        {
        }

        public NightClubContext(DbContextOptions<NightClubContext> options)
            : base(options)
        {
        }

        public virtual DbSet<IdentityCard> IdentityCards { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<MemberCard> MemberCards { get; set; } = null!;
        public virtual DbSet<Record> Records { get; set; } = null!;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=NightClub;Trusted_Connection=True;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityCard>(entity =>
            {
                entity.HasKey(e => e.IdentityCardId);

                entity.ToTable("IdentityCard");

                entity.Property(e => e.IdentityCardId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("identityCardId");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasColumnName("birthDate");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("expirationDate");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(25)
                    .HasColumnName("firstName");

                entity.Property(e => e.HasExpired).HasColumnName("hasExpired");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("lastName");

                entity.Property(e => e.NationalRegisterNumber)
                    .HasMaxLength(15)
                    .HasColumnName("nationalRegisterNumber");

                entity.Property(e => e.ValidityDate)
                    .HasColumnType("datetime")
                    .HasColumnName("validityDate");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.HasIndex(e => e.IdentityCardId, "IX_Member_identityCardId");

                entity.Property(e => e.MemberId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("memberId");

                entity.Property(e => e.IdentityCardId).HasColumnName("identityCardId");

                entity.Property(e => e.MailAddress)
                    .HasMaxLength(100)
                    .HasColumnName("mailAddress");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(25)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.BlacklistEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("blacklistEndDate");

                entity.HasOne(d => d.IdentityCardNavigation)
                    .WithOne(p => p.Member)
                    .HasForeignKey<Member>(d => d.IdentityCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Member_IdentityCardId");
            });

            modelBuilder.Entity<MemberCard>(entity =>
            {
                entity.HasKey(e => e.MemberCardId);

                entity.ToTable("MemberCard");

                entity.Property(e => e.MemberCardId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("memberCardId");

                entity.Property(e => e.IsLost).HasColumnName("isLost");
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Record");

                entity.HasIndex(e => e.MemberCardId, "IX_Record_memberCardId");

                entity.HasIndex(e => e.MemberId, "IX_Record_memberId");

                entity.Property(e => e.MemberCardId).HasColumnName("memberCardId");

                entity.Property(e => e.MemberId).HasColumnName("memberId");

                entity.HasOne(d => d.MemberCardNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MemberCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_MemberCardId");

                entity.HasOne(d => d.Member)
                    .WithMany()
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Record_Member");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
