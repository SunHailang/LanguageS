using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReplyType
{
    Blood,
    Magic,
}

public class PickActionEvent : ActionEvent
{

    private ReplyType m_replyType = ReplyType.Blood;

    public ReplyType replyType { get { return m_replyType; } }


    public void SetReplyType(ReplyType type)
    {
        m_replyType = type;
    }
}
