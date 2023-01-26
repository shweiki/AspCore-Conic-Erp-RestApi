using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<TreeAccount>
{
    public void Configure(EntityTypeBuilder<TreeAccount> builder)
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

    }
}
