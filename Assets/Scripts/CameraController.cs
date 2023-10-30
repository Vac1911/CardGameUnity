using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static TMPro.Examples.CameraController;
using static UnityEditor.SceneView;
using static CardGameControls;
using UnityEngine.InputSystem;
using System;

public class CameraController : MonoBehaviour, IPlayerActions
{
    public GameObject grid;

    CardGameControls controls;

    [Range(4f, 16f)]
    public float cameraSensitivity = 8f;
    
    private Vector2 positionDelta;
    private float zoomDelta;

    void OnEnable()
    {
        if (controls == null)
        {
            controls = new CardGameControls();
            // Tell the "gameplay" action map that we want to get told about
            // when actions get triggered.
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Update()
    {

        /*grid.transform.position += positionDelta * cameraSensitivity * Time.deltaTime;*/
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        positionDelta = context.ReadValue<Vector2>();
        print("move " + positionDelta.ToString());
        var nextPosition = transform.position;
        nextPosition.x += positionDelta.x;
        transform.position = nextPosition;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        zoomDelta = context.ReadValue<Vector2>().y;
        print("zoom " + zoomDelta.ToString());
    }
}
