using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestSystem.Domain.Models;
using QuestSystem.Domain.Models.Quests;

namespace QuestSystem.Infrastructure.Data.Configurations;

public class QuestConfiguration : IEntityTypeConfiguration<Quest>
{
    public void Configure(EntityTypeBuilder<Quest> builder)
    {
        //Placeholder code for database entity creation when needed
        
        // builder.Property(t => t.Title)
        //     .HasMaxLength(200)
        //     .IsRequired();
    }
}
