using System.IO;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace XamarinFormsAzure.Droid
{
    public class FileCache : TokenCache
    {
        private static readonly object FileLock = new object();

        private string cacheFilePath;

        public FileCache(string filePath = "TokenCache.dat")
        {
            this.cacheFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filePath);
            AfterAccess = AfterAccessNotification;
            BeforeAccess = BeforeAccessNotification;
            lock (FileLock)
            {
                Deserialize(File.Exists(this.cacheFilePath) ? File.ReadAllBytes(this.cacheFilePath) : null);
            }
        }

        public override void Clear()
        {
            base.Clear();
            File.Delete(this.cacheFilePath);
        }

        private void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            lock (FileLock)
            {
                Deserialize(File.Exists(this.cacheFilePath) ? File.ReadAllBytes(this.cacheFilePath) : null);
            }
        }

        private void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            if (HasStateChanged)
            {
                lock (FileLock)
                {
                    File.WriteAllBytes(this.cacheFilePath, Serialize());
                    HasStateChanged = false;
                }
            }
        }
    }
}
