using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RandomForceOnSpawn : MonoBehaviour
{
    [SerializeField] private float minForce = 2f;
    [SerializeField] private float maxForce = 5f;
    private Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();
    private void Start() => ApplyRandomForce();
    private void ApplyRandomForce()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float forceMagnitude = Random.Range(minForce, maxForce);
        rb.AddForce(randomDir * forceMagnitude, ForceMode2D.Impulse);
    }
}
