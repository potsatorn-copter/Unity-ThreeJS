using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;
    public List<ItemObject> ItemObjects;
    public float maxRadius = 10.0f;
    public float minRadius = 2.0f;

    public Transform itemPickerTf;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        
            Destroy(gameObject);
    }

    public void SpawnItem(SO_ITEM item, int amount)
    {
        if (item.gamePrefab == null)
        {
            Debug.LogError("No prefab in" + item.name + "");
            return;
        }

        Vector2 randPos = Random.insideUnitCircle.normalized * minRadius;
        Vector3 offset = new Vector3(randPos.x, 0, randPos.y);
        GameObject spawnItem = Instantiate(item.gamePrefab, itemPickerTf.position + offset, Quaternion.identity);
        
        spawnItem.GetComponent<ItemObject>().SetAmount(amount);
    }
    public void SpawnItemByGUI(int SpawnAmount = 1)
    {
        for (int i = 0; i < SpawnAmount; i++)
        {
            int index = Random.Range(0, ItemObjects.Count);
            float distance = Random.Range(minRadius, maxRadius);
            Vector2 RandPoint = Random.insideUnitCircle.normalized * distance;
            Vector3 offset = new Vector3(RandPoint.x, 0, RandPoint.y);
            ItemObject itemObjectSpawnItem = Instantiate(ItemObjects[index], itemPickerTf.position + offset, Quaternion.identity);
            itemObjectSpawnItem.RandomAmout();
        }
    }
    void OnDrawGizmos() 
    {
        if (itemPickerTf == null) return; // Check for null

        Gizmos.color = Color.yellow; // Set Gizmo color
        // Draw the outer radius circle
        Gizmos.DrawWireSphere(itemPickerTf.position, maxRadius);  
        // Draw the inner radius circle
        Gizmos.DrawWireSphere(itemPickerTf.position, minRadius); 
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Spawn a Random Item"))
        {
            SpawnItemByGUI();
        }
        if (GUILayout.Button("Spawn 10 Random Item"))
        {
            SpawnItemByGUI();
        }
        if (GUILayout.Button("Spawn 50 Random Item"))
        {
            SpawnItemByGUI();
        }
    }
}
