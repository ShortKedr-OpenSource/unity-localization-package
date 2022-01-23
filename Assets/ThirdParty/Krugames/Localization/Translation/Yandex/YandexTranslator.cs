using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Krugames.LocalizationSystem.Translation.YandexCloud {
    /// <summary>
    /// Translator based on YandexCloud
    /// </summary>
    public class YandexTranslator : TextTranslator {
        
        public static readonly string ServiceUrl = "https://translate.api.cloud.yandex.net/translate/v2/translate";
        public static readonly string RequestMethod = UnityWebRequest.kHttpVerbPOST;
        
        public override void Translate(string textToTranslate, SystemLanguage @from, SystemLanguage to,
            TranslationSuccessDelegate successCallback, TranslationFailDelegate failCallback) {
            
            UnityWebRequest request= new UnityWebRequest(ServiceUrl, RequestMethod);

            string param = JsonUtility.ToJson("");
            byte[] bytes = Encoding.ASCII.GetBytes(param);

            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(bytes) {
                contentType = "application/json",
            };
            request.uploadHandler= uploadHandler;
            
            DownloadHandler downloadHandler = new DownloadHandlerBuffer();
            request.downloadHandler= downloadHandler;

            request.SetRequestHeader("Authorization", $"Bearer {YandexSettings.TemporaryIamToken}");
            
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();

            void OnRequestComplete(AsyncOperation asyncOperation) {
                Debug.Log(request.result);
                Debug.Log(Encoding.ASCII.GetString(request.downloadHandler.data));
            }
            
            operation.completed += OnRequestComplete;
            
        }
        
    }
}