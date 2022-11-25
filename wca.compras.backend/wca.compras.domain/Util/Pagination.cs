using AutoMapper;
using Org.BouncyCastle.Crypto;
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
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public List<T> Items { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public Pagination(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }
        public static Pagination<T> ToPagedList<TSource>(IQueryable<TSource> source, int page, int pageSize)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, T>();

            });
            var _mapper = config.CreateMapper();

            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => _mapper.Map<T>(r))
                .ToList();
            return new Pagination<T>(items, count, page, pageSize);
        }

        public static Pagination<T> ToPagedList(IQueryable<T> source, int page, int pageSize)
        {
            return ToPagedList<T>(source, page, pageSize);
        }

    }
}
