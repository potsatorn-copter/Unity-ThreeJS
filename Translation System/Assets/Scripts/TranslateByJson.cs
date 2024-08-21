using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;

public class TranslateByJson : MonoBehaviour
{
    [SerializeField] private TMP_InputField commentText; // อินพุตสำหรับรับข้อความจากผู้ใช้
    [SerializeField] private GameObject seeTranslationButton; // ปุ่มสำหรับแสดงข้อความแปล
    [SerializeField] private GameObject seeOriginalButton; // ปุ่มสำหรับแสดงข้อความต้นฉบับ

    private Dictionary<string, string> translationsEnToTh = new Dictionary<string, string>(); // พจนานุกรมสำหรับแปลจากภาษาอังกฤษเป็นภาษาไทย
    private Dictionary<string, string> translationsThToEn = new Dictionary<string, string>(); // พจนานุกรมสำหรับแปลจากภาษาไทยเป็นภาษาอังกฤษ
    private string currentLanguage = "th"; // ภาษาปัจจุบัน (ค่าเริ่มต้นคือภาษาไทย)
    private string originalComment; // เก็บข้อความต้นฉบับ
    private string translatedComment; // เก็บข้อความที่แปลแล้ว

    
    void Start()
    {
        seeOriginalButton.SetActive(false); // ซ่อนปุ่มแสดงข้อความต้นฉบับ
        LoadTranslations(); // โหลดการแปลจากไฟล์ JSON
        originalComment = commentText.text; // เก็บข้อความต้นฉบับ
    }

    // ฟังก์ชันที่ถูกเรียกเมื่อกดปุ่มแสดงข้อความแปล
    public void OnSeeTranslationButtonClick()
    {
        originalComment = commentText.text; // เก็บข้อความต้นฉบับ
        TranslateComment(originalComment); // แปลข้อความต้นฉบับ
        ShowTranslatedComment(); // แสดงข้อความแปล
    }

    // ฟังก์ชันที่ถูกเรียกเมื่อผู้ใช้กดปุ่มแสดงข้อความต้นฉบับ
    public void OnSeeOriginalButtonClick()
    {
        ShowOriginalComment(); // แสดงข้อความต้นฉบับ
    }

    // ฟังก์ชันสำหรับโหลดการแปลจากไฟล์ JSON
    private void LoadTranslations()
    {
        LoadLanguageFile("en.json", translationsEnToTh, translationsThToEn); // โหลดการแปลจาก en.json
        LoadLanguageFile("th.json", translationsThToEn, translationsEnToTh); // โหลดการแปลจาก th.json
    }

