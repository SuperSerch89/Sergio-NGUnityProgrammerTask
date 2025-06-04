using UnityEngine;
using System.Collections.Generic;

public class RandomPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private int numberOfObjects = 10;
    [SerializeField] private Vector2 areaSize = new Vector2(10f, 10f);
    [SerializeField] private Vector2 areaCenter = Vector2.zero;
    [SerializeField] private bool avoidOverlap = false;
    [SerializeField] private float minDistance = 1f;

    private List<Vector2> placedPositions = new List<Vector2>();

    private void Start() => PlaceObjects();
    private void PlaceObjects()
    {
        if (prefabs.Count == 0 || numberOfObjects <= 0)
        {
            Debug.LogWarning("No prefabs assigned or number of objects is zero.");
            return;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
            Vector2 position;
            int safetyCounter = 0;
            do
            {
                float x = Random.Range(areaCenter.x - areaSize.x / 2f, areaCenter.x + areaSize.x / 2f);
                float y = Random.Range(areaCenter.y - areaSize.y / 2f, areaCenter.y + areaSize.y / 2f);
                position = new Vector2(x, y);
                safetyCounter++;
            }
            while (avoidOverlap && !IsPositionValid(position) && safetyCounter < 100);

            placedPositions.Add(position);
            Instantiate(prefab, new Vector3(position.x, position.y, 0f), Quaternion.identity).transform.SetParent(transform);
        }
    }

    private bool IsPositionValid(Vector2 newPos)
    {
        foreach (Vector2 pos in placedPositions)
        {
            if (Vector2.Distance(pos, newPos) < minDistance) { return false; }
        }
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
