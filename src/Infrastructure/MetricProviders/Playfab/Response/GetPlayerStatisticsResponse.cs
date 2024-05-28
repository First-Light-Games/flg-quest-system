using System.Collections.Generic;

namespace QuestSystem.Infrastructure.MetricProviders.Playfab.Response;

public class GetPlayerStatisticsResponse
{
    public StatisticData Data { get; set; } = null!;
}

public class StatisticData
{
    public List<Statistic> Statistics { get; set; } = new();
}

public class Statistic
{
    public string? StatisticName { get; set; } = null!;
    public int Value { get; set; }
}