    // ฟังก์ชันสำหรับอ่านไฟล์ JSON และเก็บข้อมูลในพจนานุกรม
    private void LoadLanguageFile(string fileName, Dictionary<string, string> fromLang, Dictionary<string, string> toLang)
    {
        string path = Path.Combine(Application.dataPath, fileName); // กำหนดพาธของไฟล์ JSON
        if (File.Exists(path)) // ตรวจสอบว่าไฟล์มีอยู่หรือไม่
        {
            try
            {
                string jsonData = File.ReadAllText(path); // อ่านข้อมูลจากไฟล์ JSON
                var data = JsonConvert.DeserializeObject<LanguageData>(jsonData); // แปลงข้อมูล JSON เป็นออบเจกต์
                foreach (var kvp in data.languages[0]) // วนลูปผ่านคู่ข้อมูลใน JSON
                {
                    fromLang[kvp.Key.ToLower()] = CapitalizeFirstLetter(kvp.Value); // เก็บข้อมูลในพจนานุกรม โดยทำให้ตัวอักษรแรกเป็นตัวพิมพ์ใหญ่
                    toLang[kvp.Value.ToLower()] = CapitalizeFirstLetter(kvp.Key); // เก็บข้อมูลในพจนานุกรม โดยทำให้ตัวอักษรแรกเป็นตัวพิมพ์ใหญ่
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error reading {fileName} translation file: " + e.Message); // แสดงข้อผิดพลาดเมื่อไม่สามารถอ่านไฟล์ได้
            }
        }
        else
        {
            Debug.LogError($"Translation file not found at path: {path}"); // แสดงข้อผิดพลาดเมื่อไม่พบไฟล์
        }
    }

    // ฟังก์ชันสำหรับแปลข้อความ
    private void TranslateComment(string textToTranslate)
    {
        string lowerCaseText = textToTranslate.ToLower(); // แปลงข้อความที่จะแปลเป็นตัวพิมพ์เล็กทั้งหมด

        if (currentLanguage == "th") // ตรวจสอบว่าภาษาปัจจุบันคือภาษาไทยหรือไม่
        {
            if (translationsThToEn.ContainsKey(lowerCaseText)) // ตรวจสอบว่ามีคำแปลในพจนานุกรมหรือไม่
            {
                translatedComment = translationsThToEn[lowerCaseText]; // ดึงคำแปลจากพจนานุกรม
            }
            else if (translationsEnToTh.ContainsKey(lowerCaseText))
            {
                translatedComment = translationsEnToTh[lowerCaseText]; // ดึงคำแปลจากพจนานุกรม
            }
            else
            {
                translatedComment = "Translation not found for: " + lowerCaseText; // แสดงข้อความเมื่อไม่พบคำแปล
            }
        }
        else if (currentLanguage == "en") // ตรวจสอบว่าภาษาปัจจุบันคือภาษาอังกฤษหรือไม่
        {
            if (translationsEnToTh.ContainsKey(lowerCaseText)) // ตรวจสอบว่ามีคำแปลในพจนานุกรมหรือไม่
            {
                translatedComment = translationsEnToTh[lowerCaseText]; // ดึงคำแปลจากพจนานุกรม
            }
            else if (translationsThToEn.ContainsKey(lowerCaseText))
            {
                translatedComment = translationsThToEn[lowerCaseText]; // ดึงคำแปลจากพจนานุกรม
            }
            else
            {
                translatedComment = "Translation not found for: " + lowerCaseText; // แสดงข้อความเมื่อไม่พบคำแปล
            }
        }
    }

    // ฟังก์ชันสำหรับแสดงข้อความแปล
    private void ShowTranslatedComment()
    {
        commentText.text = translatedComment; // แสดงข้อความแปลในอินพุต
        seeOriginalButton.SetActive(true); // แสดงปุ่มสำหรับดูข้อความต้นฉบับ
        seeTranslationButton.SetActive(false); // ซ่อนปุ่มสำหรับดูข้อความแปล
    }

    // ฟังก์ชันสำหรับแสดงข้อความต้นฉบับ
    private void ShowOriginalComment()
    {
        commentText.text = originalComment; // แสดงข้อความต้นฉบับในอินพุต
        seeOriginalButton.SetActive(false); // ซ่อนปุ่มสำหรับดูข้อความต้นฉบับ
        seeTranslationButton.SetActive(true); // แสดงปุ่มสำหรับดูข้อความแปล
    }

    // ฟังก์ชันสำหรับเปลี่ยนภาษาปัจจุบัน
    public void SetCurrentLanguage(string language)
    {
        currentLanguage = language; // เปลี่ยนค่าภาษาปัจจุบัน
        LoadTranslations(); // โหลดการแปลใหม่ตามภาษาที่เปลี่ยน
    }

    // ฟังก์ชันสำหรับทำให้ตัวอักษรแรกเป็นตัวพิมพ์ใหญ่
    private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input; // ถ้าข้อความว่างหรือเป็น null ให้คืนค่าข้อความเดิม
        }

        return char.ToUpper(input[0]) + input.Substring(1); // ทำให้ตัวอักษรแรกเป็นตัวพิมพ์ใหญ่และคืนค่าข้อความที่ถูกปรับ
    }
}

[Serializable]
public class LanguageData
{
    public List<Dictionary<string, string>> languages; // ประกาศคลาสสำหรับเก็บข้อมูลการแปลจากไฟล์ JSON
}