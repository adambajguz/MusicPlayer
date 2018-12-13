using MusicPlayer.Core.Entities;

namespace MusicPlayer.Core.NullObjects
{
    class ImageNullObject : Image
    {
        private static readonly string DEFAULT_IMAGE_PATH = "";

        public ImageNullObject() : base()
        {
            FilePath = DEFAULT_IMAGE_PATH;
        }
    }
}
