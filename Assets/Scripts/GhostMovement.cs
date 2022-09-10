using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public Transform[] waypoints; //Arreglo de puntos de ruta para el movimiento del fantasma.
    private int currentWaypoint = 0;
    public float speed = 0.3f;
    public bool shouldWaitHome = false;

    private void Update()
    {
        if(GameManager.sharedInstance.invincibleTime>0)
        {
            GetComponent<Animator>().SetBool("PacManInvincible", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("PacManInvincible", false);
        }
    }

    private void FixedUpdate()
    {
        GetComponent<AudioSource>().volume = 0.1f;
        if(GameManager.sharedInstance.gameStarted && !GameManager.sharedInstance.gamePaused)
        {
            if(!shouldWaitHome)
            {
                float distanceToWaypoint = Vector2.Distance((Vector2)this.transform.position,
                                       (Vector2)waypoints[currentWaypoint].position);       //Distancia entre el fantasma y el punto de destino
                if(distanceToWaypoint < 0.1f)   //Si la distancia entre el fantasma y el waypoint es menor a 0.1, pasa al siguiente waypoint.
                {
                    currentWaypoint = (currentWaypoint + 1) % waypoints.Length; //Se pasa al siguiente waypoint, pero si estamos en el ultimo, el ultimo waypoint se divide entre si mismo, dando residuo 0 y comenzando de nuevo los waypoints.
                    Vector2 newDirection = waypoints[currentWaypoint].position - this.transform.position; //El vector de direccion se calcula mediante la posicion actual menos la posicion destino.
                    GetComponent<Animator>().SetFloat("DirX", newDirection.x);
                    GetComponent<Animator>().SetFloat("DirY", newDirection.y);
                }
                else
                {
                    Vector2 newPos = Vector2.MoveTowards(this.transform.position,
                                                        waypoints[currentWaypoint].position,
                                                        speed * Time.deltaTime);
                    GetComponent<Rigidbody2D>().MovePosition(newPos);
                }
            }
        }
        else
        {
            GetComponent<AudioSource>().volume = 0.0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            if(GameManager.sharedInstance.invincibleTime<=0)
            {
                GameManager.sharedInstance.gameStarted = false; 
                Destroy(otherCollider.gameObject);
                StartCoroutine("RestartGame");
            }
            else
            {
                UIManager.sharedInstance.ScorePoints(100);
                GameObject home = GameObject.Find("GhostHome");
                this.transform.position = home.transform.position;
                this.currentWaypoint = 0;
                this.shouldWaitHome = true;
                StartCoroutine("AwakeFromHome");
            }
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        GameManager.sharedInstance.RestartGame();
    }

    IEnumerator AwakeFromHome()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        this.shouldWaitHome = false;
        this.speed *= 1.2f; //Cada vez que el fantasma sale de casa es 20% mas rapido
    }
}
