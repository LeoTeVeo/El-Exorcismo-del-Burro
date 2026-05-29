using System.Collections;
using UnityEngine;

public class CandleSystem : MonoBehaviour
{
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    public AudioSource FireSound;

    float scroll = 0f;
    float MatchAcumulator = 0f;

    public bool cursed = false;
    public bool iluminating = false;
    public bool turningOn = false;


    [Header("Base Light Parametres")]
    public Light l;
    private float max;
    private float min;
    private float speed;

    [Header("Flicker Internals")]
    private float targetIntensity;
    private float flickerTimer;
    private float currentFlickerTime = 0.07f; // Tiempo entre cambios de intensidad
    private float currentSpeed; // Velocidad interna para el Lerp
    private float currentMin, currentMax; // Valores internos que seguiremoss

    [Header ("Not Cursed Parametres")]
    public float notC_max = 2.0f;
    public float notC_min = 1f;
    public float notC_speed = 2.0f;
    public float notC_range = 6.0f;
    public Color notC_color;

    [Header("Cursed Parametres")]
    public float C_max = 1.5f;
    public float C_min = 0.5f;
    public float C_speed = 4.0f;
    public float C_range = 6.0f;
    public Color C_color;

    [Header("Match Sounds")]
    public AudioClip[] matchVector;
    public AudioClip matchBurning;
    private bool isPlayingMatchSound = false;


    private void Start()
    {
        cursed = false;
        iluminating = false;
        l.enabled = false;
        ApplySettings(notC_range, notC_color, notC_max, notC_min, notC_speed, 0.5f);
    }

    private void OnEnable()
    {
        CursedObserverSystem.OnCPressed += changeSetupLightCursed;
    }

    private void OnDisable()
    {
        CursedObserverSystem.OnCPressed -= changeSetupLightCursed;
    }


    // Update is called once per frame
    void Update()
    {
        HandleFlicker();

        if (Input.GetKeyDown(KeyCode.X))
        {
            TurnLight();
        }

        if (!iluminating)
        {
            //Ignition
            scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll < 0)
            {
                if (!turningOn)
                {
                    StartCoroutine(TurningCandleOn());
                }
                if (!isPlayingMatchSound)
                {
                    StartCoroutine(playMatchSound());
                }
                MatchAcumulator++;
            }
        }

    }

    private void changeSetupLightCursed()
    {
        if (!iluminating) return;

        cursed = !cursed;

        if (cursed)
        {
           ApplySettings(C_range, C_color, C_max, C_min, C_speed, 1f);
        }
        else
        {
            ApplySettings(notC_range, notC_color, notC_max, notC_min, notC_speed, 0.5f);
        }
    }

    private void ApplySettings(float r, Color c, float ma, float mi, float s, float shadow)
    {
            l.range = r;
            l.color = c;
            max = ma;
            min = mi;
            speed = s;
            l.shadowStrength = shadow;
    }
    

    private void TurnLight()
    {
        iluminating = !iluminating;
        l.enabled = iluminating;

        if (!iluminating)
        {
            cursed = false;
            ps1.Stop();
            ps2.Stop();
            FireSound.Stop();
            ApplySettings(0, notC_color, 0, 0, 0, 1f);
        }
        else
        {
            ApplySettings(notC_range, notC_color, notC_max, notC_min, notC_speed, 0.5f);

            ps1.Play();
            ps2.Play();
            FireSound.Play();

            GameObject tempObj = new GameObject("MatchBurning");
            tempObj.transform.position = transform.position;
            AudioSource source = tempObj.AddComponent<AudioSource>();

            AudioClip clip = matchBurning;

            source.clip = clip;
            source.volume = 0.6f;

            source.Play();
            Destroy(tempObj, clip.length);

        }
    }

    private void HandleFlicker()
    {
        if (!iluminating || l == null)
        {
            if (l != null) l.intensity = 0;
            return;
        }

        flickerTimer -= Time.deltaTime;

        if (flickerTimer <= 0)
        {
            // Elegimos un nuevo destino aleatorio dentro de tu rango
            targetIntensity = Random.Range(min, max);

            // Si quieres que el parpadeo sea más irregular, puedes aleatorizar el tiempo
            flickerTimer = Random.Range(0.05f, 0.1f);
        }

        // Suavizado del movimiento de la luz
        l.intensity = Mathf.Lerp(l.intensity, targetIntensity, Time.deltaTime * speed);
    }

    IEnumerator TurningCandleOn()
    {
        float timer = 0; //Start Timer
        turningOn = true; //Block

        while (timer < 2f)
        {
            timer += Time.deltaTime;
            if (MatchAcumulator >= 15) 
            {
                MatchAcumulator = 0; //Reset of the mouse wheel counter
                turningOn = false; //Stop blocking
                TurnLight(); //Turn on the lights
                yield break;
            }
            yield return null;
        }

        MatchAcumulator = 0; //Reset of the mouse wheel counter
        turningOn = false; //Stop blocking
        yield break;
    }

    IEnumerator playMatchSound()
    {
        isPlayingMatchSound = true; //Block

        //Play random clip and variations
        if (matchVector.Length > 0)
        {
            GameObject tempObj = new GameObject("MatchSound");
            tempObj.transform.position = transform.position;
            AudioSource source = tempObj.AddComponent<AudioSource>();

            AudioClip clip = matchVector[Random.Range(0, matchVector.Length)];

            source.clip = clip;
            source.volume = 0.6f;

            source.Play();
            yield return new WaitForSeconds(clip.length);

            isPlayingMatchSound = false;
            Destroy(tempObj);
        }
    }
}
