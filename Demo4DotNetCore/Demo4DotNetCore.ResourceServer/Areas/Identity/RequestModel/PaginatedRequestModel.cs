namespace Demo4DotNetCore.AuthorizationServer.RequestModel
{
    public class PaginatedRequestModel : BaseRequestModel
    {
        public PaginatedRequestModel()
        {
            pageSize = 10;
        }

        private int pageIndex = 1;
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value > 0 && value <= int.MaxValue ? value : 1; }
        }

        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 0 && value <= int.MaxValue ? value : pageSize; }
        }

        private string orderBy = "Id";
        public string OrderBy
        {
            get { return orderBy; }
            set
            {
                orderBy = string.IsNullOrEmpty(value) ? "Id" : value;
            }
        }

        private string direction = "ASC";
        public string Direction
        {
            get { return direction; }
            set
            {
                if (string.IsNullOrEmpty(value)) { return; }
                direction = value.Trim().ToUpper().Equals("ASC") ? "ASC" : "DESC";
            }
        }
        public string SortExpression
        {
            get
            {
                if (string.IsNullOrEmpty(OrderBy))
                {
                    return null;
                }
                else
                {
                    return OrderBy.Trim() + " " + Direction;
                }
            }
        }
    }
}