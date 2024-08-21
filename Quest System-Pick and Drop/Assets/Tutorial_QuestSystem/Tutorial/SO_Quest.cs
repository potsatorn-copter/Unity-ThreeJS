using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest system / Create new quest")]
public class SO_Quest : ScriptableObject
{
    [Header("Quest data")]
    public string questName; // ชื่อเควส
    public string questDescription; // คำอธิบายเควส
    public Objective[] objectives; // วัตถุประสงค์ของเควส

    [Header("Reward")]
    public int goldReward; // รางวัลเป็นทอง
    public int expReward; // รางวัลเป็นประสบการณ์
    public List<SO_ItemData> itemReward; // รางวัลเป็นไอเทม
}
