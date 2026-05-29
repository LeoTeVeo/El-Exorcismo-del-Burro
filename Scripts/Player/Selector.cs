using UnityEngine;

public class Selector : MonoBehaviour
{
    //-- Ray cast system --//
    LayerMask mask;
    public float distance = 2.5f;
    public GameObject TextInteract;

    //-- Inspecting System --//
    public static bool IsLocked = false; 

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("RayCastDetect");
        if(TextInteract != null) TextInteract.SetActive(false);
    }

    void Update()
    {


        //If we are inspecting an object, we hide the text and dont cast the RayCast
        if (IsLocked)
        {
            if(TextInteract != null) TextInteract.SetActive(false);
            return;
        }

        //RayCast(origin, direction, out hit / object detected, distance, mask)
        RaycastHit hit; //Objeto con el que impacta nuestro RayCast

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance, mask))
        {
            TextInteract.SetActive(true);
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                interactable?.Interact(); //If interactable is not null, we shout the function Interact()
            }
        }     
        else if (TextInteract != null)  TextInteract.SetActive(false);
    }
}