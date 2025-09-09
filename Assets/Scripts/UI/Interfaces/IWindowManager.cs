namespace SnakeGame.UI
{
    public interface IWindowManager
    {
        void OpenWindow(string windowId, object windowParam = default);
        void CloseWindow(string windowId);
    }
}