using MusicPlayer.Core.Entities;

namespace MusicPlayer.Core.NullObjects
{
    public class ImageNullObject : Image
    {
        private static readonly string DEFAULT_IMAGE_PATH = "ms-appx:///Assets/no-photo-available.png";

        public ImageNullObject() : base()
        {
            FilePath = DEFAULT_IMAGE_PATH;
        }
    }
}
