using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Configuration
{
    public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {


            builder.ToTable("InventoryItem");

            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.Description).HasColumnType("nvarchar(max)");

            builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

          

            builder.HasData
            (
                new InventoryItem
                {
                    Id = 1,
                    Name = "المخزن 1",
                    Description = "",
                    Status = 0,
                    IsPrime = true,
                }
            );
        }
    }
}
