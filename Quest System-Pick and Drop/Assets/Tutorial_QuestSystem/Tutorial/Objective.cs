using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective
{
    public ObjectiveType type; // ประเภทของวัตถุประสงค์
    public ScriptableObject targetDetail; // รายละเอียดของเป้าหมาย
    public int requiredAmount = 1; // จำนวนที่ต้องการ
    public int currentAmount = 0; // จำนวนปัจจุบัน
    public bool isCompleted = false; // สถานะเสร็จสมบูรณ์หรือไม่

    public Objective(Objective objective)
    {
        type = objective.type; // ตั้งค่าประเภทของวัตถุประสงค์
        targetDetail = objective.targetDetail; // ตั้งค่ารายละเอียดของเป้าหมาย
        requiredAmount = objective.requiredAmount; // ตั้งค่าจำนวนที่ต้องการ
        currentAmount = objective.currentAmount; // ตั้งค่าจำนวนปัจจุบัน
        isCompleted = objective.isCompleted; // ตั้งค่าสถานะเสร็จสมบูรณ์
    }
}

public enum ObjectiveType
{
    Hunt, // ประเภทของวัตถุประสงค์การล่า
    Gather, // ประเภทของวัตถุประสงค์การเก็บรวบรวม
    Deliver // ประเภทของวัตถุประสงค์การส่งของ
}
