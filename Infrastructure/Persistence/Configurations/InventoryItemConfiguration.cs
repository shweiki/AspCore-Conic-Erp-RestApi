﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {


        builder.ToTable("InventoryItem");

        builder.Property(e => e.Id);

        builder.Property(e => e.Description);

        builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

    }
}
