using CleanArch.Domain.Abstractions;
using CleanArch.Domain.Entities;
using MediatR;

namespace CleanArch.Application.Members.Commands;

public class DeleteMemberCommand : IRequest<Member>
{
    public int Id { get; set; }

    public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Member>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMemberCommandHandler(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        public async Task<Member> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
        {
            var deleteMember = await _unitOfWork.MemberRepository.DeleteMember(request.Id);
            if (deleteMember is null)
                throw new ApplicationException("Member not found.");

            await _unitOfWork.CommitAsync();

            return deleteMember;
        }
    }
}