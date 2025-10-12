using System;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Abstractions.DataAccess;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}