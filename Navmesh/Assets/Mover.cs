using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    private NavMeshAgent[] _agents;

    public Transform Marker;

    public float verticalOffset = 10f;
    // Start is called before the first frame update
    void Start()
    {
        _agents = FindObjectsOfType(typeof(NavMeshAgent)) as NavMeshAgent[];
    }

    void UpdateTarget(Vector3 targetPosition)
    {
        foreach (var agent in _agents)
        {
            agent.destination = targetPosition;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out var hitInfo))
            {
                var targetPosition = hitInfo.point;
                UpdateTarget(targetPosition);
                if (Marker != null)
                {
                    Marker.position = targetPosition + new Vector3(0, verticalOffset, 0);
                }
            }
        }
    }
}
