using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionActionEvent : ActionEvent
{

    public void OnDestroy()
    {
        GameObject obj = Instantiate(ResourcesManager.LoadParticle<GameObject>("Explosion"), transform.parent);
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        Destroy(obj, 1.5f);
    }
}
