using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Mvp.Feature.People.Extensions
{
    public static class HttpRequestExtension
	{
		public static string UpdatePagerInUrl(this HttpRequest request, string key, string value)
		{
			var url = request.Path;
			var queryKvp = request.Query.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value)).ToList(); ;

			var existingQs = queryKvp.Where(q => q.Key.Equals(key)).FirstOrDefault();
			if (existingQs.Equals(new KeyValuePair<string, string>()))
			{
				queryKvp.Add(new KeyValuePair<string, string>(key, value));
			}
			else
			{
				string newValue = value;
				queryKvp.Remove(existingQs);
				queryKvp.Add(new KeyValuePair<string, string>(key, newValue));
			}

			string queryString = QueryString.Create(queryKvp).ToUriComponent();
			return WebUtility.UrlEncode(url + queryString);
		}

	}
}
