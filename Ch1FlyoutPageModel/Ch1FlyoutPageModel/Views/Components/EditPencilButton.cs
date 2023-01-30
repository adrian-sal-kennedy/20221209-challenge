namespace Ch1FlyoutPageModel.Views.Components
{
    using Xamarin.Forms;
    using Xamarin.Forms.Shapes;
    public class EditPencilIconPath : Path
    {
        private readonly string outline = $"M 8,21 9,20 4,15 3,16 2,22 Z M 22,7 17,2 5,14 l 5,5 Z";

        public EditPencilIconPath()
        {
            Data = (Geometry)new PathGeometryConverter().ConvertFromInvariantString(outline);
        }
    }

    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public class EditPencilIcon : ContentView
    {
        public static readonly BindableProperty DiameterProperty =
            BindableProperty.CreateAttached(
                nameof(Diameter),
                typeof(double),
                typeof(EditPencilIcon),
                default(double),
                BindingMode.TwoWay
            );

        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set
            {
                SetValue(DiameterProperty, value);
            }
        }

        public static readonly BindableProperty FillProperty =
            BindableProperty.CreateAttached(
                nameof(Fill),
                typeof(Brush),
                typeof(EditPencilIcon),
                default(Brush),
                BindingMode.TwoWay
            );

        public Brush Fill
        {
            get
            {
                return (Brush)GetValue(FillProperty);
            }
            set
            {
                SetValue(FillProperty, value);
            }
        }

        public EditPencilIcon()
        {
            WidthRequest = Diameter;
            HeightRequest = Diameter;
            Visual = VisualMarker.Material;
            Padding = 0;
            Margin = 0;
            Diameter = 24;
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;
            Content = new EditPencilIconPath()
            {
                HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center,
            };
            Fill = new SolidColorBrush(Color.Black);
            ((Path)Content).Fill = Fill;
        }

        protected override void OnPropertyChanged(string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Fill):
                    ((Path)Content).Fill = Fill;
                    break;
                case nameof(WidthRequest):
                case nameof(HeightRequest):
                case nameof(Diameter):
                    if (Diameter > 0)
                    {
                        WidthRequest = Diameter;
                        HeightRequest = Diameter;
                        Scale = Diameter / 24; // SVG artwork is created on a 24x24 canvas
                    }

                    break;
            }
        }
    }
}
