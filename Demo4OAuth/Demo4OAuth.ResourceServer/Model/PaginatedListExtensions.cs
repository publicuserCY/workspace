using Dtsc.TPCIS.Model.Core;
using System.Collections.Generic;

namespace Dtsc.TPCIS.Model.Extensions
{
    public static class PaginatedListExtensions
    {
        public static PaginatedDto<TDto> ToPaginatedDto<TDto, TEntity>(this PaginatedList<TEntity> source,IEnumerable<TDto> items) where TDto : IDto
        {
            return new PaginatedDto<TDto>
            {
                PageIndex = source.PageIndex,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                TotalPageCount = source.TotalPageCount,
                HasNextPage = source.HasNextPage,
                HasPreviousPage = source.HasPreviousPage,
                Items = items
            };
        }

        public static PaginatedViewModel<TViewModel> ToPaginatedViewModel<TViewModel, TEntity>(this PaginatedList<TEntity> source, IEnumerable<TViewModel> items) where TViewModel : IViewModel
        {
            return new PaginatedViewModel<TViewModel>
            {
                PageIndex = source.PageIndex,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                TotalPageCount = source.TotalPageCount,
                HasNextPage = source.HasNextPage,
                HasPreviousPage = source.HasPreviousPage,
                Items = items
            };
        }
    }
}
