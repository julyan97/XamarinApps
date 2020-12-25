using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;

namespace androidOG
{
    [Activity(Label = "Activity1")]
    public class Level1Activity : Activity
    {
   
        public List<string> Animal { get; set; } = new List<string>()
        {
            "cow",
            "dog",
            "cat",
            "sheep",
            "horse"
        };

        private Dictionary<Button, string> buttonMap = new Dictionary<Button, string>();
        public void Relocate(Intent re)
        {
            StartActivity(re);
        }
        void ButtonClicked(Button cur,Dictionary<Button,string> map,TextView compare)
        {
            cur.Click += delegate
              {
                  if(map[cur].Trim()==compare.Text.Trim())
                  {
                      compare.Text = "Congrates you win";
                      MediaPlayer win = MediaPlayer.Create(this,Resource.Raw.applause);
                      win.Start();
                      Intent home = new Intent(this,typeof(MainActivity));
                      Relocate(home);
                  }
              };
        }
        void setImgeOnButton(Button cur)
        {
            if (cur.Text == "cat")
            {

                cur.SetBackgroundResource(Resource.Drawable.cat);
            }
            if (cur.Text == "dog")
            {

                cur.SetBackgroundResource(Resource.Drawable.dog);
            }
            if (cur.Text == "sheep")
            {

                cur.SetBackgroundResource(Resource.Drawable.sheep);
            }
            if (cur.Text == "cow")
            {

                cur.SetBackgroundResource(Resource.Drawable.cow);
            }
            if (cur.Text == "horse")
            {

                cur.SetBackgroundResource(Resource.Drawable.horse);
            }
        }

        void StartSound(TextView animal)
        {

            if(animal.Text=="cow".Trim())
            {
                MediaPlayer play = MediaPlayer.Create(this, Resource.Raw.Cow);
                play.Start();
            }
            if (animal.Text == "dog".Trim())
            {
                MediaPlayer play = MediaPlayer.Create(this, Resource.Raw.Dog);
                play.Start();
            }
            if (animal.Text == "cat".Trim())
            {
                MediaPlayer play = MediaPlayer.Create(this, Resource.Raw.Cat);
                play.Start();
            }
            if (animal.Text == "sheep".Trim())
            {
                MediaPlayer play = MediaPlayer.Create(this, Resource.Raw.Sheep);
                play.Start();
            }
            if (animal.Text == "horse".Trim())
            {
                MediaPlayer play = MediaPlayer.Create(this, Resource.Raw.horse_blow);
                play.Start();
            }
        }
        
        public string GetRandomAnimal()
        {
            Random r = new Random();
            int num = r.Next(0, 4);
            var animal = Animal[num];
            return animal;
        }
        public  void Shuffle(List<string> list)
        {
            int n = list.Count;
            Random rng = new Random();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.level1);
            
            var animal = FindViewById<TextView>(Resource.Id.animalView);
            animal.Text = GetRandomAnimal();

            var animal1 = FindViewById<Button>(Resource.Id.button1);
            var animal2 = FindViewById<Button>(Resource.Id.button2);
            var animal3 = FindViewById<Button>(Resource.Id.button3);
            var animal4 = FindViewById<Button>(Resource.Id.button4);
       

            List<Button> buttonList= new List<Button>()
            {
                animal1,
                animal2,
                animal3,
                animal4
            };
            StartSound(animal);
            Shuffle(Animal);
            for (int i = 0; i < 4; i++)
            {
                buttonMap[buttonList[i]] = Animal[i];
            }

            for (int i = 0; i < buttonList.Count; i++)
            {
                ButtonClicked(buttonList[i], buttonMap, animal);
                buttonList[i].Text = buttonMap[buttonList[i]];
                setImgeOnButton(buttonList[i]);
                buttonList[i].Text = "";
            }
            
        }
    }
}