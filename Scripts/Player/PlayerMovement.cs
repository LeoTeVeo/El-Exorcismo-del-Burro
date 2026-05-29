using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState {Free, onSpline}
    public PlayerState currentState = PlayerState.Free;


    [Header("Camera and rotation")]

    public Camera playerCamera;
    public float lookSpeed;
    public float lookXLimit;

    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float jumpPower;
    public float gravity;
    public bool isMoving = false;

    [Header("Crouch")]
    public float defaultHeight;
    public float crouchHeight;
    public float crouchSpeed;
    private KeyCode crouchKey = KeyCode.LeftControl;
    private KeyCode runKey = KeyCode.LeftShift;

    [Header("Steps Sounds")]
    public AudioClip[] stepsVector;
    public float walkingStepInterval;
    public float runningStepInterval;
    public float crouchingStepInterval;
    private float stepTimer = 0;

    [Header("SpLines Conf")]
    public SplineContainer spl;
    private float splineT = 0f;


    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    public CharacterController characterController;
    private bool canMove = true;

    private float baseWalkSpeed;
    private float baseRunSpeed;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        baseWalkSpeed = walkSpeed;
        baseRunSpeed = runSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //If the selector says the player is bussy, it locks the movement
        if (Selector.IsLocked) return;

        ApplyRotation();

        if (currentState == PlayerState.Free) FreeMovement();
        else HandleSplineMovement();
    }

    private void FreeMovement()
    {
        bool isRunning = Input.GetKey(runKey);
        bool isCrouching = Input.GetKey(crouchKey);

        if ((canMove))
        {
            HandleCrouch();
            ApplyFreeMovement(isRunning);
        }

        isMoving = characterController.velocity.magnitude > 0.1f;
    }

    //-- Crouch movement logic --// 
    private void HandleCrouch()
    {
        if(Input.GetKey(crouchKey))
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            //Reset values
            walkSpeed = baseWalkSpeed;
            runSpeed = baseRunSpeed;
        }
    }


    //-- Free movement logic --// 
    private void ApplyFreeMovement(bool isRunning)
    {

        //Calculate directions
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //Horizontal speeds
        float speed = isRunning ? runSpeed : walkSpeed;
        float curSpeedX = speed * Input.GetAxis("Vertical");
        float curSpeedY = speed * Input.GetAxis("Horizontal");

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);


        //Jump and gravity
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
             moveDirection.y = jumpPower;
        
        else moveDirection.y = movementDirectionY;
       

        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;
        
        else if (moveDirection.y < 0)
            moveDirection.y = 0;
        
        characterController.Move(moveDirection * Time.deltaTime);

        if (isMoving) stepsSounds(isRunning, false);
    }

    //-- SPLine Movement logic --//
    private void HandleSplineMovement()
    {
        float input = Input.GetAxis("Vertical");
        print(splineT);

        if(Mathf.Abs(input) > 0.01f)
        {
            // VERIFICACIÓN DE TOPE: 
            if ((splineT >= 1f && input > 0) || (splineT <= 0f && input < 0))  return;
            

            float splineLength = spl.CalculateLength();
            float moveAmount = (input * walkSpeed * Time.deltaTime) / splineLength;
            splineT = Mathf.Clamp01(splineT + moveAmount);

            //Apply position
            transform.position = (Vector3)spl.EvaluatePosition(splineT);
            if(splineT >= 0.725f) splineT = 0.219f;

            // Orientar cuerpo según el raíl
            Vector3 forward = (Vector3)spl.EvaluateTangent(splineT);

            stepsSounds(false, false);
        }
    }

    public void conectingRailSignal()
    {
        StartCoroutine(ConnectingToRail());
    }
    IEnumerator ConnectingToRail()
    {
        currentState = PlayerState.onSpline;
        characterController.enabled = false;

        Vector3 targetpos = (Vector3)spl.EvaluatePosition(splineT);
        float transitionTime = 0.5f; // Medio segundo de "encaje"
        float elapsed = 0;
        Vector3 startPos = transform.position;

        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetpos, elapsed / transitionTime);
            yield return null;
        }
        //characterController.enabled = true;
    }
    private void ExitSpline()
    {
        currentState = PlayerState.Free;
        characterController.enabled = true;
        // Reset de rotación de cámara para que vuelva al centro del cuerpo
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    //-- Camera rotation logic --//
    private void ApplyRotation()
    {
        //Rotation
        if (playerCamera == null)
        {
            return;
        }

        //Vertical Rotation (Camera)
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        //Horizontal Rotation (Body)
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    //-- Random Steps sounds generator --//
    private void stepsSounds(bool isRunning, bool isCrouching)
    {
        float currentInterval = isCrouching ? crouchingStepInterval : (isRunning ? runningStepInterval : walkingStepInterval);
        float volume = isCrouching ? 0.1f : (isRunning ? 0.9f : 0.6f);

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0)
        {
            //Play random clip and variations
            if (stepsVector.Length > 0)
            {
                GameObject tempObj = new GameObject("StepSound");
                tempObj.transform.position = transform.position;
                AudioSource source = tempObj.AddComponent<AudioSource>();

                AudioClip clip = stepsVector[Random.Range(0, stepsVector.Length)];
                
                source.clip = clip;
                source.volume = volume;

                source.Play();
                Destroy(tempObj, clip.length);
            }
            stepTimer = currentInterval;
        }
    }

    public void changePlayerState()
    {
       if(currentState == PlayerState.onSpline) currentState = PlayerState.Free;

       else currentState = PlayerState.onSpline;
    }
}