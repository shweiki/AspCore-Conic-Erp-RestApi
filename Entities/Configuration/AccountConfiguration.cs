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
                    Id = 1,
                    Name = "موجودات",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Main",
                    IsPrime = false,
                    ParentId = 0

                },
                new Account
                {
                    Id = 2,
                    Name = "مطلوبات",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Main",
                    IsPrime = false,
                    ParentId = 0

                },
                new Account
                {
                    Id = 3,
                    Name = "حقوق ملكية",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Main",
                    IsPrime = false,
                    ParentId = 0

                },
                new Account
                {
                    Id = 4,
                    Name = "حسابات التشغيلية",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Main",
                    IsPrime = false,
                    ParentId = 0

                },
                new Account
                {
                    Id = 5,
                    Name = "موجودات ذابتة",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Main",
                    IsPrime = false,
                    ParentId = 1

                },
                new Account
                {
                    Id = 6,
                    Name = "موجودات متداولة",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Main",
                    IsPrime = false,
                    ParentId = 1

                },  
                new Account
                {
                    Id = 7,
                    Name = "نقد",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Main",
                    IsPrime = false,
                    ParentId = 6

                },
                new Account
                {
                    Id = 8,
                    Name = "صندوق 1",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Cash",
                    IsPrime = false,
                    ParentId = 7

                },
                new Account
                {
                    Id = 9,
                    Name = "بنوك",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Main",
                    IsPrime = false,
                    ParentId = 6

                }, 
                new Account
                {
                    Id = 10,
                    Name = "ذمم مدينة",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Main",
                    IsPrime = false,
                    ParentId = 6

                },
                new Account
                {
                    Id = 11,
                    Name = "شيكات برسم التحصيل",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Main",
                    IsPrime = false,
                    ParentId = 6

                },
                new Account
                {
                    Id = 12,
                    Name = "مبيعات",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "InCome",
                    IsPrime = false,
                    ParentId = 0

                },
                new Account
                {
                    Id = 13,
                    Name = "اشتراكات",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "InCome",
                    IsPrime = false,
                    ParentId = 0

                },
                new Account
                {
                    Id = 14,
                    Name = "مقبوضات",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "InCome",
                    IsPrime = false,
                    ParentId = 0

                },

                new Account
                {
                    Id = 15,
                    Name = "زبون نقدي",
                    Description = "",
                    Status = 0,
                    Code = "",
                    Type = "Vendor",
                    IsPrime = false,
                    ParentId = 0
                }
            );
        }
    }
}
