using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataTableServerSide.AspNetCore
{
    public class DataTableHelper<T>
    {
        private IQueryable<T> _data;
        private readonly HttpRequest _request;

        public DataTableHelper(IList<T> data, HttpRequest request)
        {
            _data = data.AsQueryable();
            _request = request;
        }

        public DataTableResult<T> GetDataTableResult()
        {
            var draw = int.Parse(_request.Form["draw"].FirstOrDefault() ?? "0");
            var sortColumn = _request.Form["columns[" + _request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = _request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = _request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(_request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(_request.Form["start"].FirstOrDefault() ?? "0");

            var totalRecord = _data.Count();

            var filteredData = ApplyFilters(searchValue);
            var filterRecord = filteredData.Count();
            var sortedData = ApplySorting(sortColumn, sortColumnDirection);

            var resultData = sortedData.Skip(skip).Take(pageSize).ToList();

            return new DataTableResult<T>
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = resultData
            };
        }

        private IQueryable<T> ApplyFilters(string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower();
                _data = _data.Where(x => PropertyContains(x, searchValue));
            }

            return _data;
        }

        private IQueryable<T> ApplySorting(string sortColumn, string sortColumnDirection)
        {
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                return OrderByColumn(sortColumn, sortColumnDirection);

            return _data;
        }

        private IQueryable<T> OrderByColumn(string sortColumn, string sortColumnDirection)
        {
            if (sortColumnDirection.ToLower() == "asc")
                return _data.OrderBy(x => GetPropertyValue(x, sortColumn));
            else
                return _data.OrderByDescending(x => GetPropertyValue(x, sortColumn));
        }

        private static bool PropertyContains(T item, string searchValue)
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var propertyValue = GetPropertyValue(item, property.Name);
                if (propertyValue != null && propertyValue.ToString().ToLower().Contains(searchValue))
                    return true;
            }

            return false;
        }

        private static object GetPropertyValue(T item, string propertyName)
        {
            return item.GetType().GetProperty(propertyName)?.GetValue(item, null);
        }
    }
}
