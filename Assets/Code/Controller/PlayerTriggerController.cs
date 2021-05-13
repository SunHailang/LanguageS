using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    private void Update()
    {
        float checkDistance = 1.0f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkDistance, 1 << 8);
        EventManager<Events>.Instance.TriggerEvent(Events.PickType, colliders);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        ActionEvent action = other.gameObject.GetComponent<ActionEvent>();
        if (action != null)
        {
            switch (action.actionType)
            {
                case SkillActionType.PickType:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
