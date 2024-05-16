using FluentValidation;

namespace QuestSystem.Application.Quests.Commands.ConfigureQuest;

public class ConfigureQuestCommandValidator : AbstractValidator<ConfigureQuestCommand>
{
    public ConfigureQuestCommandValidator()
    {
        // RuleFor(v => v.Title)
        //     .MaximumLength(200)
        //     .NotEmpty();
        //
        // RuleFor(v => v.Description)
        //     .MaximumLength(200)
        //     .NotEmpty();
    }
}
