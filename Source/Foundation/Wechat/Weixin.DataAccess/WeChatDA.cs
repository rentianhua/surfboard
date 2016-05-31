#region

using System.Diagnostics;
using Cedar.Core.Logging;
using Cedar.Foundation.WeChat.Entities.WeChat;

#endregion

namespace Cedar.Foundation.WeChat.DataAccess
{
    /// <summary>
    /// </summary>
    public class WeChatDA : WeChatDataAccessBase
    {
        /// <summary>
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public bool IsWechatFriendExists(string appid, string openid)
        {
            const string sql = "select 1 from wechat_friend where accountid=@appid and openid=@openid;";
            var result = Helper.ExecuteScalar<int>(sql, new {appid, openid});
            return result == 1;
        }

        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CreaeWechatFriend(FriendModel model)
        {
            var strSql = (@"INSERT INTO wechat_friend
                  (`innerid`,`accountid`,`nickname`,`photo`,`openid`,`remarkname`,`area`,`sex`,`isdel`,`subscribe_time`,`subscribe`,country,province,city,`createdtime`)
                  VALUES
                  (@innerid,@accountid,@nickname,@photo,@openid,@remarkname,@area,@sex,@isdel,@subscribetime,@subscribe,@country,@province,@city,@createdtime);");

            return Helper.Execute(strSql, model) > 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateWechatFriend(FriendModel model)
        {
            var strSql = @"UPDATE wechat_friend
                                        SET `nickname` = @nickname, `photo` = @photo, `remarkname` = @remarkname,`area` = @area,
                                         `sex` = @sex, `isdel` = @isdel, `subscribe_time` = @subscribetime,
                                          country=@country,province=@province,city=@city,
                                         `subscribe` = @subscribe
                                          WHERE `openid` = @openid and `accountid` = @accountid ";

            return Helper.Execute(strSql, model) > 0;
        }

        /// <summary>
        /// 更新粉丝
        /// </summary>
        /// <param name="scenestr"></param>
        /// <param name="openid"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public bool UpdateWechatFriendQrScene(string openid,string scenestr,string appid)
        {
            var strSql = @"UPDATE wechat_friend SET `scenestr` = @scenestr WHERE `openid` = @openid and `accountid` = @accountid ";

            return Helper.Execute(strSql, new
            {
                scenestr,
                openid,
                accountid = appid
            }) > 0;
        }

        /// <summary>
        /// 更新粉丝状态（取消关注）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateWechatFriendUnSubscribe(FriendModel model)
        {
            LoggerFactories.CreateLogger().Write("Accountid:" + model.Accountid, TraceEventType.Information);
            LoggerFactories.CreateLogger().Write("OPENID:" + model.OPENID, TraceEventType.Information);
            LoggerFactories.CreateLogger().Write("SubscribeTime:" + model.SubscribeTime, TraceEventType.Information);
            LoggerFactories.CreateLogger().Write("Subscribe:" + model.Subscribe, TraceEventType.Information);
            var strSql = @"UPDATE wechat_friend SET `subscribe_time` = @subscribetime,`subscribe` = @subscribe WHERE `openid` = @openid and `accountid` = @accountid";
            return Helper.Execute(strSql, new
            {
                subscribe_time = model.SubscribeTime,
                subscribe = model.Subscribe,
                openid = model.OPENID,
                accountid = model.Accountid
            }) > 0;
        }
    }
}