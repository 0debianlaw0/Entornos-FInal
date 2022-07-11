using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class CharacterAnimBasedMovement : MonoBehaviour
{

    public float rotationSpeed = 4f;
    public float rotationThreshold = 0.3f;
    [Range(0, 180f)]
    public float degreesToTurn = 160f;

    [Header("Animator Parameters")]
    public string motionParam = "motion";
    public string mirrorIdleParam = "mirrorIdle";
    public string turn180Param = "turn180";
    public string jumpParam = "jump";

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    public float range = 3f;
    private float Speed;
    private Vector3 desiredMoveDirection;
    private CharacterController characterController;
    private ArticulationBody articulationBody;
    private Animator animator;
    private InputData input;
    public Transform wallDetector;
    private bool mirrorIdle;
    private bool turn180;
    private bool onAir = false;

    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
        input = this.GetComponent<InputData>();
        articulationBody = this.GetComponent<ArticulationBody>();
        this.characterController.detectCollisions = true;
        //this.characterController.attachedRigidbody.mass = 90f;
        //this.characterController.attachedRigidbody.useGravity = true;
        
    }
    
    private void Update()
    {
        input.getInput();
        if (input.jump && onAir == false)
        {
            onAir = true;
            animator.SetFloat(jumpParam, 1f);
            StartCoroutine(OnAir());
        }
        Vector3 direction = Vector3.forward;
        Ray theRay = new Ray(new Vector3(transform.position.x, transform.position.y + 8, transform.position.z), transform.TransformDirection(direction * range));
        //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 8, transform.position.z), transform.TransformDirection(direction * range));

        if (Physics.Raycast(theRay, out RaycastHit hit, range))
        {
            if (hit.collider.gameObject.layer == 6)
            {
                Debug.Log("Pared!");
                animator.SetBool("wall", true);
            }
        }
        else
        {
            animator.SetBool("wall", false);        }
    }

    public void moveCharacter(float hInput, float vInput, Camera cam, bool jump, bool dash)
    {

        // Calculate the Input Magnitude
        Speed = new Vector2(hInput, vInput).normalized.sqrMagnitude;

        // Dash only if character has reached maxSpeed(animator parameter value)
        if (Speed >= Speed - rotationThreshold && dash)
        {
            Speed = 1.5f;
        }


        //Physically move player
        if (Speed > rotationThreshold)
        {
            animator.SetFloat(motionParam, Speed, StartAnimTime, Time.deltaTime);
            Vector3 forward = cam.transform.forward;
            Vector3 right = cam.transform.right;
            //Vector3 up = cam.transform.up;

            forward.y = 0f;
            right.y = 0f;

            //up.Normalize();
            forward.Normalize();
            right.Normalize();

            // Rotate the character towards desired move direction based on player Input and camera position
            desiredMoveDirection = forward * vInput + right * hInput;

            if (Vector3.Angle(transform.forward, desiredMoveDirection) >= degreesToTurn)
            {
                turn180 = true;
            }
            else
            {
                turn180 = false;
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(desiredMoveDirection),
                    rotationSpeed * Time.deltaTime);
            }

            animator.SetBool(turn180Param, turn180);

            animator.SetFloat(motionParam, Speed, StartAnimTime, Time.deltaTime);

        }
        else if (Speed < rotationThreshold)
        {
            animator.SetBool(mirrorIdleParam, mirrorIdle);
            animator.SetFloat(motionParam, Speed, StopAnimTime, Time.deltaTime);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {

        if (Speed < rotationThreshold) return;

        float distanceToLeftFoot = Vector3.Distance(transform.position, animator.GetIKPosition(AvatarIKGoal.LeftFoot));
        float distanceToRightFoot = Vector3.Distance(transform.position, animator.GetIKPosition(AvatarIKGoal.RightFoot));

        if (distanceToRightFoot > distanceToLeftFoot)
        {
            mirrorIdle = true;
        }
        else
        {
            mirrorIdle = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            onAir = false;
        }
    }

    IEnumerator OnAir()
    {
        yield return new WaitForSeconds(0.1f);
        onAir = false;
        animator.SetFloat(jumpParam, 0);
    }

}