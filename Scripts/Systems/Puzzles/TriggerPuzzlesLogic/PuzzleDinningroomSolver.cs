using System.Collections;
using UnityEngine;

public class PuzzleDinningroomSolver : MonoBehaviour, ISolver
{
    public int counterObjective;
    private int count = 0;
    public GameObject door;
    public float speed = 2;

    public void trySolve()
    {
        count++;
        if (count == counterObjective) StartCoroutine(RotateDoor());
    }

    IEnumerator RotateDoor()
    {
        AudioSource AS;
        AS = door.GetComponent<AudioSource>();
        AS.Play();

        float targetAngle = 90;

        Quaternion targetRotation = Quaternion.Euler(targetAngle, 0, 0);
        Quaternion startRotation = transform.localRotation;

        float elapsed = 0;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * speed;
            door.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsed);
            yield return null;
        }

        Destroy(AS);
    }

}
