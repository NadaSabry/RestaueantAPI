using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestaueantAPI.UATModels;

public partial class MomkenContext : DbContext
{
    public MomkenContext()
    {
    }

    public MomkenContext(DbContextOptions<MomkenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){ }
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CS_AI");

        modelBuilder.Entity<Restaurant>(entity =>
        {
			//entity.HasNoKey();
			entity.HasKey(e => e.Id);

			entity.HasIndex(e => e.RestaurantCode, "IX_Restaurants").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.RestaurantCode).HasMaxLength(50);
            entity.Property(e => e.RestaurantName).HasMaxLength(100);
            entity.Property(e => e.RestaurantNameAr).HasMaxLength(100);
        });
        modelBuilder.HasSequence("Request_seq")
            .HasMin(1L)
            .HasMax(999999999999999999L);
        modelBuilder.HasSequence("REQUEST_SEQ")
            .StartsAt(100L)
            .HasMin(100L)
            .HasMax(999999999999L);
        modelBuilder.HasSequence("Transaction_Seq")
            .HasMin(1L)
            .HasMax(999999999999999999L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
