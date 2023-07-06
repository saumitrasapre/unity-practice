using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IAgent, IHittable
{
    [SerializeField]
    private int maxHealth;
    private int health;
    public int Health { get=>health; set
        {
            health = Mathf.Clamp(value,0,maxHealth);
            uiHealth.UpdateUI(health);
        } }
    private bool isDead = false;

    [field:SerializeField]
    public UIHealth uiHealth { get; set; }

    [field:SerializeField]
    public UnityEvent OnDie { get; set; }
    [field:SerializeField]
    public UnityEvent OnGetHit { get; set; }

    private void Start()
    {
        Health = maxHealth;
        uiHealth.Initialize(Health);
    }

    public void GetHit(int damage, GameObject damageDealer)
    {
        if (isDead == false)
        {
            Health -= damage;
            if (OnGetHit != null)
            {
                OnGetHit.Invoke();
            }
            if (Health <= 0)
            {
                isDead = true;
                if (OnDie != null)
                {
                    OnDie.Invoke();
                }
            }
        }
       
    }

    public void DisableInput()
    {
        GameInput.Instance.DisableGameInput();
    }

}
