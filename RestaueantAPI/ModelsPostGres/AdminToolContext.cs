using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestaueantAPI.ModelsPostGres;

public partial class AdminToolContext : DbContext
{
    public AdminToolContext()
    {
    }

    public AdminToolContext(DbContextOptions<AdminToolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UploadedFile> UploadedFiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UploadedFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uploaded_files_pkey");

            entity.ToTable("uploaded_files");

            entity.HasIndex(e => e.Filename, "uploaded_files_filename_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EntryDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("entry_date");
            entity.Property(e => e.ErrorMsg)
                .HasMaxLength(200)
                .HasColumnName("error_msg");
            entity.Property(e => e.FailedCount).HasColumnName("failed_count");
            entity.Property(e => e.FileStatus)
                .HasMaxLength(20)
                .HasColumnName("file_status");
            entity.Property(e => e.FileType)
                .HasMaxLength(20)
                .HasColumnName("file_type");
            entity.Property(e => e.Filename)
                .HasMaxLength(200)
                .HasColumnName("filename");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.SuccessCount).HasColumnName("success_count");
            entity.Property(e => e.SumAmount).HasColumnName("sum_amount");
            entity.Property(e => e.TotalCount).HasColumnName("total_count");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .HasColumnName("user_name");
        });
        modelBuilder.HasSequence("mapp_book_id_seq");
        modelBuilder.HasSequence("mapp_review_id_seq");
        modelBuilder.HasSequence("mapp_t1_id_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
