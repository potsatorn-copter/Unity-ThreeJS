using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowPlayer : MonoBehaviour
{
    public new Transform camera;
    public Transform player;
    public int offset = -1;

    [FormerlySerializedAs("_xClamp")] public float[] xClamp;
    [FormerlySerializedAs("_yClamp")] public float[] yClamp;

    
    private void Update()
    {
        if (player != null && camera != null)
        {
            var position = player.position;

            float xClamp = Mathf.Clamp(position.x, this.xClamp[0], this.xClamp[1]);

            float yClamp = Mathf.Clamp(position.y, this.yClamp[0], this.yClamp[1]);

            camera.position = new Vector3(xClamp, yClamp, offset);
        }
    }
}
