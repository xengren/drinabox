using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using IBM.Cloud.SDK;
using IBM.Watson.ToneAnalyzer.V3.Model;

public class SpeechManager : MonoBehaviour
{
    public ToneHandler toneHandler;
    public AssistantHandler assistantHandler;

    private float emotion_threshold = 0.65f;

    // callback for Tone
    // callback for Text
    // callback for Intention

    public void ProcessSpeech(string rawText)
    {
        Log.Debug("SpeechManager.ProcessSpeech", rawText);

        string trimmedText = rawText.Trim();

        if(toneHandler)
        {
            toneHandler.GetTone(trimmedText);
        }

        if (assistantHandler)
        {
            assistantHandler.GetIntent(trimmedText);
        }
    }

    public void ProcessTone(ToneAnalysis toneAnalysis, string text)
    {
        Log.Debug("SpeechManager.ProcessTone", "{0}", toneAnalysis);

        double? anger = toneAnalysis.DocumentTone.ToneCategories[0].Tones[0].Score;
        double? disgust = toneAnalysis.DocumentTone.ToneCategories[0].Tones[1].Score;
        double? fear = toneAnalysis.DocumentTone.ToneCategories[0].Tones[2].Score;
        double? joy = toneAnalysis.DocumentTone.ToneCategories[0].Tones[3].Score;
        double? sadness = toneAnalysis.DocumentTone.ToneCategories[0].Tones[4].Score;

        var tones = new SortedDictionary<string, double?> {
                                                            { "anger",  anger},
                                                            { "disgust",  disgust},
                                                            { "fear",  fear},
                                                            { "joy",  joy},
                                                            { "sadness",  sadness},
                                                            };

        string max_tone = tones.Aggregate((l, right) => l.Value > right.Value ? l : right).Key;

        if (tones[max_tone] > emotion_threshold)
        {
            Log.Debug("SpeechManager.ProcessTone", "Tone over threshold: {0} {1}", max_tone, tones[max_tone]);
        }
    }

    public void ProcessIntent(string intent, string text)
    {
        Log.Debug("SpeechManager.ProcessIntent", "{0} {1}", intent, text);
    }
}
