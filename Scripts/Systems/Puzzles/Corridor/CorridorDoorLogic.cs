using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorDoorLogic : MonoBehaviour, ISolver
{
    private int count = 0;
    public int countObjective = 2;
    public void trySolve()
    {
        count ++;
        if (count == countObjective) print("se abre");
    }
}
