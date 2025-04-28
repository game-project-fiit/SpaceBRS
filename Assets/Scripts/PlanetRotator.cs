using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private void Update()
        => transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
}
