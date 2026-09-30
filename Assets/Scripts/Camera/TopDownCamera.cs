using UnityEngine;

namespace HordeEvolution
{
    public sealed class TopDownCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset=new(0,12,-10);
        [SerializeField] float smooth=12;
        void LateUpdate()
        {
            if(!target)return;
            transform.position=Vector3.Lerp(transform.position,target.position+offset,1-Mathf.Exp(-smooth*Time.deltaTime));
            transform.rotation=Quaternion.Euler(50,0,0);
        }
    }
}
