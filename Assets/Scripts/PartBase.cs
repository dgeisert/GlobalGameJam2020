using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBase : MonoBehaviour
{
    public float control;
    public PartBase child;
    public bool leftSide;
    void Update()
    {
        PartUpdate();
    }

    public virtual void PartUpdate()
    {
        if(child == null)
        {
            return;
        }
        child.control = control;
    }
    public void OnSnap(GameObject go)
    {
        PartBase pb = go.GetComponent<PartBase>();
        if(pb == null)
        {
            return;
        }
        child = pb;
        child.leftSide = leftSide;
    }
    public void OnUnsnap(GameObject go)
    {
        child = null;
    }
}
