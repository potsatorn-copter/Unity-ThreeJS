using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance; // อินสแตนซ์ของ QuestManager
    public QuestTracker[] OngoingQuest; // เควสที่กำลังทำอยู่
    public QuestUI[] OngoingQuestUI; // UI ของเควสที่กำลังทำอยู่

    private void Awake()
    {
        if (instance == null) // ถ้ายังไม่มี instance ของ QuestManager
        {
            instance = this; // กำหนดให้ instance เป็นตัวนี้
        }
        else
        {
            Destroy(gameObject); // ถ้ามี instance อยู่แล้ว ให้ทำลายตัวนี้ทิ้ง
        }

        OngoingQuest = new QuestTracker[OngoingQuestUI.Length]; // กำหนดขนาดของ OngoingQuest ให้เท่ากับ OngoingQuestUI
    }

    public void AcceptQuest(SO_Quest quest)
    {
        foreach (QuestTracker tracker in OngoingQuest) // ตรวจสอบเควสที่กำลังทำอยู่
        {
            if (tracker.trackedQuest == null)
                continue;

            if (tracker.trackedQuest == quest) // ถ้าเควสนี้ถูกยอมรับแล้ว
            {
                Debug.Log("You already accepted this quest."); // แสดงข้อความว่าเควสนี้ถูกยอมรับแล้ว
                return; // ออกจากฟังก์ชัน
            }
        }

        bool isFull = true; // ตรวจสอบว่าสามารถรับเควสเพิ่มได้หรือไม่
        for (int i = 0; i < OngoingQuest.Length; i++)
        {
            if (OngoingQuest[i].trackedQuest == null) // ถ้ามีช่องว่างใน OngoingQuest
            {
                OngoingQuest[i] = new QuestTracker(quest); // เพิ่มเควสใหม่
                isFull = false; // ตั้งค่า isFull เป็น false
                OngoingQuestUI[i].SetValue(OngoingQuest[i], i); // อัพเดต UI
                break; // ออกจากลูป
            }
        }

        if (isFull) // ถ้าไม่มีช่องว่างใน OngoingQuest
        {
            Debug.Log("Cannot accept more quest."); // แสดงข้อความว่าไม่สามารถรับเควสเพิ่มได้
        }
        else
        {
            Debug.Log("The quest \"" + quest.questName + "\" has been accepted."); // แสดงข้อความว่าเควสนี้ถูกยอมรับแล้ว
        }
    }

    public void CancelQuest(int index)
    {
        if (OngoingQuest[index].trackedQuest != null) // ถ้าเควสในตำแหน่งนี้ไม่ใช่ null
        {
            Debug.Log("The quest \"" + OngoingQuest[index].questName + "\" has been canceled."); // แสดงข้อความว่าเควสนี้ถูกยกเลิกแล้ว
            OngoingQuest[index].trackedQuest = null; // ตั้งค่าเควสในตำแหน่งนี้ให้เป็น null
            OngoingQuestUI[index].gameObject.SetActive(false); // ซ่อน UI ของเควสนี้
        }
    }

    public void CompleteQuest(int index)
    {
        if (OngoingQuest[index].trackedQuest != null) // ถ้าเควสในตำแหน่งนี้ไม่ใช่ null
        {
            string itemList = ""; // สร้างตัวแปรสำหรับเก็บรายการไอเทม
            if (OngoingQuest[index].trackedQuest.itemReward.Count > 0) // ถ้ามีรางวัลเป็นไอเทม
            {
                foreach (SO_ItemData item in OngoingQuest[index].trackedQuest.itemReward) // วนลูปรางวัลไอเทม
                {
                    if (item == null)
                        continue;

                    itemList = "Item reward : " + item.itemName + "\n"; // เพิ่มชื่อไอเทมในรายการ
                }
            }

            Debug.Log
            (
                "The quest \"" + OngoingQuest[index].questName + "\" has been completed." + "\n" +
                "Gold reward : " + OngoingQuest[index].trackedQuest.goldReward + "\n" +
                "Exp reward : " + OngoingQuest[index].trackedQuest.expReward + "\n" +
                itemList
            ); // แสดงข้อความว่าเควสนี้เสร็จสมบูรณ์และแสดงรางวัล

            OngoingQuest[index].trackedQuest = null; // ตั้งค่าเควสในตำแหน่งนี้ให้เป็น null
            OngoingQuestUI[index].gameObject.SetActive(false); // ซ่อน UI ของเควสนี้
        }
    }

    public void UpdateQuestProgress(ObjectiveType type, ScriptableObject targetData)
    {
        if (OngoingQuest == null) // ถ้า OngoingQuest เป็น null
            return;

        for (int i = 0; i < OngoingQuest.Length; i++) // วนลูปผ่าน OngoingQuest
        {
            if (OngoingQuest[i].trackedQuest != null) // ถ้าเควสในตำแหน่งนี้ไม่ใช่ null
            {
                OngoingQuest[i].UpdateProgress(type, targetData); // อัพเดตความคืบหน้าของเควส
                OngoingQuestUI[i].UpdateProgress(OngoingQuest[i].questCanComplete); // อัพเดต UI
            }
        }
    }
}