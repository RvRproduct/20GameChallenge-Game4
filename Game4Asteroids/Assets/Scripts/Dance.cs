using UnityEngine;

public class Dance : MonoBehaviour
{
    private Quaternion initialRotation;
    private bool isRightDance = true;
    [SerializeField] private Quaternion rightDance = Quaternion.Euler(0f, 0f, 45f);
    [SerializeField] private Quaternion leftDance = Quaternion.Euler(0f, 0f, -45f);
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float rotationThreshold = 1.0f;

    private void Awake()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        Dancing();
    }

    private void Dancing()
    {
        if (isRightDance)
        {
            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rightDance,
            rotationSpeed * Time.deltaTime
            );

            float angleDiff = Quaternion.Angle(transform.rotation, rightDance);

            if (angleDiff < rotationThreshold)
            {
                transform.rotation = rightDance;
                isRightDance = false;
            }
        }
        else
        {
           transform.rotation = Quaternion.Slerp(
           transform.rotation,
           leftDance,
           rotationSpeed * Time.deltaTime
           );

            float angleDiff = Quaternion.Angle(transform.rotation, leftDance);

            if (angleDiff < rotationThreshold)
            {
                transform.rotation = leftDance;
                isRightDance = true;
            }
        }
    }
}
