using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Krugames.LocalizationSystem.Translation.LibreTranslate {
    /// <summary>
    /// Text Translator based on free https://libretranslate.com/
    /// </summary>
    public class LibreTextTranslator : TextTranslator {

        private class LibreTextRequestBody {
            public string q;
            public string source;
            public string target;
            public string format = "text";

            public LibreTextRequestBody(string q, string source, string target) {
                this.q = q;
                this.source = source;
                this.target = target;
            }
        }
        
        public override void Translate(string textToTranslate, SystemLanguage @from, SystemLanguage to,
            TranslationSuccessDelegate successCallback, TranslationFailDelegate failCallback) {
            
            UnityWebRequest request= new UnityWebRequest("https://libretranslate.com/translate", UnityWebRequest.kHttpVerbPOST);

            string param = JsonUtility.ToJson(new LibreTextRequestBody("Hello", "en", "ru"));
            byte[] bytes = Encoding.ASCII.GetBytes(param);

            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(bytes) {contentType = "application/json"};
            request.uploadHandler= uploadHandler;
            
            DownloadHandler downloadHandler = new DownloadHandlerBuffer();
            request.downloadHandler= downloadHandler;

            UnityWebRequestAsyncOperation operation = request.SendWebRequest();

            void OnRequestComplete(AsyncOperation asyncOperation) {
                    Debug.Log(request.result);
                    Debug.Log(Encoding.ASCII.GetString(request.downloadHandler.data));
            }
            
            operation.completed += OnRequestComplete;
        }
    }
}