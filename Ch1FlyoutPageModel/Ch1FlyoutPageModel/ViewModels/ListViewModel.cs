using Ch1FlyoutPageModel.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ch1FlyoutPageModel.ViewModels
{
    public class ListViewModel : BaseViewModel
    {
        // if this project involved more than one GET request and nothing else, I would
        // put the httpclient stuff into it's own subclass, or better still auto-generate
        // it with NSwag or the like.
        private static HttpClient clientBacking = CreateClient();
        private HttpClient client => clientBacking ?? CreateClient();
        private static ObservableCollection<Album>? albums;
        // if this is null, it just returns an empty list. The Page's OnAppearing
        // lifecycle event will hit the API endpoint once and set this when ready.
        public ObservableCollection<Album>? Albums
        {
            get => albums ?? new();
            set
            {
                if (value is { } && value != albums)
                {
                    SetProperty(ref albums, value);
                }
            }
        }
        public ListViewModel()
        {
        }
        private static HttpClient CreateClient()
        {
            return new(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate, })
            {
                BaseAddress = new("https://jsonplaceholder.typicode.com"),
                Timeout = new(0, 0, 1, 0),
            };
        }
        private async Task<ObservableCollection<Album>?> GetListAsync()
        {
            var res = await client.GetAsync("albums/1/photos");
            if (!res.IsSuccessStatusCode || res.Content is not { } httpContent) { return null; }
            var content = JsonConvert.DeserializeObject<List<Album>>(await httpContent.ReadAsStringAsync());
            if (content is { })
            {
                return new(content);
            }
            else { return null; }
        }
    }
}