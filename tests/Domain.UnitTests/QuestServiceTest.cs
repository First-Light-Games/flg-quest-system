using System;
using NUnit.Framework;
using QuestSystem.Domain.Exceptions;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Domain.Models.Objectives;
using QuestSystem.Domain.Models.Quests;
using QuestSystem.Domain.Services;
using QuestSystem.Domain.ValueObjects;

namespace QuestSystem.Domain.UnitTests;

[TestFixture]
public class QuestServiceTests
{
    private QuestService _questService = null!;
    
    private string _eventMetric = null!;
    private IObjective _eventObjective = null!;
    private Quest _eventQuest = null!;
    
    private string _reachAmountMetric = null!;
    private IObjective _reachAmountObjective = null!;
    private Quest _reachAmountQuest = null!;
    
    private string _predictableReachAmountMetric = null!;
    private IObjective _predictableReachAmountObjective = null!;
    private Quest _predictableReachAmountQuest = null!;


    
    [SetUp]
    public void SetUp()
    {
        _questService = new QuestService();
        
        _eventMetric = "Use_SpecialItem";
        _eventObjective = new EventObjective("Use MOAB special item", _eventMetric, "MOAB");
        _eventQuest = new Quest("Gotta Blast 'Em all!", "Use the MOAB", _eventObjective);
        
        _reachAmountMetric = "BP_Level";
        _reachAmountObjective = new AmountObjective("Reach Blast Pass Level 10", _reachAmountMetric, 10);
        _reachAmountQuest = new Quest("Not so Noob!", "Play matches to reach Blast Pass level 10.", _reachAmountObjective);
        
        _predictableReachAmountMetric = "KILL_Players";
        _predictableReachAmountObjective = new AmountPredictableObjective("Kill 50 Players", _predictableReachAmountMetric, 50);
        _predictableReachAmountQuest = new Quest("God save their souls!", "You know the drill! Kill 50 players!", _predictableReachAmountObjective);
    }

    
    [Test]
    public void ConfigureActiveQuest_ValidQuest_AddsQuest()
    {
        _questService.ConfigureQuest(_reachAmountQuest);
        
        var result = _questService.ListActiveQuests();
        Assert.Contains(_reachAmountQuest, result);
    }

    
    [Test]
    public void ConfigureActiveQuest_QuestAlreadyActive_ThrowsException()
    {
        _questService.ConfigureQuest(_reachAmountQuest);
        
        var ex = Assert.Throws<QuestAlreadyActiveForMetricException>(() => _questService.ConfigureQuest(_reachAmountQuest));
        Assert.That(ex?.Message, Is.EqualTo($"Quest already active for metric: {_reachAmountMetric}"));
    }

    
    [Test]
    public void CheckQuestCompletion_AmountQuest_ValidTypeAndValue_ReturnsTrue()
    {
        _questService.ConfigureQuest(_reachAmountQuest);
        
        bool isCompleted = _questService.CheckQuestCompletion(_reachAmountQuest.Title, 10);
        Assert.IsTrue(isCompleted);
    }

    
    [Test]
    public void CheckQuestCompletion_AmountQuest_ValidTypeAndWrongValue_ReturnsFalse()
    {
        _questService.ConfigureQuest(_reachAmountQuest);
        
        bool isCompleted = _questService.CheckQuestCompletion(_reachAmountQuest.Title, 9);
        Assert.IsFalse(isCompleted);
    }
    
    
    [Test]
    public void CheckQuestCompletion_AmountQuest_InvalidTypeAndValue_ThrowsArgumentException()
    {
        _questService.ConfigureQuest(_reachAmountQuest);
        
        var ex = Assert.Throws<ArgumentException>(() => _questService.CheckQuestCompletion(_reachAmountQuest.Title, "10"));
        Assert.That(ex?.Message, Is.EqualTo($"Invalid type for check the quest completion - Input was: String, and the input expected for this quest configuration Int32"));
    }
    
    
    [Test]
    public void CheckQuestCompletion_EventQuest_ValidTypeAndValue_ReturnsTrue()
    {
        _questService.ConfigureQuest(_eventQuest);
        
        bool isCompleted = _questService.CheckQuestCompletion(_eventQuest.Title, "MOAB");
        Assert.IsTrue(isCompleted);
    }

    
    [Test]
    public void CheckQuestCompletion_EventQuest_ValidTypeAndWrongValue_ReturnsFalse()
    {
        _questService.ConfigureQuest(_eventQuest);
        
        bool isCompleted = _questService.CheckQuestCompletion(_eventQuest.Title, "GRENADE");
        Assert.IsFalse(isCompleted);
    }
    
    
    [Test]
    public void CheckQuestCompletion_EventQuest_InvalidTypeAndValue_ThrowsArgumentException()
    {
        _questService.ConfigureQuest(_eventQuest);
        
        var ex = Assert.Throws<ArgumentException>(() => _questService.CheckQuestCompletion(_eventQuest.Title, 150));
        Assert.That(ex?.Message, Is.EqualTo($"Invalid type for check the quest completion - Input was: Int32, and the input expected for this quest configuration String"));
    }
    
    
    [Test]
    public void CheckQuestCompletion_PredictableAmountQuest_ValidMetricAndValues_ReturnsTrue()
    {
        _questService.ConfigureQuest(_predictableReachAmountQuest);

        var predictableValues = new PredictValues<int>(15, 65); 
        
        bool isCompleted = _questService.CheckQuestCompletion(_predictableReachAmountQuest.Title, predictableValues);
        Assert.IsTrue(isCompleted);
    }

    
    [Test]
    public void CheckQuestCompletion_PredictableAmountQuest_ValidTypeAndWrongValue_ReturnsFalse()
    {
        _questService.ConfigureQuest(_predictableReachAmountQuest);

        var predictableValues = new PredictValues<int>(15, 40); 
        
        bool isCompleted = _questService.CheckQuestCompletion(_predictableReachAmountQuest.Title, predictableValues);
        Assert.IsFalse(isCompleted);
    }
    
    
    [Test]
    public void CheckQuestCompletion_PredictableAmountQuest_InvalidTypeAndValue_ThrowsArgumentException()
    {
        _questService.ConfigureQuest(_predictableReachAmountQuest);
        
        var ex = Assert.Throws<ArgumentException>(() => _questService.CheckQuestCompletion(_predictableReachAmountQuest.Title, 10));
        Assert.That(ex?.Message, Is.EqualTo("Invalid type for check the quest completion - Input was: Int32, and the input expected for this quest configuration PredictValues`1"));
    }
    
    
    [Test]
    public void CheckQuestCompletion_QuestNotFound_ThrowsException()
    {
        var ex = Assert.Throws<QuestNotFoundWithTitleException>(() => _questService.CheckQuestCompletion(_reachAmountQuest.Title, 10));
        Assert.That(ex?.Message, Is.EqualTo($"Quest not found for Title {_reachAmountQuest.Title}"));
    }

    
    [Test]
    public void ListActiveQuests_ReturnsAllActiveQuests()
    {
        _questService.ConfigureQuest(_reachAmountQuest);
        _questService.ConfigureQuest(_eventQuest);
        _questService.ConfigureQuest(_predictableReachAmountQuest);
        
        var result = _questService.ListActiveQuests();
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.Contains(_reachAmountQuest, result);
        Assert.Contains(_eventQuest, result);
        Assert.Contains(_predictableReachAmountQuest, result);
    }
}
