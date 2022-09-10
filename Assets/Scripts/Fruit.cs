using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            UIManager.sharedInstance.ScorePoints(100);
            GameManager.sharedInstance.MakeInvincibleFor(10.0f);
            Destroy(this.gameObject);
        }
    }
}
