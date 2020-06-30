using System;
using System.Drawing;
using System.IO;

namespace MediaPanther.Framework.Imaging
{
	public class ImageHelper
	{
		/// <summary>
		/// Returns the width and height of an image by inspecting the image itself.
		/// </summary>
		/// <param name="path">The full file-system path (local or UNC) to the image file.</param>
		public static Size GetImageDimensions(string path)
		{
		    using (var i = Image.FromFile(path))
				return i.Size;
		}

	    /// <summary>
	    /// Returns the width and height of an image by inspecting the image itself.
	    /// </summary>
	    /// <param name="stream">The stream representing the image to get the size for.</param>
	    /// <exception cref="ArgumentNullException">Thrown if an invalid stream is passed in.</exception>
	    public static Size GetImageDimensions(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            using (var i = Image.FromStream(stream))
                return i.Size;
        }
	}
}