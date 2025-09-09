using UnityEngine;

namespace SnakeGame.UI
{
    public abstract class AbstractWindow : MonoBehaviour, IWindow
    {
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void SetParam(object param)
        {
            
        }

        public virtual void HandleCloseClick()
        {
            gameObject.SetActive(false);
        }
    }
}
