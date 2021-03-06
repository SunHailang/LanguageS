using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TerrainController : MonoBehaviour
{
    [SerializeField]
    private Transform m_mushroomsParent = null;

    private Transform m_mushroomsPrefab;

    private Transform m_flowersPrefab;

    private Transform m_flowersLowPrefab;




    private IEnumerator ShowWebCamTexture()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("webcam found");
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length > 0)
            {
                WebCamTexture camTexture = new WebCamTexture(1920, 1080, 60);
                camTexture.deviceName = devices[0].name;

                //m_camMaterial.mainTexture = camTexture;

                camTexture.Play();
            }
            else
            {
                Debug.Log("No WebCamera.");
            }
        }
        else
        {
            Debug.Log("webcam not found");
        }

        //Microphone.devices
    }

    private void Start()
    {
        if (m_mushroomsPrefab == null) m_mushroomsPrefab = ResourcesManager.LoadEnemys<Transform>("mushrooms");
        for (float i = -49; i < 49; i += 10.5f)
        {
            for (float j = -40; j < 40; j += 8.8f)
            {
                Transform trans = Instantiate(m_mushroomsPrefab, m_mushroomsParent);
                float x = UnityEngine.Random.Range(0.6f, 1.0f) * i;
                float z = UnityEngine.Random.Range(0.7f, 1.0f) * j;
                trans.localPosition = new Vector3(x, 0, z);
                trans.gameObject.AddComponent<ExplosionActionEvent>().SetAction(SkillActionType.ExplosionType, -125f);
                trans.gameObject.name = $"mushroom{i + j}";
            }
        }
        if (m_flowersPrefab == null) m_flowersPrefab = ResourcesManager.LoadEnemys<Transform>("flowers");
        for (float i = -49; i < 49; i += 8.5f)
        {
            for (float j = -40; j < 40; j += 5.8f)
            {
                Transform trans = Instantiate(m_flowersPrefab, m_mushroomsParent);
                float x = UnityEngine.Random.Range(0.6f, 1.0f) * i;
                float z = UnityEngine.Random.Range(0.7f, 1.0f) * j;
                trans.localPosition = new Vector3(x, 0, z);
                PickActionEvent action = trans.gameObject.AddComponent<PickActionEvent>();
                action.SetAction(SkillActionType.PickType, 105f);
                action.SetReplyType(ReplyType.Blood);
                trans.gameObject.name = $"flowers{i + j}";
            }
        }
        if (m_flowersLowPrefab == null) m_flowersLowPrefab = ResourcesManager.LoadEnemys<Transform>("flowersLow");
        for (float i = -49; i < 49; i += 5.5f)
        {
            for (float j = -40; j < 40; j += 3.8f)
            {
                Transform trans = Instantiate(m_flowersLowPrefab, m_mushroomsParent);
                float x = UnityEngine.Random.Range(0.5f, 1.0f) * i;
                float z = UnityEngine.Random.Range(0.5f, 1.0f) * j;
                trans.localPosition = new Vector3(x, 0, z);
                PickActionEvent action = trans.gameObject.AddComponent<PickActionEvent>();
                action.SetAction(SkillActionType.PickType, 65f);
                action.SetReplyType(ReplyType.Magic);
                trans.gameObject.name = $"flowersLow{i + j}";
            }
        }

    }

}
