using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleCardMachine : CardMachine
{
    public override void Open()
    {
        animator.Play("OpenIdleApple");
    }

    public override void Close()
    {
        animator.Play("CloseIdleApple");
    }
}
