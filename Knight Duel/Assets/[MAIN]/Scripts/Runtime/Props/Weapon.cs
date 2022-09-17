using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightDuel
{
    public class Weapon : MonoBehaviour
    {
        [Header("=============== PHYSICS DATA ==============")]
        [SerializeField] private CapsuleCollider col;
       // public Collider Col => col;
        [SerializeField] int power = 5;
        public int Power => power;
        [SerializeField] float hitInterval = .35f;

        [Header("================= WEAPON STATE================")]
        [SerializeField] bool hasCollided;

        //[Header("================= AUDIO DATA =================")]


        Character owner;

        public void Init(Character o, int p)
        {
            owner = o;
            power = p;
            owner.UpdatePowerUI(p);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //return;
            if (collision.transform.CompareTag("Ragdoll") && !hasCollided && collision.transform.GetComponentInParent<Character>() != owner)
            {
                //Debug.Log(collision.transform.name);
                collision.transform.GetComponentInParent<Character>().OnCollision(power, collision);
                GameManager.instance.PlayFX(0, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
                GameManager.instance.ShakeCam();
                StartCoroutine(AwaitNextCollision());
            }
            
        }


        public void Activate(bool on)
        {
            col.enabled = on;
        }

        public void UpdatePower(int p)
        {
            power = p;
            owner.UpdatePowerUI(p);
        }

        IEnumerator AwaitNextCollision()
        {
            hasCollided = true;

            //Debug.Log("Is col");
            yield return new WaitForSeconds(hitInterval);

            hasCollided = false;
        }
    }
}
