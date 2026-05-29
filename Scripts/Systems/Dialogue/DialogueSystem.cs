using UnityEngine;
using TMPro;
using System.Collections;
public class DialogueSystem : MonoBehaviour, IInteractable
{
    //Desplegable de personajes
    public enum CharacterType {Bartolome, Jugador, Lolita}

    //Estructura de las lineas del guion
    [System.Serializable]
    public struct DialogueLine
    {
        public CharacterType character; //Quien habla?
        [TextArea(1,5)] public string text; //Que dice?
        public AudioSource audioSourceCharacter; //Donde habla?
    }


    [Header("Configuración de Interfaz")] //UI
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;


    [Header("Guión del Diálogo")]
    [SerializeField] private DialogueLine[] scriptLines;


    [Header("Configuración de Audio")]

    [SerializeField] private AudioSource BartolomeVoice;
    [SerializeField] private AudioSource JugadorVoice;
    [SerializeField] private AudioSource LolitaVoice;



    [Header("Tiempos")]
    public float typingTime = 0.05f;
    

    private AudioSource audioSource; //Referencia al AudioSource que se usará al hablar
    public int charsToPlay;
    private bool didDialogueStart = false;
    private int lineIndex = 0;
    private Coroutine typingCoroutine;



    private void Update()
    {
        if (!didDialogueStart) return;

        if (Input.GetMouseButtonDown(1))
        {
            // Comparamos con el texto de la línea actual del guión
            if (dialogueText.text == scriptLines[lineIndex].text) //Si texto completo
            {
                NextDialogueLine();
            }
            else
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = scriptLines[lineIndex].text;
                if (audioSource != null) audioSource.Stop(); // Paramos el audio si el jugador salta el texto
            }
        }
    }

    public void Interact()
    {
        if(!didDialogueStart) StartDialogue();
    }

    public string GetDescription() => ("Empieza Dialongo");

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;
        int charIndex = 0;

        // Ejecutamos la lógica de audio correspondiente al personaje actual
        SetActualAudioSource();

        foreach(char c in scriptLines[lineIndex].text)
        {
            dialogueText.text += c;

            if (charIndex % charsToPlay == 0)
            {
                audioSource.Play();
            }

            charIndex++;
            yield return new WaitForSeconds(typingTime);
        }
    }

    private void SetActualAudioSource()
    {
        if (audioSource != null)  audioSource.Stop(); // Cortamos cualquier audio anterior

        // 1. Comprobamos si la línea actual tiene un audio exclusivo asignado en el Inspector
        if (scriptLines[lineIndex].audioSourceCharacter != null)
        {
            return;
        }

        else
        {
            switch (scriptLines[lineIndex].character)
            {
                case CharacterType.Bartolome:
                    audioSource = BartolomeVoice;
                    break;
                case CharacterType.Jugador:
                    audioSource = JugadorVoice;
                    break;
                case CharacterType.Lolita:
                    audioSource = LolitaVoice;
                    break;
            }
        }
    }

    private void StartDialogue()
    {
        if (scriptLines.Length == 0) return;

        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        lineIndex = 0;

        typingCoroutine = StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        if(lineIndex < scriptLines.Length)
        {
            typingCoroutine = StartCoroutine(ShowLine());
        }

        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            if (audioSource != null) audioSource.Stop();
        }
    }
}
