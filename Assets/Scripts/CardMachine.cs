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


    public Animator animator;
    public Button clickable;
    public CardMode current;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        current = CardMode.IDLE;
    }


    public virtual void Close()
    {
        animator.Play("CloseIdle");
    }

    public virtual void Open()
    {
        animator.Play("OpenIdle");
    }


    public void ChangeState(CardMode next)
    {
        if(current == CardMode.IDLE && next == CardMode.SELECTED)
        {
            Open();
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
        yield return new WaitForSeconds(0.5f);
        current = CardMode.IDLE;
        Close();
    }

    IEnumerator matchRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        current = CardMode.MATCHED;
        this.gameObject.SetActive(false);
    }

    
}
