using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Reflection;

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

        public static Dictionary<string, object> ToDictionary(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var dictionary = new Dictionary<string, object>();

            // Get all public instance properties of the object
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in properties)
            {
                // Ensure the property can be read
                if (prop.CanRead)
                {
                    // Get the property name as the key
                    string key = prop.Name;
                    // Get the property value
                    object value = prop.GetValue(obj);

                    // Add the key-value pair to the dictionary
                    dictionary.Add(key, value);
                }
            }

            return dictionary;
        }
    }
}
