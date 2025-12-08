using System;       //nodig voor Action
using UnityEngine;

public class BumperHit : MonoBehaviour
{
    [SerializeField] private int scoreValue = 100;
    public static event Action<string, int> onBumperHit;
    public int hitsToDestroy = 3;     // totaal aantal hits dat deze peg aankan
    public int pointsPerHit = 10;     // aantal punten dat één hit waard is


  private void OnCollisionEnter(Collision collision )
   {
        if (collision.gameObject.CompareTag("Ball")) {
            // score toekennen
            

            // aftellen
            hitsToDestroy--;

            onBumperHit?.Invoke(gameObject.tag, scoreValue);//bericht versturen dat er een bumper geraakt is. De tag en waarde sturen we mee


            // check of de peg nu op is
            if (hitsToDestroy <= 0)
            {
                Destroy(gameObject, 0.25f);
            } 
        }
   }
}