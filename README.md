# **Challenge 1: FlyoutPage Model**

- **Xamarin.Forms app** (focus on Android)
- **MVVM pattern**
- **API 30 target (tested on my Galaxy s9 which is API 29...)**
- **Permissions asked at startup**
  - It's worth noting that Google specifically discourages this practice, but for the purposes of proving that I can indeed implement such things, it makes far more sense to do permissions the "old" way.

## Permissions

- First up (at `App.OnStart()`) we check the 3 permissions we need and if they are not granted we store each into an IEnumerable of a class that contains the permission name, description and rationale for display.
- We then ask the user for each permission, and if we get it we remove that permission from our IEnumerable.
- When the first page is navigated to via our `AppShell`, It will display those permissions we don't yet have, and present an option to ask again.
- Originally I was going to use Xamarin.Essentials.Permissions, but it mapped confusingly with the (relatively new) ActivityRecognition permission in Android.