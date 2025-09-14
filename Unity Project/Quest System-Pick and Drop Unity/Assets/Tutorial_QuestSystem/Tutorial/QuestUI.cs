using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public int questIndex; // ตัวแปรเก็บตำแหน่งของเควสใน OngoingQuest

    public TextMeshProUGUI title; // ตัวแปรเก็บ UI สำหรับแสดงชื่อเควส
    public TextMeshProUGUI description; // ตัวแปรเก็บ UI สำหรับแสดงคำอธิบายเควส
    public Button completeBTN; // ตัวแปรเก็บปุ่มสำหรับทำเควสให้สำเร็จ

    public void SetValue(QuestTracker tracker, int index)
    {
        gameObject.SetActive(true); // แสดง UI ของเควส

        questIndex = index; // ตั้งค่า questIndex เป็น index

        title.text = tracker.questName; // ตั้งค่า title เป็นชื่อเควส
        description.text = tracker.questDescription; // ตั้งค่า description เป็นคำอธิบายเควส

        completeBTN.interactable = tracker.questCanComplete; // ตั้งค่าการคลิกปุ่ม completeBTN ตามสถานะ questCanComplete
    }

    public void UpdateProgress(bool canComplete)
    {
        completeBTN.interactable = canComplete; // ตั้งค่าการคลิกปุ่ม completeBTN ตามค่า canComplete
    }

    public void CompleteQuest()
    {
        QuestManager.instance.CompleteQuest(questIndex); // เรียกฟังก์ชัน CompleteQuest ใน QuestManager
        gameObject.SetActive(false); // ซ่อน UI ของเควส
    }

    public void CancelQuest()
    {
        QuestManager.instance.CancelQuest(questIndex); // เรียกฟังก์ชัน CancelQuest ใน QuestManager
        gameObject.SetActive(false); // ซ่อน UI ของเควส
    }
}