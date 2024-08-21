using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Translator : MonoBehaviour
{ 
    [SerializeField] private TMP_InputField commentText;  // ช่องข้อความที่จะแสดงผลทั้งต้นฉบับและแปลแล้ว
    [SerializeField] private GameObject seeTranslationButton;  // ปุ่มสำหรับแปลข้อความ
    [SerializeField] private GameObject seeOriginalButton;  // ปุ่มสำหรับกลับไปดูข้อความต้นฉบับ
    private string apiUrl = "http://127.0.0.1:5000/translate";  // URL ของเซิร์ฟเวอร์ API
    private string originalComment;
    private string translatedComment;

    [System.Serializable]
    public class TranslationRequest
    {
        public string text;
    }

    [System.Serializable]
    public class TranslationResponse
    {
        public string translated_text;
    }

    void Start()
    {
        seeOriginalButton.SetActive(false);  // เริ่มต้นให้ปุ่มดูข้อความต้นฉบับซ่อนไว้
        originalComment = commentText.text;  // เก็บข้อความต้นฉบับไว้
    }

    public void OnSeeTranslationButtonClick()
    {
        originalComment = commentText.text;  // อัปเดตข้อความต้นฉบับทุกครั้งที่กดปุ่มแปล
        translatedComment = null;  // รีเซ็ตตัวแปร translatedComment ทุกครั้งที่แปลข้อความใหม่
        StartCoroutine(TranslateComment(originalComment));
    }

    public void OnSeeOriginalButtonClick()
    {
        ShowOriginalComment();
    }

    private IEnumerator TranslateComment(string textToTranslate)
    {
        TranslationRequest requestData = new TranslationRequest();
        requestData.text = textToTranslate;

        var jsonData = JsonUtility.ToJson(requestData);
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var jsonResponse = request.downloadHandler.text;
            var responseData = JsonUtility.FromJson<TranslationResponse>(jsonResponse);
            translatedComment = responseData.translated_text;
            ShowTranslatedComment();
        }
        else
        {
            Debug.LogError("Translation failed: " + request.error);
            translatedComment = "Translation failed";  // แสดงข้อความผิดพลาด
            ShowTranslatedComment();
        }
    }

    private void ShowTranslatedComment()
    {
        commentText.text = translatedComment;  // แสดงข้อความที่แปลแล้ว
        seeOriginalButton.SetActive(true);  // แสดงปุ่มดูข้อความต้นฉบับ
        seeTranslationButton.SetActive(false);  // ซ่อนปุ่มดูข้อความแปล
    }

    private void ShowOriginalComment()
    {
        commentText.text = originalComment;  // แสดงข้อความต้นฉบับ
        seeOriginalButton.SetActive(false);  // ซ่อนปุ่มดูข้อความต้นฉบับ
        seeTranslationButton.SetActive(true);  // แสดงปุ่มดูข้อความแปล
    }
}