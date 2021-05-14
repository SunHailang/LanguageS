using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    private void Update()
    {
        float checkDistance = 0.85f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkDistance, 1 << 8);
        EventManager<Events>.Instance.TriggerEvent(Events.PickType, colliders);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            switch (other.tag)
            {
                case "Enemy":
                    ActionEvent[] actions = other.gameObject.GetComponents<ActionEvent>();
                    EventManager<Events>.Instance.TriggerEvent(Events.HurtType, actions);
                    Destroy(other.gameObject);
                    break;
            }
        }
    }
}
