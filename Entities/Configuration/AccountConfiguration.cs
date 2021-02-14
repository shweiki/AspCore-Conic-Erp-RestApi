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

                builder.Property(e => e.Id);

                builder.Property(e => e.Code)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                builder.Property(e => e.Description);

                builder.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                builder.Property(e => e.Type)
                    .IsRequired();


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
                },
                new Account
                {
                    Id = 3,
                    Name = "اشتراكات",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "InCome",
                    IsPrime = false
                },
                new Account
                {
                    Id = 4,
                    Name = "مقبوضات",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "InCome",
                    IsPrime = false
                },
                new Account
                {
                    Id = 5,
                    Name = "خزينة 1",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Cash",
                    IsPrime = false
                },
                new Account
                {
                    Id = 6,
                    Name = "زبون نقدي",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Vendor",
                    IsPrime = false
                }
            );
        }
    }
}
