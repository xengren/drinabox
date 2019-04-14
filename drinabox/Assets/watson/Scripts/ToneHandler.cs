/**
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

// This class has been adapted from the Rainbow Octopus code: https://github.com/ibmets/rainbow-octopus
// Changes have been made to make it work with a more update version of the IBM Watson SDK


using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Utilities;

using IBM.Watson.ToneAnalyzer.V3;
using IBM.Watson.ToneAnalyzer.V3.Model;

public class ToneHandler : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Space(10)]
    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string _iamApikey;
    [Tooltip("The IAM url used to authenticate the apikey (optional). This defaults to \"https://iam.bluemix.net/identity/token\".")]
    [SerializeField]
    private string _iamUrl;
    [Tooltip("Speech Manager")]
    [SerializeField]
    private SpeechManager speechManager;
    #endregion

    private ToneAnalyzerService _service;
    private bool _analyzeToneTested = false;
    private string inputText;

    void Start()
    {
        LogSystem.InstallDefaultReactors();

        Runnable.Run(CreateService());
    }

    // Derived from the Rainbow Octopus.
    private IEnumerator CreateService()
    {
        Credentials credentials = null;

        if (!string.IsNullOrEmpty(_iamApikey))
        {
            //  Authenticate using iamApikey
            TokenOptions tokenOptions = new TokenOptions()
            {
                IamApiKey = _iamApikey,
                IamUrl = _iamUrl
            };

            credentials = new Credentials(tokenOptions, null);

            while (!credentials.HasIamTokenData())
                yield return null;
        }
        else
        {
            throw new IBMException("Please provide either username and password or IAM apikey to authenticate the service.");
        }

        _service = new ToneAnalyzerService("2017-05-26", credentials);
        _service.VersionDate = "2017-05-26";
    }

    // Derived from the Rainbow Octopus.
    public void GetTone(string text)
    {
        Log.Debug("ToneHandler.GetTone()", "{0}", text);

        inputText = text;

        Runnable.Run(AnalyzeTone());
    }

    // Derived from the Rainbow Octopus.
    private IEnumerator AnalyzeTone()
    {
        List<string> tones = new List<string>()
            {
                "emotion",
                "language",
                "social"
            };

        ToneInput toneInput = new ToneInput()
        {
            Text = inputText
        };

        if (!_service.Tone(callback: OnGetToneAnalyze, toneInput: toneInput, sentences: true, tones: tones, contentLanguage: "en", acceptLanguage: "en", contentType: "application/json"))
            Log.Debug("ToneHandler.GetTone()", "Failed to analyze!");

        while (!_analyzeToneTested)
            yield return null;

        _analyzeToneTested = false;
    }

    private void OnGetToneAnalyze(DetailedResponse<ToneAnalysis> response, IBMError error)
    {
        ToneAnalysis toneAnalysis = JsonConvert.DeserializeObject<ToneAnalysis>(response.Response);

        if(speechManager)
        {
            speechManager.ProcessTone(toneAnalysis, inputText);
        }

        _analyzeToneTested = true;
    }
}
