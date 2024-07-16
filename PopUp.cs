using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PopUp : MonoBehaviour
{
    public GameObject popUpBox;
    Animator myAnimator;
    TMP_Text popUpText;
    BoxCollider2D mySignCollider;


    public void Start()
    {
        mySignCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        popUpBox.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            popUpBox.SetActive(true);
        }
        else
        {
            popUpBox.SetActive(false);

        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            popUpBox.SetActive(false);
        }
    }


}
