using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorSystem : MonoBehaviour, IInteractable
{
    public Transform player;
    public Camera mirrorCamera;
    public Camera lyingCamera;
    public GameObject mirrorPlane;


    private Vector3 originalPos;
    private Vector3 cursedPos;
    private Quaternion rotacionCursedInicial;

    public RenderTexture renderTexture;
    public RenderTexture renderTextureLy;

    private bool lying = false;
    private Material mirrorMaterial;

    void Awake()
    {
        if (mirrorCamera != null) originalPos = mirrorCamera.transform.position;
        if (lyingCamera != null) cursedPos = lyingCamera.transform.position;

        if (mirrorPlane != null)
        {
            Renderer r = mirrorPlane.GetComponent<Renderer>();
            if (r != null)
            {
                mirrorMaterial = r.material;
            }
            else
            {
                Debug.LogError("¡ERROR! El objeto asignado en 'mirrorPlane' no tiene un componente Renderer/MeshRenderer.", this);
            }
        }
        else
        {
            Debug.LogError("¡ERROR! No has asignado ningún objeto en la casilla 'mirrorPlane' del Inspector.", this);
        }
    }

    void Start()
    {
        lying = false;

        // Configuramos el material inicial en modo normal
        if (mirrorMaterial != null && renderTexture != null)
        {
            mirrorMaterial.mainTexture = renderTexture;
        }

        // Activamos la cámara normal
        if (mirrorCamera != null)
        {
            mirrorCamera.enabled = true;
            mirrorCamera.targetTexture = renderTexture;
            mirrorCamera.Render(); // Renderizado forzado inicial
        }

        // Dejamos la cámara cursed apagada al iniciar para no consumir recursos
        if (lyingCamera != null)
        {
            rotacionCursedInicial = lyingCamera.transform.rotation;
            lyingCamera.targetTexture = renderTextureLy;

            //IMPORTANTE PARA QUE SE VEA
            lyingCamera.enabled = true;  // ¡La encendemos un momento!
            lyingCamera.Render();         // Forzamos su renderizado en la pestaña Game
            lyingCamera.enabled = false; // Ahora sí, la apagamos de forma segura
        }

    }

    private void OnEnable()
    {
        CursedObserverSystem.OnCPressed += changeCursed;
    }

    private void OnDisable()
    {
        CursedObserverSystem.OnCPressed -= changeCursed;
    }

    private void changeCursed()
    {
        lying = !lying;

        // 1. Intercambiamos la textura que debe mostrar el material del espejo
        if (mirrorMaterial != null)
        {
            mirrorMaterial.mainTexture = lying ? renderTextureLy : renderTexture;
        }

        // 2. ¡LA CLAVE! Encendemos la cámara que se necesita y apagamos la otra
        if (mirrorCamera != null)
        {
            mirrorCamera.enabled = !lying;
        }

        if (lyingCamera != null)
        {
            lyingCamera.enabled = lying;

            // Forzamos un renderizado manual inmediato para evitar el cuello de botella del primer frame en la pestaña Game
            if (lying)
            {
                lyingCamera.Render();
            }
        }
    }

    public void Interact()
    {
        print("Espejo");
    }
    public string GetDescription() => "Se interactua con el espejo";

    void Update()
    {
        // Solo calculamos el reflejo si no estamos en el modo portal (lying)
        if (player != null)
        {
            mirrorVision();

            if (mirrorMaterial != null)
            {
                mirrorMaterial.mainTexture = lying ? renderTextureLy : renderTexture;
            }
        }
    }

    private void mirrorVision()
    {
        // --- Tu lógica de posición (se mantiene intacta) ---
        Vector3 direccionEspejoJugador = player.position - mirrorCamera.transform.position;
        Vector3 direccionReflejo = Vector3.Reflect(direccionEspejoJugador, mirrorPlane.transform.forward);


        // 2. CALCULAMOS LA ROTACIÓN DEL REFLEJO
        // Esta es la rotación que idealmente tendría el reflejo en el mundo
        Quaternion rotacionReflejoGlobal = Quaternion.LookRotation(direccionReflejo, mirrorPlane.transform.up);

        // 3. SEPARACIÓN DE ESTADOS
        if (!lying)
        {
            // MODO NORMAL: La cámara base rota de forma estándar con el reflejo
            mirrorCamera.transform.rotation = rotacionReflejoGlobal;
        }
        else
        {
            // MODO CURSED: Extraemos la rotación local del reflejo respecto al plano del espejo
            Quaternion rotacionLocalDelReflejo = Quaternion.Inverse(mirrorPlane.transform.rotation) * rotacionReflejoGlobal;

            // Aplicamos ese mismo comportamiento angular sobre la rotación base de la sala maldita
            lyingCamera.transform.rotation = rotacionCursedInicial * rotacionLocalDelReflejo;
        }
    }
}

