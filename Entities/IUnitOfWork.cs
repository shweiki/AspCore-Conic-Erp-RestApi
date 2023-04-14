using Domain.Interfaces;
using System;

namespace Domain;

public interface IUnitOfWork : IDisposable
{
    IBaseRepository<Bank> Banks { get; }
    IBaseRepository<Cash> Cashes { get; }

    int Complete();
}