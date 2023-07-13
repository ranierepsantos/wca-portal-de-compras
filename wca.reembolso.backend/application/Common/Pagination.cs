using AutoMapper;

namespace application.Common
{
    public sealed class Pagination<T>
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
        public static Pagination<T> ToPagedList<TSource>(IMapper _mapper, IQueryable<TSource> source, int page, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => _mapper.Map<T>(r))
                .ToList();
            return new Pagination<T>(items, count, page, pageSize);
        }

        public static Pagination<T> ToPagedList(IMapper _mapper, IQueryable<T> source, int page, int pageSize)
        {
            return ToPagedList<T>(_mapper, source, page, pageSize);
        }

    }
}
