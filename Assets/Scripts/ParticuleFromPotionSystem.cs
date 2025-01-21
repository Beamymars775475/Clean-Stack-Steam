using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticuleFromPotionSystem : MonoBehaviour
{
    public GameObject particuleObject; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ParticuleInAnimation(float cooldown, Vector3 itemTargetPos)
    {
        GameObject particules = Instantiate(particuleObject, itemTargetPos, Quaternion.identity);
        yield return new WaitForSeconds(cooldown);
        Destroy(particules);
    }
}
