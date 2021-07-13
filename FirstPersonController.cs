using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float MoveSpeed = 2.5f;
    public Camera childCamera = null;
    public float LookSensitivity = 5.0f;    //amount of look per mouse move, higher means look around faster
    public float LookSmooth = 2.0f;         //less jagged look movement
    private Vector2 LookDirection;

    private void Start()
    {
        //note: since we are using the mouse to look, turn off cursor visibility & confine to game window
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        ControlMovement();
        ControlLookAround();

        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            //show the cursor, unlock it from the game window
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void ControlMovement()
    {
        //read input for vertical, horizontal (-1.0 ... 1.0)
        float xAxisMove = Input.GetAxis("Horizontal");
        float zAxisMove = Input.GetAxis("Vertical");

        //move player
        this.transform.Translate(xAxisMove * MoveSpeed * Time.deltaTime, 0.0f, zAxisMove * MoveSpeed * Time.deltaTime);
    }
    private void ControlLookAround()
    {
        //read mouse movement input (-1.0 ... 1.0)
        Vector2 mouseDir = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //multiply the input by sensitivity
        mouseDir = Vector2.Scale(mouseDir, new Vector2(LookSensitivity, LookSensitivity));

        //use Lerp to slowly change the mouseLook direction
        Vector2 LookDelta = new Vector2();
        LookDelta.x = Mathf.Lerp(LookDelta.x, mouseDir.x, 1.0f / LookSmooth);
        LookDelta.y = Mathf.Lerp(LookDelta.y, mouseDir.y, 1.0f / LookSmooth);
        LookDirection += LookDelta;

        //limit the up & down angle
        LookDirection.y = Mathf.Clamp(LookDirection.y, -75.0f, 75.0f);

        //rotate camera up & down
        childCamera.transform.localRotation = Quaternion.AngleAxis(-LookDirection.y, Vector3.right);

        //rotate player (...and therefore the camera) left & right
        this.transform.localRotation = Quaternion.AngleAxis(LookDirection.x, this.transform.up);
    }
}