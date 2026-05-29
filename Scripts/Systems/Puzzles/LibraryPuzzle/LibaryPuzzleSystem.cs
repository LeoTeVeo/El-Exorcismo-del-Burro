using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibaryPuzzleSystem : MonoBehaviour
{
    private int count = 1;

    private int puzzleLenght = 4;

    public GameObject bars;
    private AudioSource AudioSource;

    private bool solved = false;


    public void setNumber(int order)
    {

        if (!solved)
        {
            if (count == order) count++;
            else resetAnswer();

            print(count);

            if(count > puzzleLenght) Finished();
        }
    }

    private void resetAnswer()
    {
        count = 1;
    }

    private void Finished()
    {
        solved = true;
        StartCoroutine(moveBarrs());
        
    }
   

    private IEnumerator moveBarrs()
    {
        AudioSource = bars.GetComponent<AudioSource>();
        solved = true;

        float elapsedTime = 0f;
        float totalTime = AudioSource.clip.length; 

        Vector3 startPos = bars.transform.position;
        Vector3 endPos = startPos + new Vector3 (0, -0.75f, 0);

        AudioSource.Play();

        while (elapsedTime < totalTime)
        {
            float t = elapsedTime / totalTime;  

            bars.transform.position = Vector3.Lerp (startPos, endPos, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        bars.transform.position = endPos;
        bars.GetComponent<BoxCollider>().enabled = false;
        
        
    }
}
