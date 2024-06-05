using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jusoft.DingtalkStream.Robot
{
    /// <summary>
    /// 钉钉机器人 Webhook 工具类
    /// </summary>
    public static class DingtalkRobotWebhookUtilites
    {
        #region 创建Webhook 消息内容的辅助方法
        /// <summary>
        /// 写入at指定人的json
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="atMobiles"></param>
        /// <param name="atUserIds"></param>
        static void WriteAtInfo(this Utf8JsonWriter writer , string[] atMobiles = null , string[] atUserIds = null)
        {
            var hasAtMobiles = atMobiles != null && atMobiles.Length > 0;
            var hasAtUserIds = atUserIds != null && atUserIds.Length > 0;

            if (hasAtMobiles || hasAtUserIds)
            {
                writer.WritePropertyName("at");
                writer.WriteStartObject();

                if (hasAtMobiles)
                {
                    writer.WritePropertyName("atMobiles");
                    writer.WriteStartArray();
                    foreach (var item in atMobiles)
                    {
                        writer.WriteStringValue(item);
                    }
                    writer.WriteEndArray();
                }
                if (hasAtUserIds)
                {
                    writer.WritePropertyName("atUserIds");
                    writer.WriteStartArray();
                    foreach (var item in atUserIds)
                    {
                        writer.WriteStringValue(item);
                    }
                    writer.WriteEndArray();
                }

                writer.WriteEndObject();
            }
        }
        /// <summary>
        /// 写入at所有人的json
        /// </summary>
        /// <param name="writer"></param>
        static void WriteAtAll(this Utf8JsonWriter writer)
        {
            writer.WritePropertyName("at");
            writer.WriteStartObject();
            writer.WriteBoolean("isAtAll" , true);
            writer.WriteEndObject();
        }
        /// <summary>
        /// 创建 Webhook 的 text 消息json字符串
        /// </summary>
        /// <param name="content">发送的内容</param>
        /// <param name="atAll">是否@所有人</param>
        /// <returns></returns>
        public static async Task<string> CreateTextMessage(string content , bool atAll)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();
            writer.WriteString("msgtype" , "text");

            writer.WritePropertyName("text");
            writer.WriteStartObject();
            writer.WriteString("content" , content);
            writer.WriteEndObject();

            if (atAll)
            {
                writer.WriteAtAll();
            }

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0 , SeekOrigin.Begin);

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        /// <summary>
        /// 创建 Webhook 的 text 消息json 字符串
        /// </summary>
        /// <param name="content">发送的内容</param>
        /// <param name="atMobiles">@的手机号集合</param>
        /// <param name="atUserIds">@的userid集合</param>
        /// <returns></returns>
        public static async Task<string> CreateTextMessage(string content , string[] atMobiles = null , string[] atUserIds = null)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("msgtype" , "text");

            writer.WritePropertyName("text");
            writer.WriteStartObject();
            writer.WriteString("content" , content);
            writer.WriteEndObject();

            writer.WriteAtInfo(atMobiles , atUserIds);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0 , SeekOrigin.Begin);

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        /// <summary>
        /// 创建 Webhook 的 link消息json字符串
        /// </summary>
        /// <param name="title">消息标题。</param>
        /// <param name="text">消息内容。如果太长只会部分展示。</param>
        /// <param name="picUrl">点击消息跳转的URL。</param>
        /// <param name="messageUrl">图片URL。</param>
        /// <returns></returns>
        public static async Task<string> CreateLinkMessage(string title , string text , string picUrl , string messageUrl)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();
            writer.WriteString("msgtype" , "link");

            writer.WritePropertyName("link");
            writer.WriteStartObject();
            writer.WriteString("title" , title);
            writer.WriteString("text" , text);
            writer.WriteString("picUrl" , picUrl);
            writer.WriteString("messageUrl" , messageUrl);
            writer.WriteEndObject();

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0 , SeekOrigin.Begin);

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        /// <summary>
        /// 创建 Webhook 的  Markdown 消息json字符串
        /// </summary>
        /// <param name="title">首屏会话透出的展示内容。</param>
        /// <param name="text">Markdown格式的消息内容。</param>
        /// <param name="atAll">是否@所有人</param>
        /// <returns></returns>
        public static async Task<string> CreateMarkdownMessage(string title , string text , bool atAll)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();
            writer.WriteString("msgtype" , "markdown");

            writer.WritePropertyName("markdown");
            writer.WriteStartObject();
            writer.WriteString("title" , title);
            writer.WriteString("text" , text);
            writer.WriteEndObject();

            if (atAll)
            {
                writer.WritePropertyName("at");
                writer.WriteStartObject();
                writer.WriteBoolean("isAtAll" , true);
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0 , SeekOrigin.Begin);

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        /// <summary>
        /// 创建 Webhook 的 Markdown 消息json字符串
        /// </summary>
        /// <param name="title">首屏会话透出的展示内容。</param>
        /// <param name="text">Markdown格式的消息内容。</param>
        /// <param name="atMobiles">@的手机号集合</param>
        /// <param name="atUserIds">@的userid集合</param>
        /// <returns></returns>
        public static async Task<string> CreateMarkdownMessage(string title , string text , string[] atMobiles = null , string[] atUserIds = null)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();
            writer.WriteString("msgtype" , "markdown");

            writer.WritePropertyName("markdown");
            writer.WriteStartObject();
            writer.WriteString("title" , title);
            writer.WriteString("text" , text);
            writer.WriteEndObject();

            writer.WriteAtInfo(atMobiles , atUserIds);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0 , SeekOrigin.Begin);

            return Encoding.UTF8.GetString(stream.ToArray());
        }
        /// <summary>
        /// 创建 Webhook 的 ActionCard消息json字符串
        /// </summary>
        /// <param name="title">首屏会话透出的展示内容。</param>
        /// <param name="text">markdown格式的消息内容。</param>
        /// <param name="orientation">按钮方向</param>
        /// <param name="btns">超过两个按钮时，orientation 参数将不再生效</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<string> CreateActionCardMessage(string title , string text , BtnOrientation orientation , params (string title, string url)[] btns)
        {
            if (btns == null || btns.Length == 0)
            {
                throw new ArgumentException($"{nameof(btns)} 不能为空" , nameof(btns));
            }
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("msgtype" , "actionCard");

            writer.WritePropertyName("actionCard");
            writer.WriteStartObject();
            writer.WriteString("title" , title);
            writer.WriteString("text" , text);

            if (btns.Length > 1)
            {
                writer.WriteString("btnOrientation" , orientation == BtnOrientation.Horizontal ? "0" : "1");
                writer.WritePropertyName("btns");
                writer.WriteStartArray();
                foreach (var btn in btns)
                {
                    writer.WriteStartObject();
                    writer.WriteString("title" , btn.title);
                    writer.WriteString("actionURL" , btn.url);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteString("singleTitle" , btns[0].title);
                writer.WriteString("singleURL" , btns[0].url);
            }

            writer.WriteEndObject();

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0 , SeekOrigin.Begin);

            return Encoding.UTF8.GetString(stream.ToArray());
        }
        /// <summary>
        /// 创建 Webhook 的 ActionCard消息json字符串
        /// </summary>
        /// <param name="title">首屏会话透出的展示内容。</param>
        /// <param name="text">markdown格式的消息内容。</param>
        /// <param name="btns">横向排列的按钮配置</param>
        /// <returns></returns>
        public static Task<string> CreateActionCardMessage(string title , string text , params (string title, string url)[] btns)
        {
            return CreateActionCardMessage(title , text , BtnOrientation.Horizontal , btns);
        }
        /// <summary>
        /// 创建 Webhook 的 FeedCard消息json字符串
        /// </summary>
        /// <param name="links">单条信息</param>
        /// <returns></returns>
        public static async Task<string> CreateFeedCardMessage(params (string title, string messageUrl, string picUrl)[] links)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();
            writer.WriteString("msgtype" , "feedCard");

            writer.WritePropertyName("feedCard");
            writer.WriteStartObject();
            writer.WritePropertyName("links");
            writer.WriteStartArray();
            foreach (var link in links)
            {
                writer.WriteStartObject();
                writer.WriteString("title" , link.title);
                writer.WriteString("messageURL" , link.messageUrl);
                writer.WriteString("picURL" , link.picUrl);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0 , SeekOrigin.Begin);

            return Encoding.UTF8.GetString(stream.ToArray());
        }
        #endregion

        // HTTP 请求通道
        readonly static HttpClient BackChannel = new HttpClient();

        #region 创建Webhook发送消息的辅助方法

        /// <summary>
        /// 给指定 webhook 地址 发送JSON消息
        /// </summary>
        /// <param name="webhook">webhook地址</param>
        /// <param name="jsonContent">JSON 内容</param>
        /// <returns></returns>
        static async Task<bool> SendMessage(string webhook , string jsonContent)
        {
            var response = await BackChannel.PostAsync(webhook , new StringContent(jsonContent , Encoding.UTF8 , "application/json"));
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// 使用给定 webhook 地址发送 text 消息
        /// </summary>
        /// <param name="webhook">webhook 地址 </param>
        /// <param name="content">发送的内容</param>
        /// <param name="isAtAll">是否@所有人</param>
        /// <returns></returns>
        public static async Task<bool> SendTextMessage(string webhook , string content , bool isAtAll)
        {
            return await SendMessage(webhook , await CreateTextMessage(content , isAtAll));
        }
        /// <summary>
        /// 使用给定 webhook 地址发送 text 消息
        /// </summary>
        /// <param name="webhook">webhook 地址 </param>
        /// <param name="content">发送的内容</param>
        /// <param name="atMobiles">@的手机号集合</param>
        /// <param name="atUserIds">@的userid集合</param>
        /// <returns></returns>
        public static async Task<bool> SendTextMessage(string webhook , string content , string[] atMobiles = null , string[] atUserIds = null)
        {
            return await SendMessage(webhook , await CreateTextMessage(content , atMobiles , atUserIds));
        }
        /// <summary>
        /// 使用给定 webhook 地址发送 link 消息
        /// </summary>
        /// <param name="webhook">webhook 地址 </param>
        /// <param name="title">消息标题。</param>
        /// <param name="text">消息内容。如果太长只会部分展示。</param>
        /// <param name="picUrl">点击消息跳转的URL。</param>
        /// <param name="messageUrl">图片URL。</param>
        /// <returns></returns>
        public static async Task<bool> SendLinkMessage(string webhook , string title , string text , string messageUrl , string picUrl = null)
        {
            return await SendMessage(webhook , await CreateLinkMessage(title , text , messageUrl , picUrl));
        }
        /// <summary>
        /// 使用给定 webhook 地址发送 Markdown 消息
        /// </summary>
        /// <param name="webhook">webhook 地址 </param>
        /// <param name="title">首屏会话透出的展示内容。</param>
        /// <param name="text">Markdown格式的消息内容。</param>
        /// <param name="atAll">是否@所有人</param>
        /// <returns></returns>
        public static async Task<bool> SendMarkdownMessage(string webhook , string title , string text , bool atAll)
        {
            return await SendMessage(webhook , await CreateMarkdownMessage(title , text , atAll));
        }
        /// <summary>
        /// 使用给定 webhook 地址发送 Markdown 消息
        /// </summary>
        /// <param name="webhook">webhook 地址 </param>
        /// <param name="title">首屏会话透出的展示内容。</param>
        /// <param name="text">Markdown格式的消息内容。</param>
        /// <param name="atMobiles">@的手机号集合</param>
        /// <param name="atUserIds">@的userid集合</param>
        /// <returns></returns>
        public static async Task<bool> SendMarkdownMessage(string webhook , string title , string text , string[] atMobiles = null , string[] atUserIds = null)
        {
            return await SendMessage(webhook , await CreateMarkdownMessage(title , text , atMobiles , atUserIds));
        }
        /// <summary>
        /// 使用给定 webhook 地址发送 ActionCard 消息
        /// </summary>
        /// <param name="webhook">webhook 地址 </param>
        /// <param name="title">首屏会话透出的展示内容。</param>
        /// <param name="text">markdown格式的消息内容。</param>
        /// <param name="orientation">按钮方向</param>
        /// <param name="btns">超过两个按钮时，orientation 参数将不再生效</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<bool> SendActionCardMessage(string webhook , string title , string text , BtnOrientation orientation , params (string title, string url)[] btns)
        {
            return await SendMessage(webhook , await CreateActionCardMessage(title , text , orientation , btns));
        }
        /// <summary>
        /// 使用给定 webhook 地址发送 ActionCard 消息
        /// </summary>
        /// <param name="webhook">webhook 地址 </param>
        /// <param name="title">首屏会话透出的展示内容。</param>
        /// <param name="text">markdown格式的消息内容。</param>
        /// <param name="btns">横向排列的按钮配置</param>
        /// <returns></returns>
        public static async Task<bool> SendActionCardMessage(string webhook , string title , string text , params (string title, string url)[] btns)
        {
            return await SendMessage(webhook , await CreateActionCardMessage(title , text , btns));
        }
        /// <summary>
        /// 使用给定 webhook 地址发送 FeedCard 消息
        /// </summary>
        /// <param name="webhook">webhook 地址 </param>
        /// <param name="links">单条信息</param>
        /// <returns></returns>
        public static async Task<bool> SendFeedCardMessage(string webhook , params (string title, string messageUrl, string picUrl)[] links)
        {
            return await SendMessage(webhook , await CreateFeedCardMessage(links));
        }
        #endregion

    }
}
