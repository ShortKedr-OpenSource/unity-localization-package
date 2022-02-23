using System;
using System.Collections.Generic;
using System.Net.Http;
using Krugames.LocalizationSystem.Translation.LanguageEncoding;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Krugames.LocalizationSystem.Translation.RapidApi {
    /// <summary>
    /// TextTranslator based on RapidAPI Google Translate https://rapidapi.com/googlecloud/api/google-translate1/
    /// </summary>
    public class RapidApiGoogleTranslator : StringTranslator {

        public override void Translate(string textToTranslate, SystemLanguage from, SystemLanguage to,
            TranslationSuccessDelegate successCallback, TranslationFailDelegate failCallback) {

            if (from == to) {
                successCallback?.Invoke(textToTranslate, to, from);
                return;
            }
            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://google-translate1.p.rapidapi.com/language/translate/v2"),
                Headers =
                {
                    { "x-rapidapi-host", "google-translate1.p.rapidapi.com" },
                    { "x-rapidapi-key", "26ed99c731msh61deb61d64acd75p187ff1jsn71701035c4b3" },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "q", textToTranslate },
                    { "target", SystemLanguageEncoding.Iso639_1.GetCode(to) },
                    { "source", SystemLanguageEncoding.Iso639_1.GetCode(from) },
                }),
            };
            
            async void SendRequest() {
                using (var response = await client.SendAsync(request))
                {
                    try {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();
                        
                        JObject result = JObject.Parse(body);
                        JToken data = result["data"];
                        JArray translations = (JArray) data?["translations"];
                        
                        if (translations != null && translations.Count > 0) {
                            JToken translation = translations[0];
                            JToken translatedTextToken = translation?["translatedText"];
                            successCallback?.Invoke(translatedTextToken?.Value<string>(), to, from);
                        } else {
                            failCallback?.Invoke(textToTranslate, from, to);
                        }

                    } catch (Exception e) {
                        Debug.LogError(e.Message);
                        failCallback?.Invoke(textToTranslate, from, to);
                    }
                }
            }
            
            SendRequest();

        }
        
    }
}