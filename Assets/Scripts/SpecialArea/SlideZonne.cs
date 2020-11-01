using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideZonne : MonoBehaviour
{
    Playercontroler playercontroler;
    // Start is called before the first frame update
    [SerializeField] float slideDeceleration= 1 ;


    private void OnTriggerEnter2D(Collider2D other)
    {
        playercontroler = other.gameObject.GetComponent<Playercontroler>();
        if (playercontroler != null)
        {
            playercontroler.ChangeDecel(slideDeceleration);

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        playercontroler = other.gameObject.GetComponent<Playercontroler>();
        if (playercontroler != null)
        {
            playercontroler.UnChangeDecel(slideDeceleration);
        }
    }
}
