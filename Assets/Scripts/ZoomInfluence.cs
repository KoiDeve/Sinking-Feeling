using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInfluence : MonoBehaviour
{

    private CameraController theCamera;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        theCamera.ZoomOut();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        theCamera.ZoomIn();
    }

}
