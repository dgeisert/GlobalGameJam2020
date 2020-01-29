using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : HolsteredObject
{
    public bool hitScan = false;
    public GameObject muzzleFlash;
    public GameObject bullet;
    public GameObject hitScanImpact;
    public float hitScanDistance = 100;
    public Transform spawnPoint;
    LineRenderer lineRenderer;

    void Start()
    {
        if (hitScan)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }

    public void PullTrigger()
    {
        if (hitScan)
        {
            Ray ray = new Ray(spawnPoint.position, spawnPoint.forward);
            Vector3 hit = Fire(ray);
            Vector3 endPos = spawnPoint.position + spawnPoint.forward * hitScanDistance;
            if (hit != null)
            {
                endPos = hit;
            }
            if (lineRenderer)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, spawnPoint.position);
                lineRenderer.SetPosition(1, endPos);
                StartCoroutine(HideLine());
            }
        }
        else
        {
            /*
            GameObject go = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
            Projectile p = go.GetComponent<Projectile>();
            if (p != null)
            {
                p.player = true;
            }
            */
        }
    }

    public virtual Vector3 Fire(Ray ray)
    {
        return Vector3.zero;
    }

    IEnumerator HideLine()
    {
        yield return null;
        yield return null;
        lineRenderer.enabled = false;
    }
}