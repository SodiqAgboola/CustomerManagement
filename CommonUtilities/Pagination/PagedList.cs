using IPagedList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities.Pagination
{
    /// <summary>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <remarks>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </remarks>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <seealso cref="IPagedList{T}"/>
    /// <seealso cref="BasePagedList{T}"/>
    /// <seealso cref="StaticPagedList{T}"/>
    /// <seealso cref="List{T}"/>
    [JsonObject]
    public class PagedList<T> : BasePagedList<T>
    {
        public PagedList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset into subsets the size of the supplied pageSize. The instance then only containes the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <param name="useTotalCount">Used to add total count to the returned query. Set to true if table count is quite large</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public PagedList(IQueryable<T> superset, int pageNumber, int pageSize, bool useTotalCount = true)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "PageNumber cannot be below 1.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");

            // set source to blank list if superset is null to prevent exceptions
            PageSize = pageSize;
            PageNumber = pageNumber;

            if (useTotalCount)
            {
                TotalItemCount = superset == null ? 0 : superset.Count();

                PageCount = TotalItemCount > 0
                    ? (int)Math.Ceiling(TotalItemCount / (double)PageSize)
                    : 0;
                HasPreviousPage = PageNumber > 1;
                HasNextPage = PageNumber < PageCount;


                //IsFirstPage = PageNumber == 1;
                //IsLastPage = PageNumber >= PageCount;
                //FirstItemOnPage = (PageNumber - 1) * PageSize + 1;
                //var numberOfLastItemOnPage = FirstItemOnPage + PageSize - 1;
                //LastItemOnPage = numberOfLastItemOnPage > TotalItemCount
                //                ? TotalItemCount
                //                : numberOfLastItemOnPage;

                // add items to internal list
                if (superset != null && TotalItemCount > 0)
                    Subset.AddRange(pageNumber == 1
                        ? superset.Skip(0).Take(pageSize).ToList()
                        : superset.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
                    );


            }
            else
            {
                // add items to internal list
                if (superset != null)
                    Subset.AddRange(pageNumber == 1
                        ? superset.Skip(0).Take(pageSize).ToList()
                        : superset.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
                    );
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class that divides the supplied superset into subsets the size of the supplied pageSize. The instance then only containes the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public PagedList(IEnumerable<T> superset, int pageNumber, int pageSize)
            : this(superset.AsQueryable<T>(), pageNumber, pageSize)
        {
        }

        public override IEnumerable<T> Items => Subset;

        public void SetItems(IEnumerable<T> items) => Subset.AddRange(items);
    }
}
