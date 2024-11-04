﻿using System.Threading.Tasks;
using QuestSystem.Application.Common.Models.Provider;

namespace QuestSystem.Application.Common.Interfaces.Providers;

public interface IMetricProvider
{ 
    Task<MetricData> GetMetricFromUser(string requestEmail, string metricName);
}