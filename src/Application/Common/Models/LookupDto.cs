using AutoMapper;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Domain.Models.Quests;

namespace QuestSystem.Application.Common.Models;

public class LookupDto
{
    public int Id { get; init; }

    public string? Title { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<IObjective, LookupDto>();
            CreateMap<Quest, LookupDto>();
        }
    }
}
