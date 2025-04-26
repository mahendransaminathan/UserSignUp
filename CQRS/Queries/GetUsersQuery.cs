using System.Collections.Generic;
using MediatR;
using Models;

namespace CQRS.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<User>>
    {
        public string Email { get; }

        public GetUsersQuery(string email)
        {
            Email = email;
        }
    }
}