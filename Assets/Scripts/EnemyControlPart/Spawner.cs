using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] Prefabs;
    public float MinSize;
    public float MaxSize;
    public bool DrawGizmo;

    // Start is called before the first frame update
    void Start()
    {
        var enemyPrefab = Prefabs[UnityEngine.Random.Range(0, Prefabs.Length)];
        var size = UnityEngine.Random.Range(MinSize, MaxSize);
        var instance = Instantiate<GameObject>(enemyPrefab, /*parent:*/this.transform);
        instance.transform.localScale = new Vector3(size, size, 0f);
    }

    private void OnDrawGizmos()
    {
        if (DrawGizmo)
        {
            Gizmos.DrawSphere(this.transform.position, MaxSize);
        }
    }
}