using Ch1FlyoutPageModel.ViewModels;

namespace Ch1FlyoutPageModel.Views
{
    public partial class ListPage
    {
        public ListPage()
        {
            InitializeComponent();
        }

        public string ErrorPlaceholder => "https://makeawebsitehub.com/wp-content/uploads/2019/08/shrug-emoji-type.png";

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
