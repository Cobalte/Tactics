using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Op_Base : ScriptableObject
{
    public abstract void Resolve(OperationContext context);
}
