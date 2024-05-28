using FluentValidation;

namespace QuestSystem.Application.Quests.Commands.ConfigurePlatformQuest;

public class ConfigurePlatformQuestCommandValidator : AbstractValidator<ConfigurePlatformQuestCommand>
{
    public ConfigurePlatformQuestCommandValidator()
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
