using Ch1FlyoutPageModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.ViewModels
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using DependencyServices;

    public class BaseViewModel : INotifyPropertyChanged
    {
        public bool HasAllNecessaryPermissions => Permissions.Where(p=>p.IsEssentialForAppToRunProperly).All(p=>p.IsGranted);
        public bool IsBluetoothOn => DependencyService.Get<IDevices>().BtIsOn;

        public bool IsGpsOn => DependencyService.Get<IDevices>().GpsIsOn;

        // Yes, yes, I'm aware that the below doesn't actually guarantee Internet access.
        // We could always ping google.com or something like it if we absolutely needed to be sure.
        public bool IsInternetAvailable => Connectivity.NetworkAccess == NetworkAccess.Internet;
        protected static ObservableCollection<IChPermission> permissions = new();

        public ObservableCollection<IChPermission> Permissions
        {
            get => permissions;
            set
            {
                SetProperty(ref permissions, value);
                OnPropertyChanged(nameof(HasAllNecessaryPermissions));
            }
        }

        bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler? changed = PropertyChanged;
            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public static HttpClient CreateClient(string? baseUrl = null)
        {
            baseUrl ??= "https://jsonplaceholder.typicode.com";
            var client =
                new HttpClient(new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                }) { BaseAddress = new(baseUrl), Timeout = new(0, 0, 1, 0), };
            client.DefaultRequestHeaders.TryAddWithoutValidation("UserAgent",
                new[]
                {
                    "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; InfoPath.1; .NET CLR 2.0.50727)"
                });
            // client.DefaultRequestHeaders.TryAddWithoutValidation("UserAgent", new[] { "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0" });
            client.DefaultRequestHeaders.TryAddWithoutValidation("cookie",
                new[] { "_ga_135DDP1L05=GS1.1.1674398675.3.0.1674398675.0.0.0; _ga=GA1.2.917325757.1674365588" });
            // "cookie": "_ga_135DDP1L05=GS1.1.1674398675.3.0.1674398675.0.0.0; _ga=GA1.2.917325757.1674365588"
            // client.DefaultRequestHeaders.Add("UserAgent", new[] { "Ch1FlyoutPageModel" });
            // client.DefaultRequestHeaders.Add("Expect", new[] { "1.0.0.0" });
            // client.DefaultRequestHeaders.Add("ClkOnceV", new[] { "DEBUG version" });

            return client;
        }
    }
}
