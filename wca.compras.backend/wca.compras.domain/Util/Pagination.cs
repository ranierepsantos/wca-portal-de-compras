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

        public List<T> Items { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;


        public Pagination(List <T> items, int currentPage, int pageSize, int totalPages)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            Items = items;
        }

        
    }
}
