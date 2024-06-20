using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Jusoft.DingtalkStream.Robot
{
    public static class ReceivedRobotMessageExtensions
    {
        /// <summary>
        /// 获取文本消息内容
        /// </summary>
        /// <param name="robotMessage"></param>
        /// <returns></returns>
        public static ReceivedRobotMessage.TextContent GetTextContent(this ReceivedRobotMessage robotMessage)
        {
            if (robotMessage.Payload.GetProperty("msgtype").GetString() != "text")
            {
                throw new ArgumentException("消息类型不是文本消息");
            }
            return new ReceivedRobotMessage.TextContent
            {
                Content = robotMessage.Payload.GetProperty("text").GetProperty("content").GetString()
            };
        }
        /// <summary>
        /// 获取语音消息内容
        /// </summary>
        /// <param name="robotMessage"></param>
        /// <returns></returns>
        public static ReceivedRobotMessage.AudioContent GetAudioContent(this ReceivedRobotMessage robotMessage)
        {
            if (robotMessage.Payload.GetProperty("msgtype").GetString() != "audio")
            {
                throw new ArgumentException("消息类型不是语音消息");
            }
            var content = robotMessage.Payload.GetProperty("content");

            var audioContent = new ReceivedRobotMessage.AudioContent
            {
                DownloadCode = content.GetProperty("downloadCode").GetString() ,
                Recognition = content.GetProperty("recognition").GetString() ,
            };
            // duration 有可能性不返回
            if (content.TryGetProperty("duration" , out var jsonElmDuration))
            {
                audioContent.Duration = TryGetInt64(jsonElmDuration);
            }

            return audioContent;
        }
        /// <summary>
        /// 获取图片消息内容
        /// </summary>
        /// <param name="robotMessage"></param>
        /// <returns></returns>
        public static ReceivedRobotMessage.PictureContent GetPictureContent(this ReceivedRobotMessage robotMessage)
        {
            if (robotMessage.Payload.GetProperty("msgtype").GetString() != "picture")
            {
                throw new ArgumentException("消息类型不是图片消息");
            }
            var content = robotMessage.Payload.GetProperty("content");
            return new ReceivedRobotMessage.PictureContent
            {
                DownloadCode = content.GetProperty("downloadCode").GetString() ,
                PictureDownloadCode = content.GetProperty("pictureDownloadCode").GetString() ,
            };

        }
        /// <summary>
        /// 获取视频消息内容
        /// </summary>
        /// <param name="robotMessage"></param>
        /// <returns></returns>
        public static ReceivedRobotMessage.VideoContent GetVideoContent(this ReceivedRobotMessage robotMessage)
        {
            if (robotMessage.Payload.GetProperty("msgtype").GetString() != "video")
            {
                throw new ArgumentException("消息类型不是视频消息");
            }
            var content = robotMessage.Payload.GetProperty("content");
            var videoContent = new ReceivedRobotMessage.VideoContent
            {
                DownloadCode = content.GetProperty("downloadCode").GetString() ,
                VideoType = content.GetProperty("videoType").GetString() ,
            };

            // duration 有可能性不返回
            if (content.TryGetProperty("duration" , out var jsonElmDuration))
            {
                videoContent.Duration = TryGetInt64(jsonElmDuration);
            }
            return videoContent;
        }

        /// <summary>
        /// 获取文件消息内容
        /// </summary>
        /// <param name="robotMessage"></param>
        /// <returns></returns>
        public static ReceivedRobotMessage.FileContent GetFileContent(this ReceivedRobotMessage robotMessage)
        {
            if (robotMessage.Payload.GetProperty("msgtype").GetString() != "file")
            {
                throw new ArgumentException("消息类型不是文件消息");
            }
            var content = robotMessage.Payload.GetProperty("content");
            return new ReceivedRobotMessage.FileContent
            {
                DownloadCode = content.GetProperty("downloadCode").GetString() ,
                FileName = content.GetProperty("fileName").GetString() ,
            };

        }
        /// <summary>
        /// 获取富文本消息内容
        /// </summary>
        /// <param name="robotMessage"></param>
        /// <returns></returns>
        public static ReceivedRobotMessage.RichTextContent GetRichTextContent(this ReceivedRobotMessage robotMessage)
        {
            if (robotMessage.Payload.GetProperty("msgtype").GetString() != "richText")
            {
                throw new ArgumentException("消息类型不是富文本消息");
            }
            var content = robotMessage.Payload.GetProperty("content");
            var richTextContent = new ReceivedRobotMessage.RichTextContent();

            //issues:7 问题修复，对于富文本消息，在取值转换时，少处理了一个层级
            var contentRichText = content.GetProperty("richText");
            foreach (var item in contentRichText.EnumerateArray())
            {
                var richText = new ReceivedRobotMessage.RichText();
                foreach (var property in item.EnumerateObject())
                {
                    switch (property.Name)
                    {
                        case "text":
                            richText.Text = property.Value.GetString();
                            break;
                        case "downloadCode":
                            richText.DownloadCode = property.Value.GetString();
                            break;
                        case "pictureDownloadCode":
                            richText.PictureDownloadCode = property.Value.GetString();
                            break;
                        case "type":
                            richText.Type = property.Value.GetString();
                            break;
                        default:
                            break;
                    }
                }
                richTextContent.RichText.Add(richText);
            }

            return richTextContent;
        }


        /// <summary>
        /// 获取因返回消息类型不正确时，但还是需要尝试获取转换的long类型处理办法
        /// </summary>
        /// <param name="jsonElement"></param>
        /// <returns></returns>
        static long? TryGetInt64(JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.String:
                    if (long.TryParse(jsonElement.GetString() , out var duration))
                    {
                        return duration;
                    }
                    break;
                case JsonValueKind.Number:
                    return jsonElement.GetInt64();
            }
            return null;
        }
    }
}
