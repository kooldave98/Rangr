using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Core.Portable.Network
{
	public interface IHttpRequest
    {

		Task<string> Put(string baseUrl, List<KeyValuePair<string, string>> data);

		Task<string> Post(string baseUrl, List<KeyValuePair<string, string>> data);

        Task<string> Get(string baseUrl);
    }
}
