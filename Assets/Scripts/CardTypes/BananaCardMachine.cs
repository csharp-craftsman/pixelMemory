using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCardMachine : CardMachine
{
    public override void Open()
    {
        animator.Play("OpenIdleBanana");
    }

    public override void Close()
    {
        animator.Play("CloseIdleBanana");
    }

}
