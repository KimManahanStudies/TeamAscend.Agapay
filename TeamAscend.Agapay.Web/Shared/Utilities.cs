using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace TeamAscend.Agapay.Web.Shared
{
    public class Utilities
    {
        public static byte[] ProportionallyResizeBitmap(byte[] imageData, int maxWidth, int maxHeight)
        {
            using var image = Image.Load(imageData);

            // Calculate new dimensions while maintaining aspect ratio
            double ratioX = (double)maxWidth / image.Width;
            double ratioY = (double)maxHeight / image.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            // Resize the image
            image.Mutate(x => x
                .Resize(new ResizeOptions
                {
                    Size = new Size(newWidth, newHeight),
                    Mode = ResizeMode.Max,
                    Position = AnchorPositionMode.Center
                }));

            // Save to memory stream
            using var ms = new MemoryStream();
            image.Save(ms, new JpegEncoder());
            return ms.ToArray();
        }
    }
}
