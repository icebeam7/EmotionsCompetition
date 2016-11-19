using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.IO;
using EmotionsCompetition.Classes;

namespace EmotionsCompetition.Pages
{
	public partial class CompetitionPage : ContentPage
	{
        static Stream streamCopy;
        float width, height;

        public CompetitionPage ()
		{
			InitializeComponent ();
		}

        async void PicButton_Clicked(object sender, EventArgs e)
        {
            var useCamera = ((Button)sender).Text.Contains("Take");
            var file = await ImageService.TakePic(useCamera);
            ResultsPanel.Children.Clear();
            ResultsLabel.Text = "---";

            Pic.Source = ImageSource.FromStream(() => {
                var stream = file.GetStream();
                streamCopy = new MemoryStream();
                stream.CopyTo(streamCopy);
                stream.Seek(0, SeekOrigin.Begin);
                file.Dispose();
                return stream;
            });
        }

        async void PlayButton_Clicked(object sender, EventArgs e)
        {
            if (streamCopy != null)
            {
                streamCopy.Seek(0, SeekOrigin.Begin);
                int index = EmotionsPicker.SelectedIndex;

                var persons = await EmotionsService.Analyze(streamCopy, index);

                if (persons != null)
                {
                    ResultsLabel.Text = "---Emotions Analysis---";
                    ShowResults(persons);
                }
                else ResultsLabel.Text = "---Face was not detected---";
            }
            else ResultsLabel.Text = "---Picture has not been taken/selected---";
        }

        void ShowResults(List<Person> persons)
        {
            ResultsPanel.Children.Clear();

            foreach (var person in persons)
            {
                Label lblEmotion = new Label()
                {
                    Text = person.Emotion,
                    TextColor = Color.Blue,
                    WidthRequest = 90
                };

                BoxView box = new BoxView()
                {
                    Color = Color.Lime,
                    WidthRequest = 150 * person.Score,
                    HeightRequest = 30,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };

                Label lblPercentage = new Label()
                {
                    Text = person.Score.ToString("P4"),
                    TextColor = Color.Maroon
                };

                StackLayout panel = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal
                };

                panel.Children.Add(lblEmotion);
                panel.Children.Add(box);
                panel.Children.Add(lblPercentage);
                ResultsPanel.Children.Add(panel);
            }
        }
    }
}
