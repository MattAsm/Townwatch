using UnityEngine;

public class PrefabReference : MonoBehaviour
{
    public GameObject prefab; // Reference to the original prefab

    void Awake()
    {
       
    }

    public void ifBoughtDestroy()
    {
        Destroy(prefab);
    }
}