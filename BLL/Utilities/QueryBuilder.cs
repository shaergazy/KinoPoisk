using System.Text.RegularExpressions;

namespace BLL.Utilities
{
    /// <summary>
    /// This class contains the methods for building the query to be sent to OMDb.
    /// </summary>
    public static class QueryBuilder
	{
		public static string GetItemByTitleQuery(string title, int? year, bool fullPlot = true)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
			}

			var editedTitle = Regex.Replace(title, @"\s+", "+");
			var plot = fullPlot ? "full" : "short";

			var query = $"&t={editedTitle}&plot={plot}";

			if (year != null)
			{
				if (year > 1800)
				{
					query += $"&y={year}";
				}
				else
				{
					throw new ArgumentOutOfRangeException("Year has to be greater than 1800.", nameof(year));
				}
			}
            query += $"&type=movie";
            return query;
		}

		public static string GetItemByIdQuery(string id, bool fullPlot = true)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
			}
            
			var plot = fullPlot ? "full" : "short";

			var query = $"&i={id}&plot={plot}";
            query += $"&type=movie";

            return query;
		}

		public static string GetSearchListQuery(int? year, string query, int page)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(query));
			}
            
			if (page <= 0)
			{
				throw new ArgumentOutOfRangeException("Page has to be greater than zero.", nameof(page));
			}

			var editedQuery = $"&s={Regex.Replace(query, @"\s+", "+")}&page={page}";
            
			if (year != null)
			{
				if (year > 1800)
				{
					editedQuery += $"&y={year}";
				}
				else
				{
					throw new ArgumentOutOfRangeException("Year has to be greater than 1800.", nameof(year));
				}
			}

            editedQuery += $"&type=movie";

            return editedQuery;
		}
	}
}
