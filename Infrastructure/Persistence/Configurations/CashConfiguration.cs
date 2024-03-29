﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class CashConfiguration : IEntityTypeConfiguration<Cash>
{
    public void Configure(EntityTypeBuilder<Cash> builder)
    {

        builder.ToTable("Cash");


        builder.HasIndex(e => e.AccountId, "IX_Cash_AccountID");

        builder.Property(e => e.Id);

        builder.Property(e => e.AccountId);

        builder.Property(e => e.Btcash);

        builder.Property(e => e.Description);

        builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

        builder.Property(e => e.Pcip);

        builder.HasOne(d => d.Account)
                .WithMany(p => p.Cashes)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Cash_Account");

    }
}
