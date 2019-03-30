using System.Collections.Generic;
using PdfSharp.Drawing;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PdfSharp.Pdf
{
    /// <summary>
    /// Extension methods for the <see cref="Image"/> class.
    /// </summary>
    public static class ImageExtensions
    {
        private static MemoryStream ToStream(this Image image)
        {
            var memory = new MemoryStream();
            image.Save(memory, ImageFormat.Png);
            return memory;
        }

        /// <summary>
        /// Generates a PDF page from the image.
        /// </summary>
        /// <param name="image">The image to convert to a <see cref="PdfPage"/>.</param>
        /// <returns>The <see cref="PdfPage"/> containing the image.</returns>
        public static PdfDocument ToPdf(this Image image)
        {
            if (image == null) return (new PdfDocument());
            return (ToPdf(new[] {image}));
        }

        /// <summary>
        /// Generates a PDF document from the collection of enumerations with each image representing
        /// a new page.
        /// </summary>
        /// <param name="images">The collection of images to convert to pages.</param>
        /// <returns>The <see cref="PdfDocument"/> containing the images.</returns>
        public static PdfDocument ToPdf(this IEnumerable<Image> images)
        {
            PdfDocument document = new PdfDocument();
            foreach (var image in images)
            {
                PdfPage page = new PdfPage()
                {
                    Width = image.Width,
                    Height = image.Height
                };
                document.AddPage(page);

                XGraphics xGraphics = XGraphics.FromPdfPage(page);
                XImage xImage = XImage.FromStream(image.ToStream());
                xGraphics.DrawImage(xImage, 0, 0, image.Width, image.Height);
            }
            return (document);
        }
    }
}