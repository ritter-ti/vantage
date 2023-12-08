using System;
using Goal.Seedwork.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Goal.Seedwork.Infra.Data;

public abstract class Repository<TEntity>(DbContext context) : Repository<TEntity, Guid>(context), IRepository<TEntity>
    where TEntity : class
{
}
