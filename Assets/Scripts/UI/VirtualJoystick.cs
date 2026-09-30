using UnityEngine;
using UnityEngine.EventSystems;

namespace HordeEvolution
{
    public sealed class VirtualJoystick : MonoBehaviour,IPointerDownHandler,IDragHandler,IPointerUpHandler
    {
        [SerializeField] RectTransform knob;
        [SerializeField] PlayerController player;
        [SerializeField] float radius=90;
        RectTransform rect;
        void Awake()=>rect=(RectTransform)transform;
        public void OnPointerDown(PointerEventData e)=>OnDrag(e);
        public void OnDrag(PointerEventData e)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,e.position,e.pressEventCamera,out var p);
            var v=Vector2.ClampMagnitude(p/radius,1);
            knob.anchoredPosition=v*radius;
            player.SetMobileInput(v);
        }
        public void OnPointerUp(PointerEventData e){knob.anchoredPosition=Vector2.zero;player.SetMobileInput(Vector2.zero);}
    }
}
