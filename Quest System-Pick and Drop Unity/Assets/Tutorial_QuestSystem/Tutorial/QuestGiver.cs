using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public SO_Quest quest; // ตัวแปรเก็บข้อมูลเควสที่ต้องการให้ผู้เล่นรับ

    private void OnTriggerEnter(Collider other) // ฟังก์ชันนี้จะถูกเรียกเมื่อมีการชนกับ Collider อื่น
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าตัวที่ชนมีแท็กเป็น "Player" หรือไม่
        {
            other.GetComponent<QuestManager>().AcceptQuest(quest); // ถ้าใช่ ให้เรียกฟังก์ชัน AcceptQuest() ใน QuestManager เพื่อรับเควส
        }
    }
}
