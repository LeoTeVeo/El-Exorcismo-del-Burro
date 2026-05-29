using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TriggerShow : MonoBehaviour
{
    public GameObject puzzleLogic;
    ISolver solver;
    public string correctType;
    private MeshRenderer MR;

    private void Start()
    {
        solver = puzzleLogic.GetComponent<ISolver>();
        if(solver != null) { print("ya"); }
        MR = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(correctType))
        {
            MR.enabled = true;
            Destroy(collision.gameObject);
            solver?.trySolve();

        }
    }
}
