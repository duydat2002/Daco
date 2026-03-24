namespace Daco.Application.Administration.UserManagement.Commands
{
    public class SuspendUserCommandHandler : IRequestHandler<SuspendUserCommand, ResponseDTO>
    {
        public async Task<ResponseDTO> Handle(SuspendUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
