using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 Targetpos = player.position + offset;
        Targetpos.x = 0;
        transform.position = Targetpos;

    }
}
