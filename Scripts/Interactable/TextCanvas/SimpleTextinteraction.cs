using System.Collections;
using UnityEngine;

public class SimpleTextinteraction : MonoBehaviour, IInteractable
{
    private TextInteract txtInt;
    public int textLimit;
    private bool interacting = false;

    public bool isPageText;

    private void Start()
    {
        txtInt = GetComponent<TextInteract>();
    }

    public void Interact()
    {
        if (txtInt != null && !interacting)
        {

            txtInt.playText();
            interacting = true;

            if (isPageText)
            {
                TogglePause(true);
            }

            StartCoroutine(exitText());
        }

    }
    public string GetDescription() => "Interactua con el objeto";

    IEnumerator exitText()
    {
        float timer = 0f;


        while (timer < textLimit)
        {
            if (Input.GetMouseButtonDown(1))
         {
             timer = textLimit; // Force end of loop
         }

            timer += Time.deltaTime;
            yield return null;
        }

        TogglePause(false);
        txtInt.playText();
        interacting = false;
    }

    private void TogglePause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Selector.IsLocked = true;
        }

        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Selector.IsLocked = false;
        }
    }
}
