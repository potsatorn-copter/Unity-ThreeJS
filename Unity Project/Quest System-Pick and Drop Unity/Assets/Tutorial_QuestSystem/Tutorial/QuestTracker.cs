using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuestTracker
{
    [Header("Quest Data")]
    public SO_Quest trackedQuest; // ตัวแปรเก็บข้อมูลเควสที่กำลังติดตาม

    [Header("Quest Details")]
    public string questName; // ชื่อเควส
    public string questDescription; // คำอธิบายเควส
    public Objective[] objectives; // วัตถุประสงค์ของเควส
    public bool questCanComplete; // สถานะว่าเควสสามารถทำสำเร็จได้หรือไม่

    public QuestTracker(SO_Quest questData)
    {
        trackedQuest = questData; // ตั้งค่า trackedQuest เป็น questData

        questName = questData.questName; // ตั้งค่า questName เป็น questData.questName
        questDescription = questData.questDescription; // ตั้งค่า questDescription เป็น questData.questDescription

        objectives = new Objective[questData.objectives.Length]; // กำหนดขนาดของ objectives ให้เท่ากับ questData.objectives.Length

        for (int i = 0; i < questData.objectives.Length; i++)
        {
            objectives[i] = new Objective(questData.objectives[i]); // คัดลอกข้อมูลวัตถุประสงค์จาก questData มายัง objectives
        }
    }

    public void UpdateProgress(ObjectiveType type, ScriptableObject targetData)
    {
        if (objectives == null) // ถ้า objectives เป็น null
            return;

        foreach (Objective objt in objectives) // วนลูปผ่าน objectives
        {
            if (objt.type == type && objt.targetDetail == targetData) // ถ้าวัตถุประสงค์มีประเภทและรายละเอียดตรงกับที่ระบุ
            {
                objt.currentAmount++; // เพิ่มจำนวนปัจจุบันของวัตถุประสงค์
                if (objt.currentAmount >= objt.requiredAmount) // ถ้าจำนวนปัจจุบันถึงจำนวนที่ต้องการ
                {
                    objt.isCompleted = true; // ตั้งค่าวัตถุประสงค์ว่าเสร็จสมบูรณ์
                    CheckCompleted(); // ตรวจสอบว่าเควสสามารถทำสำเร็จได้หรือไม่
                }
            }
        }
    }

    public void CheckCompleted()
    {
        bool allCompleted = true; // ตัวแปรบอกว่าวัตถุประสงค์ทั้งหมดเสร็จสมบูรณ์แล้วหรือไม่
        foreach (Objective objt in objectives) // วนลูปผ่าน objectives
        {
            if (!objt.isCompleted) // ถ้าวัตถุประสงค์ยังไม่เสร็จสมบูรณ์
            {
                allCompleted = false; // ตั้งค่า allCompleted เป็น false
                break; // ออกจากลูป
            }
        }

        questCanComplete = allCompleted; // ตั้งค่า questCanComplete ตามค่า allCompleted
        Debug.Log(allCompleted ? "Quest can be completed!" : "Cannot complete yet."); // แสดงข้อความว่าเควสสามารถทำสำเร็จได้หรือไม่
    }
}