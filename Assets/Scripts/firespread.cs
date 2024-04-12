using UnityEngine;

public class FireSpread : MonoBehaviour
{
    public ParticleSystem fireEffect; 

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player")) 
        {
            fireEffect.Play(); 
        }
    }
}
