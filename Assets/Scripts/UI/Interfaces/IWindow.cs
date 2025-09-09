namespace SnakeGame.UI
{
    public interface IWindow
    {
        public void Open();

        public void SetParam(object param);

        public void HandleCloseClick();
    }
}