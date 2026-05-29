using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class InspectableObjectsSystem : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    private Transform inspectionPoint;
    private bool isInspecting = false;

    [TextArea(3, 10)]
    public string objectDescription;

    //List of Positions and Rotations of the inspected object
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private TextInteract txtInt;

    void Start()
    {
        txtInt = GetComponent<TextInteract>();

        //Save of the original position and rotation of the object
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (inspectionPoint == null)
        {
            inspectionPoint = GameObject.Find("Offset").transform;
        }
    }

    void Update()
    {

        if(isInspecting)
        {
            RotateObject();
            //Soft movement of the object in front of the camera
            transform.position = Vector3.Lerp(transform.position, inspectionPoint.position, 0.1f);

            //Inspection Exit
            if (Input.GetMouseButtonDown(1))
            {
                Interact();
            }
        }

        else
        {
            // Lo devolvemos suavemente a su sitio
            transform.position = Vector3.Lerp(transform.position, originalPosition, 0.1f);
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, 0.1f);
        }
    }

    public void Interact()
    {
        isInspecting = !isInspecting;

        if (isInspecting)
        {
            Selector.IsLocked = true; //RayCast Blocked
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            Selector.IsLocked = false; //Free the RayCast
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //show or hide the text on the UI
        if (txtInt != null) txtInt.playText();
    }

    private void RotateObject()
    {
        if (Input.GetMouseButton(0)) //It only rotathe if the player holds the left mouse click
        {
            float rotationSpeed = 2.0f;
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.Rotate(Vector3.up, -mouseX, Space.World);
            transform.Rotate(Vector3.back, mouseY, Space.World);
        }
    }

    public string GetDescription() => "Se interactua con el libro";
}
