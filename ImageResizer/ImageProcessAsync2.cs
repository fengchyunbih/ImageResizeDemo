using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace ImageResizer
{
    public class ImageProcessAsync2
    {
        /// <summary>
        /// 清空指定路徑檔案
        /// </summary>
        /// <param name="destPath">指定路徑</param>
        public void Clean(string destPath)
        {
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            else
            {
                var allImageFiles = Directory.GetFiles(destPath, "*", SearchOption.AllDirectories);

                foreach (var item in allImageFiles)
                {
                    File.Delete(item);
                }
            }
        }

        /// <summary>
        /// 取得指定資料夾下圖片路徑
        /// </summary>
        /// <param name="srcPath">指定資料夾</param>
        /// <returns></returns>
        public IEnumerable<string> GetImagesPath(string srcPath)
        {
            foreach (var path in Directory.GetFiles(srcPath, "*.png", SearchOption.AllDirectories))
                yield return path;
            foreach (var path in Directory.GetFiles(srcPath, "*.jpg", SearchOption.AllDirectories))
                yield return path;
            foreach (var path in Directory.GetFiles(srcPath, "*.jpeg", SearchOption.AllDirectories))
                yield return path;
        }

        /// <summary>
        /// 進行圖片的縮放作業
        /// </summary>
        /// <param name="imagePath">圖片來源路徑</param>
        /// <param name="destPath">產生圖片目的目錄路徑</param>
        /// <param name="scale">縮放比例</param>
        public void ResizeImages(string imagePath, string destPath, double scale)
        {
            using Image imgPhoto = Image.FromFile(imagePath);
            string imgName = Path.GetFileNameWithoutExtension(imagePath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            int destionatonWidth = (int)(sourceWidth * scale);
            int destionatonHeight = (int)(sourceHeight * scale);

            using Bitmap processedImage = ProcessBitmap((Bitmap)imgPhoto,
                sourceWidth, sourceHeight,
                destionatonWidth, destionatonHeight);

            string destFile = Path.Combine(destPath, imgName + ".jpg");
            processedImage.Save(destFile, ImageFormat.Jpeg);
        }

        /// <summary>
        /// 進行圖片的縮放作業
        /// </summary>
        /// <param name="imagePath">圖片來源路徑</param>
        /// <param name="destPath">產生圖片目的目錄路徑</param>
        /// <param name="scale">縮放比例</param>
        public async Task ResizeImagesAsync(string imagePath, string destPath, double scale)
        {
            await Task.Factory.StartNew(() => ResizeImages(imagePath, destPath, scale));
        }

        /// <summary>
        /// 針對指定圖片進行縮放作業
        /// </summary>
        /// <param name="img">圖片來源</param>
        /// <param name="srcWidth">原始寬度</param>
        /// <param name="srcHeight">原始高度</param>
        /// <param name="newWidth">新圖片的寬度</param>
        /// <param name="newHeight">新圖片的高度</param>
        /// <returns></returns>
        private Bitmap ProcessBitmap(Bitmap img, int srcWidth, int srcHeight, int newWidth, int newHeight)
        {
            Bitmap resizedbitmap = new Bitmap(newWidth, newHeight);
            using Graphics g = Graphics.FromImage(resizedbitmap);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);
            g.DrawImage(img,
                new Rectangle(0, 0, newWidth, newHeight),
                new Rectangle(0, 0, srcWidth, srcHeight),
                GraphicsUnit.Pixel);
            return resizedbitmap;
        }
    }
}
