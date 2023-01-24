using System;
using System.Globalization;
using Xamarin.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using Ch1FlyoutPageModel.ViewModels;

namespace Ch1FlyoutPageModel.Views.Converters
{
    using System.IO;

    public class UriStringToStreamConverter : IValueConverter
    {
        private static Stream? imageData;
        private static readonly string defaultUrl;

        static UriStringToStreamConverter()
        {
            defaultUrl = "http://loremflickr.com/600/600/nature?filename=simple.jpg";
            GetPlaceholderImage();
        }

        private static async void GetPlaceholderImage()
        {
            using (HttpClient client = BaseViewModel.CreateClient(defaultUrl))
            {
                Stream? rsp = await Task.Run(async () =>
                {
                    try
                    {
                        var httpResponse = await client.GetAsync("");
                        if (httpResponse is { })
                        {
                            if (httpResponse is { IsSuccessStatusCode: true, Content: { } cnt })
                            {
                                return await cnt.ReadAsStreamAsync();
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch
                    {
                    }

                    return null;
                });

                imageData = rsp ?? new MemoryStream();
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // System.Func`2[System.Threading.CancellationToken,System.Threading.Tasks.Task`1[System.IO.Stream]]
            string url;
            if (value is string uriString)
            {
                url = uriString;
            }
            else
            {
                url = defaultUrl;
            }

            using (HttpClient client = BaseViewModel.CreateClient(url))
            {
                Stream? rsp = Task.Run(async () =>
                {
                    try
                    {
                        var httpResponse = await client.GetAsync("");
                        if (httpResponse is { })
                        {
                            if (httpResponse is { IsSuccessStatusCode: true, Content: { } cnt })
                            {
                                return await cnt.ReadAsStreamAsync();
                            }
                        }
                    }
                    catch
                    {
                    }

                    return imageData;
                }).Result;
                return rsp ?? new MemoryStream();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
