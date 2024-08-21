using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGO : MonoBehaviour
{
    public SO_ItemData itemData; // ตัวแปรเก็บข้อมูลไอเทม

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าตัวที่ชนมีแท็กเป็น "Player" หรือไม่
        {
            other.GetComponent<QuestManager>().UpdateQuestProgress(ObjectiveType.Gather, itemData); // ถ้าใช่ ให้เรียกฟังก์ชัน UpdateQuestProgress ใน QuestManager เพื่ออัพเดตความคืบหน้าเควส
            gameObject.SetActive(false); // ซ่อน GameObject นี้
        }
    }
}
