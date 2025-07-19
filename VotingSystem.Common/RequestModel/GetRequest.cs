using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Common.RequestModel
{
    public class GetRequest<T> where T : class
    {
        public Expression<Func<T, bool>> Filter { get; set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; }
        public string? SortBy { get; set; }
    }
}
