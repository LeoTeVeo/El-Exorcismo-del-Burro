using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DoorScript : MonoBehaviour, IInteractable
{
    public AudioClip openAudio;
    public Animator animator;


    [Header("Open / close values")]
    public bool canOpen = true;
    public float openAngle = 0;
    public float closeAngle = 0;
    public float speed = 2.0f;
    private bool isOpen = false;
    private bool isMoving = false;


    
    public void Interact()
    {
        if ((canOpen))
        {
            if (isMoving) return;

            makeSound();
            StartCoroutine(OpenDoorSequence());
        }
    }
    public string GetDescription() => "Se interactua con la puerta";

    IEnumerator OpenDoorSequence()
    {
        isMoving = true;

        if(animator != null)
        {
            animator.SetTrigger("interact");
        }

        float targetAngle = isOpen ? closeAngle : openAngle;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

        float elapsed = 0;
        Quaternion startRotation = transform.localRotation;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * speed;
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsed);
            yield return null;
        }

        isOpen = !isOpen;
        isMoving = false;
        if (animator != null)
        {
            animator.ResetTrigger("interact");
        }
    }

    private void makeSound()
    {
        GameObject tempObj = new GameObject("DoorSound");
        AudioSource source = tempObj.AddComponent<AudioSource>();
        source.clip = openAudio;
        source.Play();
        Destroy(tempObj, openAudio.length);
    }


}
