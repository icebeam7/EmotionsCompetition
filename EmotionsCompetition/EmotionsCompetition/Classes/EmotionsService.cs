using Microsoft.ProjectOxford.Emotion;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.ProjectOxford.Common;

namespace EmotionsCompetition.Classes
{
    public static class EmotionsService
    {
        static string key = "6d32785694e1458ebde30e58df480dae";

        public static async Task<List<Person>> Analyze(Stream stream, int index)
        {
            EmotionServiceClient client = new EmotionServiceClient(key);
            var persons = await client.RecognizeAsync(stream);

            if (persons == null || persons.Count() == 0)
                return null;

            Rectangle face = null;
            string emotion = "";

            List<Person> people = new List<Person>();

            foreach (var person in persons)
            {
                float score = 0;
                var currentEmotions = person.Scores;

                switch (index)
                {
                    case 0: emotion = "Happiness";  score = currentEmotions.Happiness;   break;
                    case 1: emotion = "Sadness";    score = currentEmotions.Sadness;     break;
                    case 2: emotion = "Surprise";   score = currentEmotions.Surprise;    break;
                    case 3: emotion = "Neutral";    score = currentEmotions.Neutral;     break;
                    case 4: emotion = "Anger";      score = currentEmotions.Anger;       break;
                    case 5: emotion = "Contempt";   score = currentEmotions.Contempt;    break;
                    case 6: emotion = "Disgust";    score = currentEmotions.Disgust;     break;
                    case 7: emotion = "Fear";       score = currentEmotions.Fear;        break;
                }

                people.Add(new Person(){
                        Face = face,
                        Score = score,
                        Emotion = emotion
                    });
            }

            return people;
        }
    }
}