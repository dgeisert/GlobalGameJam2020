using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartAxel : PartBase
{
    public Transform axelTransform;
    public bool reverse = false;
    public override void PartUpdate()
    {
        axelTransform.Rotate(new Vector3(0, (leftSide ? -1 : 1) * control * Time.deltaTime, 0));
        if(children.Count == 0)
        {
            return;
        }
        foreach(PartBase child in children)
        {
            if (null != child)
            {
                child.control = control;
            }
        }
    }
}
