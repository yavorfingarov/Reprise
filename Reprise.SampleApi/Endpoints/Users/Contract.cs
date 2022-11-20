using FluentValidation;

namespace Reprise.SampleApi.Endpoints.Users
{
    public record UserDto(string FirstName, string LastName);

    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty();
            RuleFor(u => u.LastName)
                .NotEmpty();
        }
    }
}
