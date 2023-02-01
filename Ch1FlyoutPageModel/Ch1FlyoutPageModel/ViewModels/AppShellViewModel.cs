namespace Ch1FlyoutPageModel.ViewModels
{
    using Models;
    using Newtonsoft.Json;
    using Xamarin.Essentials;

    public class AppShellViewModel : BaseViewModel
    {
        private static User? appUser;
        private static AppShellViewModel? current;
        public User AppUser
        {
            get => appUser!;
            set => SetProperty(ref appUser, value);
        }

        public AppShellViewModel()
        {
            appUser = new User()
            {
                FirstName = "John", MiddleName = "F", LastName = "Kennedy", NickName = "Spud",
            };
            current = this;
        }

        public static async void GetUser()
        {
            var usrJson = await SecureStorage.GetAsync("appUser");
            if (usrJson is { })
            {
                current!.AppUser = JsonConvert.DeserializeObject<User>(usrJson)!;
            }
        }
        public static async void SetUser()
        {
            await SecureStorage.SetAsync("appUser", JsonConvert.SerializeObject(appUser));
        }
    }
}
