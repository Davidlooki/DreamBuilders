using System;

namespace DreamBuilders
{
    [System.Serializable]
    public struct WebRequestResult<T> : IDisposable
    {
        #region Fields

        public T Result;
        public string Error;
        public bool Succeeded;
        public bool IsComplete;

        #endregion

        #region Methods

        public WebRequestResult(string error) : this() => Error = error;
        public WebRequestResult(T result) : this() => (Result, Succeeded) = (result, true);

        public WebRequestResult(T result, string error) : this(result) => (Error, Succeeded) = (error, false);

        public void Dispose()
        {
        }
        #endregion

    }
}