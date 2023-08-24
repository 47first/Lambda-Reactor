using System.Collections.Generic;

namespace Runtime
{
    public interface IGameView
    {
        public void SetObserver<T>(T observer);
        public void ResetObserver();
        public void ShowDropdown();
        public void HideDropdown();
        public int GetDropdownValue();
        public void ShowResult();
        public void SetOptions(IEnumerable<string> strings);
    }
}
