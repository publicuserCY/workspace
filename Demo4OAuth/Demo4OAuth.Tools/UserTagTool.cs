using Newtonsoft.Json;
using System;

namespace Demo4OAuth.Tools
{
    /// <summary>
    /// 专门用来操作UserTag的类
    /// </summary>
    public class UserTagTool
    {
        public static UserTag GetUserTag(ApplicationUser user)
        {
            if (user == null) { throw new ArgumentNullException("user"); }
            UserTag tag = null;
            try
            {
                tag = JsonConvert.DeserializeObject<UserTag>(user.Tag);
            }
            catch (Exception)
            {
                tag = null;
            }
            tag = tag ?? new UserTag();
            return tag;
        }
    }
}