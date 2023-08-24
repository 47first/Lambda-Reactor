namespace Runtime
{
    public interface IGameView
    {
        public void HideDropdown();
        public void SetObserver<T>(T observer);
        public void ResetObserver();
        public void SetOptions();
    }
}
