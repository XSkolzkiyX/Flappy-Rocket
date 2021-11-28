using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScript : MonoBehaviour
{
    public bool isSpace;
    private void Start()
    {
        if (isSpace != false)
            transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
    }
    void Update()
    {
        if (transform.position.x < -36)
        {
            transform.position = new Vector2(transform.position.x + 72, transform.position.y);
            if (isSpace != false)
                transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
        }
    }
}
