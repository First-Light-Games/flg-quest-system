using System.Reflection;
using QuestSystem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using QuestSystem.Domain.Models;

namespace QuestSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
