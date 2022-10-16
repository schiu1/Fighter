using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float startPos;
    GameObject camObj;
    [SerializeField] float parallaxEffect = 0;

    // Start is called before the first frame update
    void Start()
    {
        camObj = GameObject.Find("Main Camera");
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (camObj.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
