using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CardMode
{
    IDLE ,
    SELECTED ,
    MATCHED

}

public class CardMachine : MonoBehaviour
{


    Animator animator;
    public Button clickable;
    public CardMode current;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        current = CardMode.IDLE;
    }


    void close()
    {
        animator.Play("CloseIdle");
    }

    void open()
    {
        animator.Play("OpenIdle");
    }


    public void ChangeState(CardMode next)
    {
        if(current == CardMode.IDLE && next == CardMode.SELECTED)
        {
            open();
            current = CardMode.SELECTED;
        }
        else if (current == CardMode.SELECTED && next == CardMode.IDLE)
        {
            StartCoroutine(closeRoutine());
            
        }
            
        else if (current == CardMode.SELECTED && next == CardMode.MATCHED)
        {
            StartCoroutine(matchRoutine());
        }

        

    }


    IEnumerator closeRoutine()
    {
        current = CardMode.IDLE;
        yield return new WaitForSeconds(1f);
        close();
    }

    IEnumerator matchRoutine()
    {
        yield return new WaitForSeconds(1f);
        current = CardMode.MATCHED;
        this.gameObject.SetActive(false);
    }

    
}
