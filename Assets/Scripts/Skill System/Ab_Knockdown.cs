using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Knockdown : Ab_Transfer {

    void Awake()
    {
        base.Awake();
        Ultimate = true;
    }

    protected override void TransferProperty()
    {
        base.TransferProperty();
        //Trigger Energy blast
    }
}
