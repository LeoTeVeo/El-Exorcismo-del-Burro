using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedObserverSystem : MonoBehaviour
{
    public static event Action OnCPressed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnCPressed?.Invoke();
        }
    }
}
