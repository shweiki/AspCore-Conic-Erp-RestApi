using Entities.Interfaces;
using System;

namespace Entities;

public interface IUnitOfWork : IDisposable
{
    IBaseRepository<Bank> Banks { get; }
    IBaseRepository<Cash> Cashes { get; }

    int Complete();
}