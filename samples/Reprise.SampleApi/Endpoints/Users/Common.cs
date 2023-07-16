namespace Reprise.SampleApi.Endpoints.Users
{
    public record UserDto(int? Id, string FirstName, string LastName);

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

    public class UserMapper : IMapper<User, UserDto>
    {
        public User Map(UserDto source)
        {
            throw new NotImplementedException();
        }

        public UserDto Map(User source)
        {
            return new UserDto(source.Id, source.FirstName, source.LastName);
        }

        public void Map(UserDto source, User destination)
        {
            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
        }

        public void Map(User source, UserDto destination)
        {
            throw new NotImplementedException();
        }
    }
}
