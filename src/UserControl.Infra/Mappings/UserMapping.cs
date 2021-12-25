using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserControl.Domain.Models;

namespace UserControl.Infra.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Username)
                .IsRequired()
                .HasColumnType("VARCHAR(50)");

            builder.Property(p => p.Password)
                .IsRequired()
                .HasColumnType("VARCHAR(50)");

            builder.Property(p => p.Role)
                .IsRequired()
                .HasColumnType("VARCHAR(50)");
            
            builder.Ignore(p => p.validationResult);
        }
    }
}