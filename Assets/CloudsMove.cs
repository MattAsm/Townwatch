using Unity.VisualScripting;
using UnityEngine;

public class CloudsMove : MonoBehaviour
{

    private Vector3 clouds;
    private Vector3 destination;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clouds = new Vector3(36, 0, 0);
        destination = new Vector3(0.1f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        if(transform.position.x <= 0f)
        {
            transform.position = clouds;
        }
    }
}
