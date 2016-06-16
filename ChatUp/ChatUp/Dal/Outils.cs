using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ChatUp.Dal
{
    public class Outils
    {
        public static byte[] ImageToByteArray(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Gif);

            return ms.ToArray();
        }

        public static Image ByteArrayToImage(byte[] array)
        {
            MemoryStream ms = new MemoryStream();
            Image image = Image.FromStream(ms);

            return image;
        }
    }
}