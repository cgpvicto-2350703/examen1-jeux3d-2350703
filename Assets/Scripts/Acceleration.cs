using UnityEngine;

public class Acceleration : MonoBehaviour
{
    [SerializeField, Tooltip("force en z")]
    private float forceZ = 15;
    [SerializeField, Tooltip("duree acceleration")]
    private float delaisAccel = 1;
    private float tempsAccel = 0;
    private bool accel=false;
    [SerializeField, Tooltip("nombre de charges dacceleration")]
    private int nbCharge;
    Rigidbody boule;

    private void Update()
    {
        if (accel == true)
        {
            if (tempsAccel >= delaisAccel)
            {
                nbCharge--;
                if (nbCharge == 0)
                {
                    boule.AddForce(0, 0, -150, ForceMode.Impulse);
                    accel = false;
                }
            } 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boule"))
        {
            boule = other.GetComponent<Rigidbody>();
            if (accel == false)
            {
                boule.AddForce(0, 0, 150, ForceMode.Impulse);
                tempsAccel += Time.deltaTime;
                nbCharge++;

            }
            else if (accel == true || nbCharge<3)
            {
                nbCharge++;
            }               

        }
    }
}
