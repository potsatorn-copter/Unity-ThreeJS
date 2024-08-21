using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGO : MonoBehaviour
{
    public SO_MonsterData monsterData; // ตัวแปรเก็บข้อมูลมอนสเตอร์

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าตัวที่ชนมีแท็กเป็น "Player" หรือไม่
        {
            other.GetComponent<QuestManager>().UpdateQuestProgress(ObjectiveType.Hunt, monsterData); // ถ้าใช่ ให้เรียกฟังก์ชัน UpdateQuestProgress ใน QuestManager เพื่ออัพเดตความคืบหน้าเควส
            gameObject.SetActive(false); // ซ่อน GameObject นี้
        }
    }
}
