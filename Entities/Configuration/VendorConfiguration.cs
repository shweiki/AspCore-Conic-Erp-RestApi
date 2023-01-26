using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {


        builder.ToTable("Vendor");

        builder.HasIndex(e => e.AccountId, "IX_Vendor_AccountID");

        builder.Property(e => e.Id);

        builder.Property(e => e.AccountId);

        builder.Property(e => e.Region);

        builder.Property(e => e.Description);

        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .IsUnicode(false);

        builder.Property(e => e.Fax)
            .HasMaxLength(13)
            .IsUnicode(false);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(false);

        builder.Property(e => e.PhoneNumber1)
            .HasMaxLength(13)
            .IsUnicode(false);

        builder.Property(e => e.PhoneNumber2)
            .HasMaxLength(13)
            .IsUnicode(false);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(false);

        builder.HasOne(d => d.Account)
            .WithMany(p => p.Vendors)
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Vendor_Account");

    }
}
