using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class booksPuzzleSystem : MonoBehaviour, IInteractable
{
    public int order;

    public LibaryPuzzleSystem LibaryPuzzleSystem;
    private AudioSource AS; 

    private void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (AS != null && !AS.isPlaying)
        {
            AS.Play();
            LibaryPuzzleSystem.setNumber(order);
        }
    }

    public void ResetPosition()
    {
        
    }

    public string GetDescription() => "Se interactua con el libro";

}
