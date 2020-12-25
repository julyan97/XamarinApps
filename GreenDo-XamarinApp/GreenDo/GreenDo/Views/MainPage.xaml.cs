using System.Linq;
using Xamarin.Forms;

namespace GreenDo.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Heart_Clicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            Like("💗", button);
        }

        private void Button_Like_Clicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            Like("🌱", button);
        }

        private void Like(string emoji, Button button)
        {
            var text = button.Text;
            var numberStr = text.Split(' ').Last();
            var number = int.Parse(numberStr); number++;
            button.Text = emoji + " " + number;
            button.BackgroundColor = Color.FromHex("#46b172");
        }
    }
}
