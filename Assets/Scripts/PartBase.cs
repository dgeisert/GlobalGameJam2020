using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBase : MonoBehaviour
{
    public float control;
    public Rigidbody rb;
    public List<PartBase> children;
    public bool leftSide;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PartUpdate();
    }

    public virtual void PartUpdate()
    {
        if(children.Count == 0)
        {
            return;
        }
        foreach(PartBase child in children)
        {
            child.control = control;
        }
    }
    public void OnSnap(GameObject go)
    {
        PartBase pb = go.GetComponent<PartBase>();
        if(pb == null)
        {
            return;
        }
        children.Add(pb);
        pb.leftSide = leftSide;
    }
    public void OnUnsnap(GameObject go)
    {
        PartBase pb = go.GetComponent<PartBase>();
        if(pb == null)
        {
            return;
        }
        children.Remove(pb);
    }
}
