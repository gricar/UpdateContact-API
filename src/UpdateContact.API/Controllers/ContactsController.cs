using MediatR;
using Microsoft.AspNetCore.Mvc;
using UpdateContact.Application.Contact.Commands.Update;
using static Microsoft.AspNetCore.Http.StatusCodes;


namespace UpdateContact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _dispatcher;

        public ContactsController(IMediator dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UpdateContactCommandResponse), Status202Accepted)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<UpdateContactCommandResponse>> UpdateContact(
            [FromRoute] Guid id, [FromBody] UpdateContactCommand command, CancellationToken cancellationToken)
        {
            var response = await _dispatcher.Send(command, cancellationToken);
            return Accepted(response);
        }
    }
}
