using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnspeed = 90f;

    private void OnTriggerEnter(Collider other)
    {
        // If coin touches an obstacle, destroy the coin (not the obstacle)
        if (other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }

        // Ignore anything that’s not the player
        if (other.gameObject.name != "Player")
        {
            return;
        }

        // Player collects the coin
        GameManager.inst.incrementScore();
        Destroy(gameObject);
    }

    public void Update()
    {
        transform.Rotate(0, 0, turnspeed * Time.deltaTime);
    }
}
