using TMPro;
using UnityEngine;

public class TopInfoNotes : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_text = null;

    public void Configure(string data)
    {
        m_text.text = data;
    }

    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }
}
