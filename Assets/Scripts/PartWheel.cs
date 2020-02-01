using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartWheel : PartBase
{
    public Transform wheel;
    public override void PartUpdate()
    {
        wheel.Rotate(new Vector3(0, (leftSide ? -1 : 1) * control * Time.deltaTime, 0));
        PartCore.Instance.rb.AddForce(- PartCore.Instance.transform.forward * control / 10 * Time.deltaTime, ForceMode.VelocityChange);
        PartCore.Instance.transform.Rotate(0, (leftSide ? 1 : -1) * control / 2 * Time.deltaTime * Vector3.Distance(transform.position, PartCore.Instance.transform.position), 0);
    }
}
