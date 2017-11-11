using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.ProjectOxford.Emotion;
namespace Emotions
{
    public class ServiceEmotions
    {
        static string key = "d25b8f132af142a5b85afd2dbc1916b8";
        public static async Task<Dictionary<string, float>> GetEmotions(Stream
       stream)
        {
            EmotionServiceClient cliente = new EmotionServiceClient(key);
            var emotions = await cliente.RecognizeAsync(stream);
            if (emotions == null || emotions.Count() == 0)
                return null;
            return emotions[0].Scores.ToRankedList().ToDictionary(x => x.Key, x =>
           x.Value);
        }
    }
}

