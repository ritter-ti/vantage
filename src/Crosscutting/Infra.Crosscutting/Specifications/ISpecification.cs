using System;
using System.Linq.Expressions;

namespace Goal.Infra.Crosscutting.Specifications;

public interface ISpecification<TEntity>
    where TEntity : class
{
    Expression<Func<TEntity, bool>> SatisfiedBy();
    bool IsSatisfiedBy(TEntity entity);
}
