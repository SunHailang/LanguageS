using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    private void Update()
    {
        float checkDistance = 1.2f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkDistance, 1 << 8);
        EventManager<Events>.Instance.TriggerEvent(Events.PickType, colliders);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            switch (other.tag)
            {
                case "Mushroom":
                    ActionEvent[] actions = other.gameObject.GetComponents<ActionEvent>();
                    EventManager<Events>.Instance.TriggerEvent(Events.HurtType, actions);
                    Destroy(other.gameObject);
                    break;
                case "Enemy":
                    // death
                    PlayerData.Instance.SetPlayerData(ReplyType.Blood, -PlayerData.Instance.playerMaxBlood);
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }
}
