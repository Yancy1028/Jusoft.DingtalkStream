using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Jusoft.DingtalkStream.Core;

namespace Jusoft.DingtalkStream.Robot
{
    /// <summary>
    /// Dingtalk Stream Robot 的一些辅助方法
    /// </summary>
    public static class DingtalkStreamRobotUtilities
    {
        #region 辅助创建应答消息的方法

        #region 创建机器人回复消息的辅助方法
        /// <summary>
        /// 创建机器人回复 文本 消息数据
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<string> CreateReply_Text_MessageDataOfRobot(string content)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("content", content);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }
        /// <summary>
        /// 创建机器人回复 Markdown 消息数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static async Task<string> CreateReply_Markdown_MessageDataOfRobot(string title, string text)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("title", title);
            writer.WriteString("text", text);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }
        /// <summary>
        /// 创建机器人回复 图片 消息数据
        /// </summary>
        /// <param name="photoURL"></param>
        /// <returns></returns>
        public static async Task<string> CreateReply_Image_MessageDataOfRobot(string photoURL)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("photoURL", photoURL);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }
        /// <summary>
        /// 创建机器人回复 链接 消息数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="picUrl"></param>
        /// <param name="messageUrl"></param>
        /// <returns></returns>
        public static async Task<string> CreateReply_Link_MessageDataOfRobot(string title, string text, string picUrl, string messageUrl)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("text", text);
            writer.WriteString("title", title);
            writer.WriteString("picUrl", picUrl);
            writer.WriteString("messageUrl", messageUrl);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }
        /// <summary>
        /// 创建机器人回复 ActionCard 消息数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="singleTitle"></param>
        /// <param name="singleURL"></param>
        /// <returns></returns>
        public static async Task<string> CreateReply_ActionCard_MessageDataOfRobot(string title, string text, string singleTitle, string singleURL)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("text", text);
            writer.WriteString("title", title);
            writer.WriteString("singleTitle", singleTitle);
            writer.WriteString("singleURL", singleURL);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }
        /// <summary>
        /// 创建机器人回复 ActionCard(竖向两个按钮) 消息数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="actionTitle1"></param>
        /// <param name="actionURL1"></param>
        /// <param name="actionTitle2"></param>
        /// <param name="actionURL2"></param>
        /// <returns></returns>
        public static async Task<string> CreateReply_ActionCard_MessageDataOfRobot(string title, string text,
                                                                                    string actionTitle1, string actionURL1,
                                                                                    string actionTitle2, string actionURL2)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("text", JsonEncodedText.Encode(text));
            writer.WriteString("title", JsonEncodedText.Encode(title));
            writer.WriteString("actionTitle1", actionTitle1);
            writer.WriteString("actionURL1", actionURL1);
            writer.WriteString("actionTitle2", actionTitle2);
            writer.WriteString("actionURL2", actionURL2);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }
        /// <summary>
        /// 创建机器人回复 ActionCard(竖向三个按钮) 消息数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="actionTitle1"></param>
        /// <param name="actionURL1"></param>
        /// <param name="actionTitle2"></param>
        /// <param name="actionURL2"></param>
        /// <param name="actionTitle3"></param>
        /// <param name="actionURL3"></param>
        /// <returns></returns>
        public static async Task<string> CreateReply_ActionCard_MessageDataOfRobot(string title, string text,
                                                                                    string actionTitle1, string actionURL1,
                                                                                    string actionTitle2, string actionURL2,
                                                                                    string actionTitle3, string actionURL3)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("text", text);
            writer.WriteString("title", title);
            writer.WriteString("actionTitle1", actionTitle1);
            writer.WriteString("actionURL1", actionURL1);
            writer.WriteString("actionTitle2", actionTitle2);
            writer.WriteString("actionURL2", actionURL2);
            writer.WriteString("actionTitle3", actionTitle3);
            writer.WriteString("actionURL3", actionURL3);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }
        /// <summary>
        /// 创建机器人回复 ActionCard(竖向四个按钮) 消息数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="actionTitle1"></param>
        /// <param name="actionURL1"></param>
        /// <param name="actionTitle2"></param>
        /// <param name="actionURL2"></param>
        /// <param name="actionTitle3"></param>
        /// <param name="actionURL3"></param>
        /// <param name="actionTitle4"></param>
        /// <param name="actionURL4"></param>
        /// <returns></returns>
        public static async Task<string> CreateReply_ActionCard_MessageDataOfRobot(string title, string text,
                                                                                    string actionTitle1, string actionURL1,
                                                                                    string actionTitle2, string actionURL2,
                                                                                    string actionTitle3, string actionURL3,
                                                                                    string actionTitle4, string actionURL4)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("text", text);
            writer.WriteString("title", title);
            writer.WriteString("actionTitle1", actionTitle1);
            writer.WriteString("actionURL1", actionURL1);
            writer.WriteString("actionTitle2", actionTitle2);
            writer.WriteString("actionURL2", actionURL2);
            writer.WriteString("actionTitle3", actionTitle3);
            writer.WriteString("actionURL3", actionURL3);
            writer.WriteString("actionTitle4", actionTitle4);
            writer.WriteString("actionURL4", actionURL4);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }
        /// <summary>
        /// 创建机器人回复 ActionCard(竖向五个按钮) 消息数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="actionTitle1"></param>
        /// <param name="actionURL1"></param>
        /// <param name="actionTitle2"></param>
        /// <param name="actionURL2"></param>
        /// <param name="actionTitle3"></param>
        /// <param name="actionURL3"></param>
        /// <param name="actionTitle4"></param>
        /// <param name="actionURL4"></param>
        /// <param name="actionTitle5"></param>
        /// <param name="actionURL5"></param>
        /// <returns></returns>
        public static async Task<string> CreateReply_ActionCard_MessageDataOfRobot(string title, string text,
                                                                                    string actionTitle1, string actionURL1,
                                                                                    string actionTitle2, string actionURL2,
                                                                                    string actionTitle3, string actionURL3,
                                                                                    string actionTitle4, string actionURL4,
                                                                                    string actionTitle5, string actionURL5)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("text", text);
            writer.WriteString("title", title);
            writer.WriteString("actionTitle1", actionTitle1);
            writer.WriteString("actionURL1", actionURL1);
            writer.WriteString("actionTitle2", actionTitle2);
            writer.WriteString("actionURL2", actionURL2);
            writer.WriteString("actionTitle3", actionTitle3);
            writer.WriteString("actionURL3", actionURL3);
            writer.WriteString("actionTitle4", actionTitle4);
            writer.WriteString("actionURL4", actionURL4);
            writer.WriteString("actionTitle5", actionTitle5);
            writer.WriteString("actionURL5", actionURL5);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }
        /// <summary>
        /// 创建机器人回复 ActionCard(横向两个按钮) 消息数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="actionTitle1"></param>
        /// <param name="actionURL1"></param>
        /// <param name="actionTitle2"></param>
        /// <param name="actionURL2"></param>
        /// <returns></returns>
        public static async Task<string> CreateReply_ActionCard2_MessageDataOfRobot(string title, string text,
                                                                                    string buttonTitle1, string buttonUrl1,
                                                                                    string buttonTitle2, string buttonUrl2)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("text", JsonEncodedText.Encode(text));
            writer.WriteString("title", JsonEncodedText.Encode(title));
            writer.WriteString("buttonTitle1", JsonEncodedText.Encode(buttonTitle1));
            writer.WriteString("buttonUrl1", buttonUrl1);
            writer.WriteString("buttonTitle2", JsonEncodedText.Encode(buttonTitle2));
            writer.WriteString("buttonUrl2", buttonUrl2);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }

        /// <summary>
        /// 创建机器人回复 语音 消息数据
        /// </summary>
        /// <param name="mediaId">通过上传媒体文件接口，获取media_id参数值。支持ogg、amr格式。</param>
        /// <param name="duration">语音消息时长，单位（毫秒）</param>
        /// <returns></returns>
        public static async Task<string> CreateReply_Audio_MessageDataOfRobot(string mediaId, TimeSpan duration)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("mediaId", mediaId);
            writer.WriteString("duration", ((long)duration.TotalMilliseconds).ToString());

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }

        /// <summary>
        /// 创建机器人回复 文件 消息数据
        /// </summary>
        /// <param name="mediaId">通过上传媒体文件接口，获取media_id参数值。支持ogg、amr格式。</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fileType">文件类型，为保证兼容，此处参数自行写入。(具体支持请参考钉钉文档。本方法编写时支持：xlsx、pdf、zip、rar、doc、docx)</param>
        /// <returns></returns>
        public static async Task<string> CreateReply_File_MessageDataOfRobot(string mediaId, string fileName, string fileType)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("mediaId", mediaId);
            writer.WriteString("fileName", fileName);
            writer.WriteString("fileType", fileType.ToLower());

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }
        /// <summary>
        /// 创建机器人回复 视频 消息数据
        /// </summary>
        /// <param name="mediaId">视频media_id。通过上传媒体文件接口，获取 media_id 参数值。</param>
        /// <param name="duration">语音消息时长，单位（秒）</param>
        /// <param name="picMediaId">视频封面图media_id。通过上传媒体文件接口，获取 media_id 参数值。</param>
        /// <param name="videoType">视频类型，支持mp4格式。（其他格式请关注钉钉文档更新情况）</param>
        /// <returns></returns>
        public static async Task<string> CreateReply_Video_MessageDataOfRobot(string mediaId, TimeSpan duration, string picMediaId, string videoType = "mp4")
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("videoMediaId", mediaId);
            writer.WriteString("duration", ((long)duration.TotalSeconds).ToString());
            writer.WriteString("picMediaId", picMediaId);
            writer.WriteString("videoType", videoType);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return DingtalkStreamUtilities.CreateReply_Callback_MessageData(Encoding.UTF8.GetString(stream.ToArray()));
        }

        #endregion

        #endregion
    }
}
