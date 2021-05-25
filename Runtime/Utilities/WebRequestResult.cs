namespace DreamBuilders
{
    [System.Serializable]
    public class WebRequestResult<T>
    {
        #region Fields
        public T Result = default;
        public string Error = string.Empty;
        public bool Succeeded = false;
        public bool IsComplete = false;
        #endregion

        #region Methods
        public WebRequestResult() { }
        public WebRequestResult(string error) : this() => this.Error = error;
        public WebRequestResult(T result) : this()
        {
            this.Result = result;
            this.Succeeded = true;
        }
        public WebRequestResult(T result, string error) : this(result)
        {
            this.Error = error;
            this.Succeeded = false;
        }
        #endregion
    }
}