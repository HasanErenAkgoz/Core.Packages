using Core.Packages.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Packages.Persistence.Configurations
{
    public class TranslateConfiguration : IEntityTypeConfiguration<Domain.Entities.Translation>
    {
        public void Configure(EntityTypeBuilder<Translation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Language).IsRequired();
            builder.Property(x => x.Key).IsRequired();
            builder.Property(x => x.Value).IsRequired();
        }
    }
}
