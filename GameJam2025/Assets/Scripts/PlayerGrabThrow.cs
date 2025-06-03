using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabThrow : MonoBehaviour
{
    [Header("References")]
    public Transform holdPoint;
    public Transform grabDetector; 
    public LayerMask grabbableLayer;

    [Header("Settings")]
    public float grabDistance = 1f;
    public float throwForce = 10f;
    public float followSpeed = 10f;

    private GrabbableObject heldObject;
    private PlayerMovement playerMovement;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Grab.performed += ctx => OnGrab();

        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();  

    private void Update()
    {
        if (heldObject != null)
        {
            heldObject.transform.position = Vector2.Lerp(
                heldObject.transform.position,
                holdPoint.position,
                followSpeed * Time.deltaTime
            );
        }
    }

    private void OnGrab()
    {
        if (heldObject == null)
            TryGrab();
        else
            Throw();
    }

    private void TryGrab()
    {
        if (grabDetector == null) return; 

        Collider2D[] hits = Physics2D.OverlapCircleAll(grabDetector.position, grabDistance, grabbableLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out GrabbableObject obj))
            {
                heldObject = obj;
                heldObject.PickUp();
                break;
            }
        }
    }

    private void Throw()
    {
        float angleDegrees = 45f;
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        int facing = playerMovement.FacingDirection;

        Vector2 dir = new Vector2(Mathf.Cos(angleRadians) * facing, Mathf.Sin(angleRadians));
        heldObject.Throw(dir, throwForce);
        heldObject = null;
    }


    private void OnDrawGizmosSelected()
    {
        if (grabDetector == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(grabDetector.position, grabDistance);
    }


}
