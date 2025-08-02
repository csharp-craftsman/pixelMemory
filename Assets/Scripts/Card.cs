using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour , IPointerClickHandler
{

    public bool IsSelected = false; 
    public bool IsMatched = false;
    public bool IsDisabled = true;

    Animator anim;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!IsDisabled) Select();

    }


    public bool IsSameType(Card other)=> this.GetType().Equals(other.GetType());

    public void Select()
    {
        if (!IsSelected)
        {
            IsSelected = true;
            anim.Play("OpenIdle");
            

        }
    }

    public void Match()
    {
        StartCoroutine(matchCoroutine());
    }

    IEnumerator matchCoroutine()
    {
        IsMatched = true;
        IsSelected = false;
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);

    }



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("CloseIdle");
    }

    // Update is called once per frame
    void Update()
    {


    }
}
