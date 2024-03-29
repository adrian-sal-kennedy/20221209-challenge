﻿using Ch1FlyoutPageModel.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace Ch1FlyoutPageModel.Views
{
    using Xamarin.Forms;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PermissionsPage
    {
        private static bool HaveWeBeenToThisPageBefore;
        private PermissionsViewModel? bc = null;

        public PermissionsPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is PermissionsViewModel vm)
            {
                bc = vm;
                Task.Run(async () =>
                {
                    if (!HaveWeBeenToThisPageBefore)
                    {
                        HaveWeBeenToThisPageBefore = true;
                        await vm.AskPermissions();
                        if (vm.HasAllNecessaryPermissions)
                        {
                            await AppShell.GoToOnMainThreadAsync("//List");
                        }
                    }
                });
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is not PermissionsViewModel)
            {
                BindingContext = bc;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            BindingContext = null;
        }
    }
}
