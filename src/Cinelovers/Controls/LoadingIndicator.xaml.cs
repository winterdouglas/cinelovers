using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cinelovers.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingIndicator : ContentView
    {
        public static readonly BindableProperty IsLoadingProperty =
            BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(LoadingIndicator), 
                defaultValue: default(bool),
                propertyChanged: IsLoadingPropertyChanged);

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public LoadingIndicator()
        {
            InitializeComponent();
        }

        private static void IsLoadingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LoadingIndicator control)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var isLoading = (bool)newValue;
                    var start = isLoading ? -20 : 0;
                    var end = isLoading ? 0 : -20;
                    var transitionName = isLoading ? "Showing" : "Hiding";

                    var storyboard = new Animation();
                    var translateYAnimation = new Animation(
                        callback: value => control.LoadingLabel.TranslationY = value,
                        start: start,
                        end: end,
                        easing: Easing.CubicInOut);

                    storyboard.Add(0, 1, translateYAnimation);
                    storyboard.Commit(control.LoadingLabel, transitionName, length: 150);
                });
            }
        }
    }
}