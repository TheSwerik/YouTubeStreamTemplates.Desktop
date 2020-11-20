using System;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeStreamTemplates.LiveStream
{
    public static class ExtensionMethods
    {
        public static LiveStream ToLiveStream(this LiveBroadcast liveBroadcast)
        {
            return new
                   {
                       liveBroadcast.Id,
                       liveBroadcast.Snippet.Title,
                       liveBroadcast.Snippet.Description,
                       liveBroadcast.Snippet.Thumbnails,
                       StartTime = DateTime.Parse(liveBroadcast.Snippet.ActualStartTime),
                       EndTime = DateTime.Parse(liveBroadcast.Snippet.ActualEndTime)
                   };
        }
    }