using Ch1FlyoutPageModel.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ch1FlyoutPageModel.ViewModels
{
    using System.Windows.Input;
    using DependencyServices;
    using Xamarin.Forms;
    using ar = AppResources;

    public class ListViewModel : BaseViewModel
    {
        // if this project involved more than one GET request and nothing else, I would
        // put the httpclient stuff into it's own subclass, or better still auto-generate
        // it with NSwag or the like.
        private static readonly HttpClient clientBacking = CreateClient();
        private HttpClient Client => clientBacking ?? CreateClient();

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

        public ICommand RefreshCommand { get; set; }

        public ListViewModel()
        {
            RefreshCommand = new Command(async (sender) =>
            {
                await GetListAsync();
                if (sender is RefreshView rv) { rv.IsRefreshing = false; }
            });
        }

        public async Task<ObservableCollection<Album>?> GetListAsync()
        {
            try
            {
                var res = await Client.GetAsync("albums/1/photos");
                if (!res.IsSuccessStatusCode || res.Content is not { } httpContent) { return null; }

                var content = JsonConvert.DeserializeObject<List<Album>>(await httpContent.ReadAsStringAsync());
                if (content is { Count: > 0 })
                {
                    DependencyService.Get<IToastMessage>()
                        .Show(string.Format(ar.ToastNumberOfRecordsReturned, content.Count));
                    return new(content);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
