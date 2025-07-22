using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject pressEIcon;

    [Header("Door")]
    public bool destroyOnInteract = true;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool playerInRange = false;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        pressEIcon.SetActive(false);
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + Vector3.up * openAngle);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            pressEIcon.SetActive(false);
            if (destroyOnInteract)
            {
                Destroy(gameObject);
            }
            else
            {
                isOpen = true;
            }
        }

        if (isOpen && !destroyOnInteract)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pressEIcon.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressEIcon.SetActive(false);
        }
    }
}
