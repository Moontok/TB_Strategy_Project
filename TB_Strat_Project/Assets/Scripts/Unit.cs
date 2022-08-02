using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] Animator animator = null;

    float moveSpeed = 4f;
    float stoppingDistance = 0.1f;
    float rotationSpeed = 10f;
    
    Vector3 targetPosition = Vector3.zero;

    void Awake()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
