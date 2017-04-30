using System;

namespace XamarinFormsAzure
{
    public class Constants
    {
        private const string ACCESS_DENIED = "access_denied";

        public const string AZURE_CLIENT_ID = "6423495f-f214-4a59-aec4-98a165a9ee35";

        public const string COMMON_AUTHORITY = "https://login.microsoftonline.com/common";

        public const string GRAPH_RESOURCE_URI = "https://graph.windows.net";

        public const string DOMAIN_HINT = "domain_hint=oberdanbitencourtgmail.onmicrosoft.com";

        public static string CACHED_USER_ID = "tdc@oberdanbitencourtgmail.onmicrosoft.com";

        public const string STORAGE_ACCOUNT_NAME = "tdc";
        public const string STORAGE_QUEUE_NAME = "tdc-queue";
        public const string STORAGE_IMAGE_BLOB = "tdc-blob";
        public const string STORAGE_CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=tdc;AccountKey=VSpDg5MgvYb6g8XYIFbGARIdAJhGOagfIV0/Ky5bfci6lv8ToCa9cufgqV390lsgNvihmDtYuk2dQhYNy0VVRA==;EndpointSuffix=core.windows.net";

        public const string STORAGE_SAS_TOKEN = "?sv=2016-05-31&ss=bfqt&srt=sco&sp=rwdlacup&se=2018-04-30T23:43:47Z&st=2017-04-30T15:43:47Z&spr=https&sig=zr6R437JCuhLkxzvrrufKgZEfCGJZHoE4gskH4Fg%2F90%3D";
        public const string BLOB_SAS_URL = "https://tdc.blob.core.windows.net/?sv=2016-05-31&ss=bfqt&srt=sco&sp=rwdlacup&se=2018-04-30T23:43:47Z&st=2017-04-30T15:43:47Z&spr=https&sig=zr6R437JCuhLkxzvrrufKgZEfCGJZHoE4gskH4Fg%2F90%3D";
        public const string QUEUE_SAS_URL = "https://tdc.queue.core.windows.net/?sv=2016-05-31&ss=bfqt&srt=sco&sp=rwdlacup&se=2018-04-30T23:43:47Z&st=2017-04-30T15:43:47Z&spr=https&sig=zr6R437JCuhLkxzvrrufKgZEfCGJZHoE4gskH4Fg%2F90%3D";

        public static readonly Uri RETURN_URI = new Uri("http://tdc2017");
    }
}