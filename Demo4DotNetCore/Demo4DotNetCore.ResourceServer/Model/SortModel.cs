namespace Demo4DotNetCore.ResourceServer.Model
{
    /// <summary>
    /// 可排序的Model基类
    /// </summary>
    public abstract class SortModel<T> : BaseModel<T>
    {

        public SortModel(bool autoGenerateId) : base() { }

        /// <summary>
        /// 排序号
        /// </summary>
        public virtual int SortNumber { get; set; }
    }
}
