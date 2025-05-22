using UnityEngine;

public class RandomPlanetRotator : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float minRotationSpeed = -30f;
    public float maxRotationSpeed = 30f;
    public float minInterval = 1f;
    public float maxInterval = 2f;

    private float nextChangeTime;

    private void Start()
        => ScheduleNextChange();

    private void Update()
    {
        if (Time.time >= nextChangeTime)
        {
            rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
            ScheduleNextChange();
        }

        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void ScheduleNextChange()
    {
        var interval = Random.Range(minInterval, maxInterval);
        nextChangeTime = Time.time + interval;
    }
}