using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;

namespace ImageResizer
{

    public class ImageProcessAsync1
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

        // load image
        /// <summary>
        /// 非同步讀取圖片
        /// </summary>
        /// <param name="path">圖片路徑</param>
        /// <returns></returns>
        public async Task<Image> GetImageByPathAsync(string path)
        {
            var task = Task.Run(() => GetImageByPath(path));
            return await task;
        }

        /// <summary>
        /// 讀取圖片
        /// </summary>
        /// <param name="path">圖片路徑</param>
        /// <returns></returns>
        public Image GetImageByPath(string path)
        {
            return Image.FromFile(path);
        }

        // resize
        /// <summary>
        /// 依指定寬高比例縮放圖片
        /// </summary>
        /// <param name="image">來源圖片</param>
        /// <param name="widthScale">縮放寬度比例</param>
        /// <param name="heightScale">縮放高度比例</param>
        /// <returns>結果圖片</returns>
        public async Task<Image> ResizeImageAsync(Image image, double widthScale, double heightScale)
        {
            var task = Task.Run(() => ResizeImage(image, widthScale, heightScale));
            return await task;
        }

        /// <summary>
        /// 依指定寬高比例縮放圖片
        /// </summary>
        /// <param name="image">來源圖片</param>
        /// <param name="widthScale">縮放寬度比例</param>
        /// <param name="heightScale">縮放高度比例</param>
        /// <returns>結果圖片</returns>
        public Image ResizeImage(Image image, double widthScale, double heightScale)
        {
            int destionatonWidth = (int)(image.Width * widthScale);
            int destionatonHeight = (int)(image.Height * heightScale);

            Bitmap resizedbitmap = new Bitmap(destionatonWidth, destionatonHeight);
            using Graphics g = Graphics.FromImage(resizedbitmap);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);
            g.DrawImage(image,
                new Rectangle(0, 0, destionatonWidth, destionatonHeight),
                new Rectangle(0, 0, image.Width, image.Height),
                GraphicsUnit.Pixel);
            return resizedbitmap;
        }

        /// <summary>
        /// 依指定比例縮放圖片
        /// </summary>
        /// <param name="image">來源圖片</param>
        /// <param name="scale">縮放比例</param>
        /// <returns>結果圖片</returns>
        public async Task<Image> ResizeImageAsync(Image image, double scale)
        {
            var task = Task.Run(() => ResizeImage(image, scale));
            return await task;
        }

        /// <summary>
        /// 依指定比例縮放圖片
        /// </summary>
        /// <param name="image">來源圖片</param>
        /// <param name="scale">縮放比例</param>
        /// <returns>結果圖片</returns>
        public Image ResizeImage(Image image, double scale)
        {
            return ResizeImage(image, scale, scale);
        }

        // save image
        /// <summary>
        /// 儲存圖片
        /// </summary>
        /// <param name="image">來源圖片</param>
        /// <param name="path">目的路徑資料夾</param>
        /// <param name="fileName">目的檔案名稱</param>
        public async Task SaveImageAsync(Image image, string path, string fileName)
        {
            await Task.Run(() => SaveImage(image, path, fileName));
        }

        /// <summary>
        /// 儲存圖片
        /// </summary>
        /// <param name="image">來源圖片</param>
        /// <param name="path">目的路徑資料夾</param>
        /// <param name="fileName">目的檔案名稱</param>
        public void SaveImage(Image image, string path, string fileName)
        {
            string destFile = Path.Combine(path, fileName + ".jpg");
            image.Save(destFile);
            image.Dispose();
        }

        // mix
        public async Task ProcessResizeImageAsync(string srcParh, string destPath, double widthScale, double heightScale)
        {
            var fineName = Path.GetFileNameWithoutExtension(srcParh);

            var soureImage = await GetImageByPathAsync(srcParh);

            var resultImage = await ResizeImageAsync(soureImage, widthScale, heightScale);

            await SaveImageAsync(resultImage, destPath, fineName);
        }

        public async Task ProcessResizeImageAsync(string srcParh, string destPath, double scale)
        {
            await ProcessResizeImageAsync(srcParh, destPath, scale, scale);
        }

    }
}
