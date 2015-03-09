using System.Collections.Generic;
using log4net;
using Microsoft.WindowsAzure.Storage.Table;

namespace Blog.Bl
{
    public class StorageQuery<T> where T : TableEntity, new()
    {
        protected TableQuery<T> Query;

        public StorageQuery()
        {
            Query = new TableQuery<T>();
        }

        public virtual IEnumerable<T> ExecuteOn(CloudTable table)
        {
            var token = new TableContinuationToken();
            var segment = table.ExecuteQuerySegmented(Query, token);
            while (token != null)
            {
                foreach (var result in segment)
                {
                    yield return result;
                }

                token = segment.ContinuationToken;
                segment = table.ExecuteQuerySegmented(Query, token);
            }
        }

        protected string InclusiveRangeFilter(string key, string from, string to)
        {
            var low = TableQuery.GenerateFilterCondition(key, QueryComparisons.GreaterThanOrEqual, from);
            var high = TableQuery.GenerateFilterCondition(key, QueryComparisons.LessThanOrEqual, to);
            return TableQuery.CombineFilters(low, TableOperators.And, high);
        }
    }
}