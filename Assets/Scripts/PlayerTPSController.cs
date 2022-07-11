using UnityEngine;
using UnityEngine.Events;
public class PlayerTPSController : MonoBehaviour
{

    public Camera cam;
    public UnityEvent onInteractionInput;
    public InputData input;
    private CharacterAnimBasedMovement characterMovement;

    public bool onInteractionZone { get; set; }

    void Start()
    {
        characterMovement = GetComponent<CharacterAnimBasedMovement>();
    }

    void Update()
    {

        input.getInput();

        if (onInteractionZone && Input.GetKeyDown(KeyCode.E))
        {
            onInteractionInput.Invoke();
        }

        characterMovement.moveCharacter(input.hMovement, input.vMovement, cam, input.jump, input.dash);

    }
}