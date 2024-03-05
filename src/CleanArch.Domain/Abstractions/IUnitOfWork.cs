namespace CleanArch.Domain.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IMemberRepository MemberRepository { get; }
    Task CommitAsync();
}