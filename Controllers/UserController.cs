using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CQRS.Commands;
using CQRS.Queries;
// using Models;


namespace Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var user = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUser), new { email = user.Email }, user);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            // Assuming you have a method to get a user by ID
            var user = await _mediator.Send(new GetUsersQuery(email));
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}