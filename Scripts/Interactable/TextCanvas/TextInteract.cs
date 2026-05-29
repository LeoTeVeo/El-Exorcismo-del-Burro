using UnityEngine;
public class TextInteract : MonoBehaviour
{
    public bool texting = false;

    [Header("UI")]
    public GameObject inspectionUI;
    public TMPro.TextMeshProUGUI descriptionText;
    private ParticleSystem lightPack;

    [TextArea(3, 10)]
    public string objectDescription;

    private void Start()
    {
        lightPack = GetComponentInChildren<ParticleSystem>();
    }

    public void playText()
    {
        texting = !texting;

        if (texting)
        {
            //Show UI
            if (descriptionText != null) descriptionText.text = objectDescription;
            if (inspectionUI != null) inspectionUI.SetActive(true);

            if(lightPack != null) Destroy(lightPack.gameObject);
        }

        else
        {
            //Hide UI
            if (inspectionUI != null) inspectionUI.SetActive(false);
        } 
    }
}
