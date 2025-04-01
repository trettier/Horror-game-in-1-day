using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WallsOnEnter : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.CompareTag("Lighter"))
        {
            gameObject.GetComponent<ShadowCaster2D>().enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {

        if (collider.CompareTag("Lighter"))
        {
            gameObject.GetComponent<ShadowCaster2D>().enabled = true;
        }
    }

}
