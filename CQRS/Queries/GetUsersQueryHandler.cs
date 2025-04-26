
using MediatR;
using Repositories; // Replace 'YourNamespace.Repositories' with the actual namespace containing IUserRepository
using Models; // Replace 'YourNamespace.Models' with the actual namespace containing User

namespace CQRS.Queries
{

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserByEmail(request.Email);
        }
    }
}
        