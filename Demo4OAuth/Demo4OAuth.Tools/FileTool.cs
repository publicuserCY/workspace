namespace Demo4OAuth.Tools
{
    public class FileTool
    {
        public string GetFileMD5String(string filePath)
        {
            return new DBTek.Crypto.MD5_Hsr().HashFile(filePath);
        }
    }
}
