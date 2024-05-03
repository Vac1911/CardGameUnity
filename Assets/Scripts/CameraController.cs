using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static TMPro.Examples.CameraController;
using static UnityEditor.SceneView;
using static CardGameControls;
using UnityEngine.InputSystem;
using System;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CameraController : MonoBehaviour, IPlayerActions
{
    public GameObject encounter;

    CardGameControls controls;

    /*[Range(4f, 16f)]
    public float cameraSensitivity = 8f;*/

    // Note: higher number is less sensitive
    [MinAttribute(1f)]
    public float zoomSensitivity = 120f;

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
        if (!context.performed) return;

        positionDelta = context.ReadValue<Vector2>();
        print("move " + positionDelta.ToString());
        var nextPosition = encounter.transform.position;
        nextPosition.x -= positionDelta.x;
        nextPosition.y -= positionDelta.y;
        encounter.transform.position = nextPosition;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        zoomDelta = context.ReadValue<Vector2>().y / zoomSensitivity;
        float prevScale = encounter.transform.localScale.y;
        float nextScale = Math.Max(prevScale + zoomDelta, 1f);
        encounter.transform.localScale = Vector3.one * nextScale;


        var nextPosition = encounter.transform.position / prevScale * nextScale;
        encounter.transform.position = nextPosition;

        print("zoom " + zoomDelta.ToString());
    }
}
