using Ch1FlyoutPageModel.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}