using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Models;
using Repositories;

namespace CQRS.Commands
{   

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.Id, request.Email, request.Password, request.UserType);
            await _userRepository.CreateUser(user);
            return user;
        }
    }
}