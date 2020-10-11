using System;
using System.Linq;
using System.Linq.Expressions;
using TrickingLibrary.Models;

namespace TrickingLibrary.Data
{
    public static class QueryExtensions
    {
        public static IQueryable<Submission> PickSubmissions(
            this IQueryable<Submission> source,
            string order,
            int cursor)
        {
            Expression<Func<Submission, object>> orderBySelector = order switch
            {
                "latest" => submission => submission.Created,
                "top" => submission => submission.UpVotes.Count,
                _ => _ => 1,
            };

            return source
                .OrderByDescending(orderBySelector)
                .Skip(cursor)
                .Take(10);
        }
    }
}