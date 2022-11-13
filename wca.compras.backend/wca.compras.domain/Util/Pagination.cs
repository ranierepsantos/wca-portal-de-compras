using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.compras.domain.Util
{
    public class Pagination<T>
    {
        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }

        public IList<T> Items { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;


        public Pagination(List<T> items, int currentPage, int pageSize, int count)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (int) Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public static Pagination<T> toPageList(IQueryable<T> source, int page, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new Pagination<T>(items, page, pageSize, count);
        }
    }
}
