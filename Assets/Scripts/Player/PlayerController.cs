using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace HordeEvolution
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] float speed=6f;
        CharacterController cc;
        Vector2 mobileInput;
        void Awake()=>cc=GetComponent<CharacterController>();
        public void SetMobileInput(Vector2 v)=>mobileInput=Vector2.ClampMagnitude(v,1);

        void Update()
        {
            Vector2 input=mobileInput;
#if ENABLE_INPUT_SYSTEM
            if(Keyboard.current!=null) {
                input.x += (Keyboard.current.dKey.isPressed?1:0)-(Keyboard.current.aKey.isPressed?1:0);
                input.y += (Keyboard.current.wKey.isPressed?1:0)-(Keyboard.current.sKey.isPressed?1:0);
            }
#endif
            input=Vector2.ClampMagnitude(input,1);
            var dir=new Vector3(input.x,0,input.y);
            cc.Move(dir*speed*Time.deltaTime);
            if(dir.sqrMagnitude>.01f) transform.forward=Vector3.Slerp(transform.forward,dir,15*Time.deltaTime);
        }
    }
}
