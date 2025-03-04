namespace Application.Interfaces;

public interface IUnitOfWork
{
    // AppointmentsRepository AppointmentsRepository { get; }

    Task<bool> Complete();
    bool HasChanges();
}