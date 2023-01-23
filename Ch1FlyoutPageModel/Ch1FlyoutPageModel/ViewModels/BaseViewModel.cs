using Ch1FlyoutPageModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.ViewModels
{
    using System.Threading.Tasks;
    using DependencyServices;

    public class BaseViewModel : INotifyPropertyChanged
    {
        public bool HasAllPermissions => MissingPermissions is { Count: 0 };
        private static bool isBluetoothOn;
        public bool IsBluetoothOn => isBluetoothOn;
        public bool IsGpsOn { get; set; } = true;
        public bool IsInternetAvailable { get; set; }
        protected static ObservableCollection<IChPermission> missingPermissions = new();
        public ObservableCollection<IChPermission> MissingPermissions
        {
            get => missingPermissions;
            set
            {
                SetProperty(ref missingPermissions, value);
                OnPropertyChanged(nameof(HasAllPermissions));
            }
        }
        bool isBusy = false;
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
        public BaseViewModel()
        {
            MessagingCenter.Subscribe<BaseViewModel>(this, "BtPropertyChanged", (_) => OnPropertyChanged(nameof(IsBluetoothOn)));
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        public static void SetIsBluetoothOn(bool arg)
        {
            isBluetoothOn = arg;
            // hax to update all instances to fire propertychanged from a static method.
            MessagingCenter.Send(new BaseViewModel(),"BtPropertyChanged");
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public async void AskPermission(IChPermission? perm)
        {
            await Task.Run(async () =>
            {
                bool res = await DependencyService.Get<IPermissionAsker>().AskPermission(perm);
                if (res)
                {
                    missingPermissions.Remove(perm);
                    OnPropertyChanged(nameof(MissingPermissions));
                }
            });
        }
    }
}
