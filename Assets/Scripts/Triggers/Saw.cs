using UnityEngine;

public class Saw : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            //Die.PlayerDie();
        }
    }
}