using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeCardMachine : CardMachine
{
    public override void Open()
    {
        animator.Play("OpenIdleGrape");
    }

    public override void Close()
    {
        animator.Play("CloseIdleGrape");
    }
}
