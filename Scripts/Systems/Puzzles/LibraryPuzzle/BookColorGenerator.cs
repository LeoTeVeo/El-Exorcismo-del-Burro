using UnityEngine;

public class BookColorGenerator : MonoBehaviour
{
    private Renderer bookRenderer;
    public Color Bcolor;

    // Start is called before the first frame update
    void Start()
    {
        bookRenderer = GetComponentInChildren<Renderer>();
        if (bookRenderer != null)
        {
            bookRenderer.material.color = Bcolor;
        }

        else Debug.LogWarning("No Renderer found in " + gameObject.name);
    }
}
