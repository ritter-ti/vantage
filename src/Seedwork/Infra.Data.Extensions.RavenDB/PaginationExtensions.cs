using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Goal.Seedwork.Infra.Crosscutting;
using Goal.Seedwork.Infra.Crosscutting.Collections;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace Goal.Seedwork.Infra.Data.Extensions.RavenDB;

public static class PaginationExtensions
{
    public static IRavenQueryable<T> Paginate<T>(this IRavenQueryable<T> source, IPageSearch pageSearch)
    {
        Ensure.Argument.IsNotNull(pageSearch, nameof(pageSearch));

        IRavenQueryable<T> queryableList = source;

        if (!string.IsNullOrWhiteSpace(pageSearch.SortBy))
        {
            queryableList = queryableList.OrderBy(pageSearch.SortBy, pageSearch.SortDirection);
        }

        queryableList = queryableList.Skip(pageSearch.PageIndex * pageSearch.PageSize);
        queryableList = queryableList.Take(pageSearch.PageSize);

        return queryableList;
    }

    public static IPagedCollection<T> ToPagedList<T>(this IRavenQueryable<T> source, IPageSearch pageSearch)
    {
        Ensure.Argument.IsNotNull(pageSearch, nameof(pageSearch));

        var data = source
            .Statistics(out QueryStatistics stats)
            .Paginate(pageSearch)
            .ToList();

        return new PagedList<T>(
            data,
            stats.TotalResults);
    }

    public static async Task<IPagedCollection<T>> ToPagedListAsync<T>(this IRavenQueryable<T> source, IPageSearch pageSearch, CancellationToken cancellationToken = new CancellationToken())
    {
        Ensure.Argument.IsNotNull(pageSearch, nameof(pageSearch));

        List<T> data = await source
            .Statistics(out QueryStatistics stats)
            .Paginate(pageSearch)
            .ToListAsync(cancellationToken);

        return new PagedList<T>(
            data,
            stats.TotalResults);
    }
}
