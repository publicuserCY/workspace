using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo4DotNetCore.AuthorizationServer.Model
{
    /// <summary>
    /// PaginatedList
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> : List<T>
    {
        /// <summary>
        /// PageIndex
        /// </summary>       
        public int PageIndex { get; private set; }

        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// TotalCount
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// TotalPageCount
        /// </summary>
        public int TotalPageCount { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="source"></param>
        public PaginatedList(
            int pageIndex, int pageSize,
            int totalCount, IQueryable<T> source)
        {
            AddRange(source);

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public PaginatedList(
            int pageIndex, int pageSize,
            int totalCount, int totalPageCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = totalPageCount;
        }

        /// <summary>
        /// HasPreviousPage
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        /// <summary>
        /// HasNextPage
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPageCount);
            }
        }
    }
}