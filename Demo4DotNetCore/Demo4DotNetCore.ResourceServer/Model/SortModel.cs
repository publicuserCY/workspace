namespace Demo4DotNetCore.ResourceServer.Model
{
    /// <summary>
    /// 可排序的Model基类
    /// </summary>
    public class SortModel : BaseModel
    {
        public SortModel() { }

        public SortModel(bool autoGenerateId) : base(autoGenerateId) { }

        /// <summary>
        /// 排序号
        /// </summary>
        public virtual int SortNumber { get; set; }
    }
}
