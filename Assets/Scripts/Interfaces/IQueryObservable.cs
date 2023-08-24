using System;

namespace Runtime
{
    public interface IQueryObservable
    {
        public void Activate(Action next);
    }
}
