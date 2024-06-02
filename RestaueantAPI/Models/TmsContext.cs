using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestaueantAPI.Models;

public partial class TmsContext : DbContext
{
    public TmsContext()
    {
    }

    public TmsContext(DbContextOptions<TmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
       // => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CI_AI");

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.Id);   
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.RestaurantCode).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.RestaurantName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.RestaurantNameAr).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });
        modelBuilder.HasSequence<int>("Request_seq").StartsAt(1000L);
        modelBuilder.HasSequence<int>("Transaction_Seq").StartsAt(1000L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
