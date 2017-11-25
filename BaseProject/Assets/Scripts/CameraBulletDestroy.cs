using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBulletDestroy : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bullet")
        {
            Destroy(col.gameObject);
        }
    }
}
