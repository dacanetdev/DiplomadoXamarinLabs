using Android.App;
using Android.Widget;
using Android.OS;

namespace Lab07
{
    [Activity(Label = "Lab07", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var ValidarButton = FindViewById<Button>(Resource.Id.ValidarButton);
            ValidarButton.Click += (sender, e) =>
            {
                Validar();
            };
        }

        private async void Validar()
        {
            var client = new SALLab07.ServiceClient();

            var EmailText = FindViewById<EditText>(Resource.Id.EmailText);
            var PasswordText = FindViewById<EditText>(Resource.Id.PasswordText);
            var myDevice = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);

            var result = await client.ValidateAsync(EmailText.Text, PasswordText.Text, myDevice);
            

            if(Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var notificationText = $"{result.Status} {result.Fullname} {result.Token}";

                var builder = new Notification.Builder(this)
                    .SetContentTitle("Validacion de la Actividad")
                    .SetContentText(notificationText)
                    .SetSmallIcon(Resource.Drawable.Icon);

                builder.SetCategory(Notification.CategoryMessage);

                var ObjectNotifcation = builder.Build();
                var Manager = GetSystemService(Android.Content.Context.NotificationService) as NotificationManager;
                Manager.Notify(0, ObjectNotifcation);
            }
            else
            {
                var message = $"{result.Status}\n{result.Fullname}\n{result.Token}";

                var resultText = FindViewById<TextView>(Resource.Id.ResultText);
                resultText.Text = message;
            }


        }
    }
}

