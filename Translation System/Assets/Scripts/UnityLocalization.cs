using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class UnityLocalization : MonoBehaviour
{
    [SerializeField] private TMP_InputField commentText; // ใช้สำหรับป้อนความคิดเห็นในภาษาแม่
    [SerializeField] private GameObject seeTranslationButton;
    [SerializeField] private GameObject seeOriginalButton;

    private string originalComment;
    private LocalizedString localizedString;

    void Start()
    {
        seeOriginalButton.SetActive(false);
        originalComment = commentText.text;
    }

    public void OnSeeTranslationButtonClick()
    {
        Debug.Log("OnSeeTranslationButtonClick - Original Comment: " + originalComment);
        originalComment = commentText.text;
        TranslateCommentEnglishToThai(originalComment);
    }

    public void OnSeeOriginalButtonClick()
    {
        ShowOriginalComment();
    }

    private void TranslateCommentEnglishToThai(string textToTranslate)
    {
        string lowerCaseText = textToTranslate.ToLowerInvariant();
        Debug.Log("TranslateCommentEnglishToThai - Text to Translate: " + lowerCaseText);

        localizedString = new LocalizedString { TableReference = "Translations", TableEntryReference = lowerCaseText };
        localizedString.StringChanged += UpdateTranslatedText;
        localizedString.RefreshString();
    }

    private void UpdateTranslatedText(string translatedText)
    {
        Debug.Log("UpdateTranslatedText called with: " + translatedText);
        commentText.text = translatedText;
        seeOriginalButton.SetActive(true);
        seeTranslationButton.SetActive(false);
    }

    private void ShowOriginalComment()
    {
        commentText.text = originalComment;
        seeOriginalButton.SetActive(false);
        seeTranslationButton.SetActive(true);
    }
}