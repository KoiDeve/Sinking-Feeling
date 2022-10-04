using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class used to zoom in and out depending on a certain boundary set for the player. See <CameraController> for more.
public class ZoomInfluence : MonoBehaviour {

    private CameraController theCamera;

    void Start() {
        theCamera = FindObjectOfType<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        theCamera.ZoomOut();
    }

    private void OnTriggerExit2D(Collider2D collision) 
        theCamera.ZoomIn();
    }

}
