
using MediatR;
using Models;

namespace CQRS.Commands
{
    public class CreateUserCommand : IRequest<User>
    {
        public string Id { get; }
        public string Email { get; }

        public string Password { get; }

        public string UserType { get; }

        public CreateUserCommand(string id, string email, string password, string userType)
        {
            Id = id;
            Email = email;
            Password = password;
            UserType = userType;
        }
        
    }
}