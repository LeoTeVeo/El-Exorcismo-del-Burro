using UnityEngine;

public class grabObject : MonoBehaviour
{
    public float grabDistance = 4f;
    public float followSpeed = 10f;
    public Transform holdParent;

    private Rigidbody heldObj;

    // Update is called once per frame
    void Update()
    {

            if (Input.GetMouseButtonDown(0)) tryGrab();

            if (Input.GetMouseButtonUp(0) && heldObj != null) DropObject();

            if (heldObj != null) MoveObject();
    }


    private void tryGrab()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, grabDistance))
        {
            if (hit.rigidbody != null)
            {
                heldObj = hit.rigidbody;
                heldObj.useGravity = false;
                heldObj.drag = 10f;
                heldObj.angularDrag = 5f;
            }
        }
    }

    void DropObject()
    {
        heldObj.useGravity = true;
        heldObj.drag = 1f;
        heldObj.angularDrag = 0.05f;
        heldObj = null;

    }

    void MoveObject()
    {
        // En lugar de posicionar, calculamos la dirección hacia el objetivo
        Vector3 direction = holdParent.position - heldObj.position;
        heldObj.velocity = direction * followSpeed;
    }

}
