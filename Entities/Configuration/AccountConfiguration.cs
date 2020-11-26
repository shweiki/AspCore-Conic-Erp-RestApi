using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
   
                builder.ToTable("Account");

                builder.Property(e => e.Id).HasColumnName("ID");

                builder.Property(e => e.Code)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                builder.Property(e => e.Description).HasColumnType("text");

                builder.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                builder.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("text");


            builder.HasData
            (
                new Account
                {
                    Id = 2,
                    Name = "مبيعات",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "InCome",
                    IsPrime = false
                }
            );
        }
    }
}
