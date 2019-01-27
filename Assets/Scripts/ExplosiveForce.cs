using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveForce : MonoBehaviour
{
    [SerializeField] float forceApplied = 500f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision col)
    {

        int layer = col.gameObject.layer;
        
        if(layer == 10 || layer == 11) // player
        {
            MinionController mc = col.gameObject.GetComponent<MinionController>();
            if (mc != null)
            {
                mc.ragdollUntilTime = Time.time + 1.0f;
            }
            else
            {
                PlayerCharacterController pcc = col.gameObject.GetComponent<PlayerCharacterController>();
                if (pcc != null)
                {
                    pcc.ragdollUntilTime = Time.time + 1.0f;

                }
            }
        }

    }
}
