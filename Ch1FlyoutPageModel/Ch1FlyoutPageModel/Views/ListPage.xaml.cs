using Ch1FlyoutPageModel.ViewModels;

namespace Ch1FlyoutPageModel.Views
{
    public partial class ListPage
    {
        public ListPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ListViewModel { } vm)
            {
                vm.Albums = await vm.GetListAsync();
            }
        }
    }
}