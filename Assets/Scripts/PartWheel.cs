using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartWheel : PartBase
{
    public Transform wheel;
    public override void PartUpdate()
    {
        if(transform.position.y < 0.2f)
        {
            wheel.Rotate(new Vector3(0, (leftSide ? -1 : 1) * control * Time.deltaTime * 10, 0));
            PartCore.Instance.rb.AddForceAtPosition(-PartCore.Instance.transform.forward * control / 10 * Time.deltaTime,
            PartCore.Instance.transform.position - Vector3.up * 0.2f, ForceMode.VelocityChange);
            PartCore.Instance.transform.Rotate(0, (leftSide ? 1 : -1) * control / 6 * Time.deltaTime * 10, 0);
        }
    }
}
