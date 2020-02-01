using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCore : MonoBehaviour
{
    public static PartCore Instance;
    public Rigidbody rb;
    public Transform axelRightFront, axelRightBack, axelLeftFront, axelLeftBack;
    public List<PartAxelOut> leftAxels;
    public List<PartAxelOut> rightAxels;
    public float speed;
    bool left, right;
    void Start()
    {
        Instance = this;
    }
    public void LeftTriggerDown()
    {
        left = true;
    }
    public void LeftTriggerUp()
    {
        left = false;
    }
    public void RightTriggerDown()
    {
        right = true;
    }
    public void RightTriggerUp()
    {
        right = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(left){
            axelLeftBack.Rotate(new Vector3(-speed * Time.deltaTime, 0, 0));
            axelLeftFront.Rotate(new Vector3(-speed * Time.deltaTime, 0, 0));
            foreach(PartAxelOut pao in leftAxels)
            {
                pao.control = speed;
            }
        }
        else
        {
            foreach(PartAxelOut pao in leftAxels)
            {
                pao.control = 0;
            }
        }
        if(right){
            axelRightBack.Rotate(new Vector3(-speed * Time.deltaTime, 0, 0));
            axelRightFront.Rotate(new Vector3(-speed * Time.deltaTime, 0, 0));
            foreach(PartAxelOut pao in rightAxels)
            {
                pao.control = speed;
            }
        }
        else
        {
            foreach(PartAxelOut pao in rightAxels)
            {
                pao.control = 0;
            }
        }
    }
}
