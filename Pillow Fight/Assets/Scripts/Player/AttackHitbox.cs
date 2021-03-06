﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class AttackHitbox : MonoBehaviour
{
    private ControllerPlayer m_Player;

    public void SetPlayer(ControllerPlayer player)
    {
        if (!m_Player)
            m_Player = player;
        else
            Debug.Log(gameObject.name + " already has a player, a new assignment was attempted");
    }

    public ControllerPlayer GetPlayer()
    {
        return m_Player;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (m_Player)
        {
            if (col.gameObject != m_Player.gameObject)
            {
                if (col.gameObject.GetComponent<ControllerPlayer>())
                {
                    col.gameObject.GetComponent<ControllerPlayer>().Kill();
                    m_Player.AddScore(1);
                }
            }
        }
    }
}
