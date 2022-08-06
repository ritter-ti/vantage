using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Goal.Seedwork.Infra.Crosscutting.Collections;

namespace Goal.Seedwork.Infra.Crosscutting.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable ForEach(this IEnumerable source, Action<object> action)
        {
            Ensure.Argument.NotNull(source, nameof(source));
            Ensure.Argument.NotNull(action, nameof(action));

            foreach (object item in source)
            {
                action(item);
            }

            return source;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Ensure.Argument.NotNull(source, nameof(source));
            Ensure.Argument.NotNull(action, nameof(action));

            foreach (T element in source)
            {
                action(element);
            }

            return source;
        }

        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> values, IPagination page)
            => values.AsQueryable().Paginate(page);

        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, IPagination page)
        {
            Ensure.Argument.NotNull(page, nameof(page));

            IQueryable<T> queryableList = source;

            if (!page.OrderByName.IsNullOrEmpty())
            {
                queryableList = queryableList.OrderBy(page.OrderByName, page.Ascending);
            }

            queryableList = queryableList.Skip(page.PageIndex * page.PageSize);
            queryableList = queryableList.Take(page.PageSize);

            return queryableList;
        }

        public static async Task<IEnumerable<T>> PaginateAsync<T>(this IEnumerable<T> values, IPagination page, CancellationToken cancellationToken = new CancellationToken())
            => await values.AsQueryable().PaginateAsync(page, cancellationToken);

        public static async Task<IQueryable<T>> PaginateAsync<T>(this IQueryable<T> dataList, IPagination page, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(dataList.Paginate(page));
        }

        public static IPagedCollection<T> PaginateList<T>(this IEnumerable<T> values, IPagination page)
            => values.AsQueryable().PaginateList(page);

        public static IPagedCollection<T> PaginateList<T>(this IQueryable<T> dataList, IPagination page)
        {
            Ensure.Argument.NotNull(page, nameof(page));
            return new PagedList<T>(dataList.Paginate(page).ToList(), dataList.Count());
        }

        public static async Task<IPagedCollection<T>> PaginateListAsync<T>(this IEnumerable<T> values, IPagination page, CancellationToken cancellationToken = new CancellationToken())
            => await values.AsQueryable().PaginateListAsync(page, cancellationToken);

        public static async Task<IPagedCollection<T>> PaginateListAsync<T>(this IQueryable<T> dataList, IPagination page, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.FromResult(dataList.PaginateList(page));
        }

        public static IPagedCollection<TResult> SelectPaged<TSource, TResult>(this IPagedCollection<TSource> source, Func<TSource, TResult> selector)
        {
            IEnumerable<TResult> items = source.Select(selector);
            return new PagedList<TResult>(items, source.TotalCount);
        }

        public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);

        public static string Join<T>(this IEnumerable<T> values, string separator) => string.Join(separator, values);
    }
}
