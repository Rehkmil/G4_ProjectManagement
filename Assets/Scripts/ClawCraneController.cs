using UnityEngine;

public class ClawCraneController : MonoBehaviour
{
    [Header("Crane Hierarchy Setup")]
    public Transform clawTransform;
    public Transform holdAnchor;

    public Animator animator;

    [Header("Audio")]
    public AudioSource moveSound;

    [Header("Speeds")]
    public float horizontalClawSpeed = 5f;
    public float verticalRailSpeed = 4f;

    [Header("World Boundaries")]
    public float minX = -6f; 
    public float maxX = 3f;
    public float minY = -3f; 
    public float maxY = 5f;

    [Header("Grab Logic")]
    public float grabRadius = 0.5f; 
    private GameObject heldItem;
    private bool isCarrying = false;

    private void Update()
    {
        if (!GameManager.instance.isGameActive)
        {
            StopMoveSound();
            return;
        }

        HandleMovement();
        HandleGrab();
    }

    private void HandleMovement()
    {
        bool isMoving = false;

        float moveY = Input.GetAxisRaw("Vertical");
        if (moveY != 0)
        {
            Vector3 newRailPos = transform.position + Vector3.up * moveY * verticalRailSpeed * Time.deltaTime;
            newRailPos.y = Mathf.Clamp(newRailPos.y, minY, maxY);

            if (!Mathf.Approximately(newRailPos.y, transform.position.y))
                isMoving = true;

            transform.position = newRailPos;
        }

        if (clawTransform != null)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            if (moveX != 0)
            {
                Vector3 newClawPos = clawTransform.position + Vector3.right * moveX * horizontalClawSpeed * Time.deltaTime;
                newClawPos.x = Mathf.Clamp(newClawPos.x, minX, maxX);

                if (!Mathf.Approximately(newClawPos.x, clawTransform.position.x))
                    isMoving = true;

                clawTransform.position = newClawPos;
            }
        }

        if (isMoving) PlayMoveSound();
        else StopMoveSound();
    }

    private void PlayMoveSound()
    {
        if (moveSound != null && !moveSound.isPlaying)
            moveSound.Play();
    }

    private void StopMoveSound()
    {
        if (moveSound != null && moveSound.isPlaying)
            moveSound.Stop();
    }

    private void HandleGrab()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isCarrying)
            {
                AttemptGrab();
            }
            else
            {
                DropItem();
                animator.ResetTrigger("Grabbed");
            }
        }
    }

    private void AttemptGrab()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(holdAnchor.position, grabRadius);
        
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Item"))
            {
                heldItem = hit.gameObject;
                isCarrying = true;
                animator.SetTrigger("Grabbed");
                animator.SetBool("IsGrabbing", true);

                ConveyorItem conveyorMovement = heldItem.GetComponent<ConveyorItem>();
                if (conveyorMovement != null) conveyorMovement.enabled = false;

                GameManager.instance.ProcessGrab(heldItem);

                heldItem.transform.position = holdAnchor.position;
                heldItem.transform.SetParent(holdAnchor);
                
                return; 
            }
        }
    }

    private void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            heldItem = null;
        }
        isCarrying = false;
        animator.ResetTrigger("Grabbed");
        animator.SetBool("IsGrabbing", false);
    }

    private void OnDrawGizmosSelected()
    {
        if (holdAnchor != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(holdAnchor.position, grabRadius);
        }
    }
}