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

using System;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

using UnityEngine;
using UnityEngine.UI;

using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Utilities;

using IBM.Watson.Assistant.V2;
using IBM.Watson.Assistant.V2.Model;

public class AssistantHandler : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Space(10)]
    [Tooltip("The version date with which you would like to use the service in the form YYYY-MM-DD.")]
    [SerializeField]
    private string _versionDate = "2017-05-25";
    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string _iamApikey;
    [Tooltip("The IAM url used to authenticate the apikey (optional). This defaults to \"https://iam.bluemix.net/identity/token\".")]
    [SerializeField]
    private string _iamUrl;
    [Tooltip("The assistantId to run the example.")]
    [SerializeField]
    private string assistantId;
    [Tooltip("Speech Manager")]
    [SerializeField]
    private SpeechManager speechManager;
    #endregion

    private static AssistantService _service;

    private Dictionary<string, object> _context = null;

    private string sessionId;

    private bool createSessionTested = false;

    private string inputText;

    void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());;
    }

    // Derived from the Rainbow Octopus.
    private IEnumerator CreateService()
    {
        //  Create credential and instantiate service
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

            //  Wait for tokendata
            while (!credentials.HasIamTokenData())
                yield return null;
        }
        else
        {
            throw new IBMException("Please provide either username and password or IAM apikey to authenticate the service.");
        }

        _service = new AssistantService("2019-04-19", credentials);
        _service.VersionDate = _versionDate;

        Runnable.Run(CreateSession());
    }

    // Derived from the Rainbow Octopus.
    private IEnumerator CreateSession()
    {
        Log.Debug("AssistantHandler.RunTest()", "Attempting to CreateSession");

        _service.CreateSession(OnCreateSession, assistantId);

        while (!createSessionTested)
        {
            yield return null;
        }
    }

    // Derived from the Rainbow Octopus.
    public void GetIntent(string text)
    {
        inputText = text;

        var input1 = new MessageInput()
        {
            Text = text,
            Options = new MessageInputOptions()
            {
                ReturnContext = true
            }
        };

        if (!_service.Message(OnMessage, assistantId, sessionId, input: input1))
            Log.Debug("ConversationHandler.AskQuestion()", "Failed to message!");
    }

    // Derived from the Rainbow Octopus.
    private void OnMessage(DetailedResponse<MessageResponse> response, IBMError error)
    {
        MessageResponse messageResponse = JsonConvert.DeserializeObject<MessageResponse>(response.Response);

        if(messageResponse == null)
        {
            return;
        }

        Log.Debug("AssistantHandler.OnMessage()", "response: {0}", messageResponse);

        List<RuntimeIntent> runtimeIntents = messageResponse.Output.Intents;

        if (runtimeIntents != null && runtimeIntents.Count > 0)
        {

            string bestIntent = runtimeIntents[0].Intent;

            Log.Debug("ConversationHandler.OnMessage()", "Intent: {0}", bestIntent);

            speechManager.ProcessIntent(bestIntent, inputText);
        }
        else
        {
            Log.Debug("intents", "No intents");
        }
    }

    private void OnCreateSession(DetailedResponse<SessionResponse> response, IBMError error)
    {
        Log.Debug("ConversationHandler.OnCreateSession()", "Session: {0}", response.Result.SessionId);
        sessionId = response.Result.SessionId;
        createSessionTested = true;
    }
}
