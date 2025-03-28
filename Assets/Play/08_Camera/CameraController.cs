using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private readonly float offsetX = 5.0f, offsetY = 8.0f, offsetZ = -5.0f;

    private void LateUpdate()
    {
        Vector3 cameraPos = new Vector3(ChampionController.Champion.position.x + offsetX, ChampionController.Champion.position.y + offsetY, ChampionController.Champion.position.z + offsetZ);
        ChampionController.quarterViewCam.transform.position = cameraPos;
    }
}
