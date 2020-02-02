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
    public Transform colliderHolder;
    public Dictionary<PartBase, Transform> parts = new Dictionary<PartBase, Transform>();
    public float speed;
    float left, right;
    void Start()
    {
        Instance = this;
    }
    public void LeftJoystickY(float f)
    {
        left = f;
    }
    public void Left(float f)
    {
        axelLeftBack.Rotate(new Vector3(-speed * f * Time.deltaTime * 10, 0, 0));
        axelLeftFront.Rotate(new Vector3(-speed * f * Time.deltaTime * 10, 0, 0));
        foreach(PartAxelOut pao in leftAxels)
        {
            pao.control = speed * f;
        }
    }
    public void RightJoystickY(float f)
    {
        right = -f;
    }
    public void Right(float f)
    {
        axelRightBack.Rotate(new Vector3(-speed * f * Time.deltaTime * 10, 0, 0));
        axelRightFront.Rotate(new Vector3(-speed * f * Time.deltaTime * 10, 0, 0));
        foreach(PartAxelOut pao in rightAxels)
        {
            pao.control = speed * f;
        }
    }

    void Update()
    {
        Right(right);
        Left(left);
    }

    void BuildColliders(PartBase pb)
    {
        if(pb.rb != null && !parts.ContainsKey(pb))
        {
            parts.Add(pb, GameObject.Instantiate(pb.primaryCollider.transform, pb.primaryCollider.transform.position, pb.primaryCollider.transform.rotation, colliderHolder));
            parts[pb].gameObject.layer = 11;
        }
        foreach(PartBase partBase in pb.children)
        {
            BuildColliders(partBase);
        }
    }

    public void Build()
    {
        StartCoroutine(DoBuild());
    }


    IEnumerator DoBuild()
    {
        yield return null;
        foreach(PartAxelOut pao in leftAxels)
        {
            BuildColliders(pao);
        }
        foreach(PartAxelOut pao in rightAxels)
        {
            BuildColliders(pao);
        }
    }

    public void Rebuild()
    {
        for(int i = colliderHolder.childCount - 1; i >= 0; i--)
        {
            Destroy(colliderHolder.GetChild(i).gameObject);
        }
        parts = new Dictionary<PartBase, Transform>();
        Build();
    }
}
