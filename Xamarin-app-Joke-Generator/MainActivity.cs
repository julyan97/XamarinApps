using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content.Res;
using System.IO;
using System;

namespace secondary
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public string GetLineFromFileAtIndex(int index, string path)
        {
            AssetManager assets = this.Assets;
            string content = "";
            using (StreamReader sr = new StreamReader(assets.Open(path)))
            {
                for (int i = 1; i <= index; i++)
                {
                    content = sr.ReadLine();
                }

                sr.Close();
            }
            return content;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //reference to the generate button
            var button = FindViewById<Button>(Resource.Id.generate);
            //reference to the joke textview
            var joke = FindViewById<TextView>(Resource.Id.joke);


            //onclick read a random line from a file then generate it at the textview
            button.Click += delegate
            {
                Random r = new Random();
                int index = r.Next(1, 31);
                joke.Text = GetLineFromFileAtIndex(index, "file.txt.txt");
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}