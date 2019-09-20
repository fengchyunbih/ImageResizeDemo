using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageResizer
{
    internal class Program
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private static readonly int _processAmount = 200;
        private static readonly int _scale = 2;

        private static async Task Main(string[] args)
        {
            string sourceFolder = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationFolder = Path.Combine(Environment.CurrentDirectory, "output"); ;

            #region -非同步 (個別處理存取放大圖片)-
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"--- 非同步 (個別處理存取放大圖片) ---");
            ImageProcessAsync1 imageProcessAsync1 = new ImageProcessAsync1();
            var asyncCost1 = new List<long>();
            int asyncCount1 = 1;
            while (asyncCount1 <= _processAmount)
            {
                var processTasks = new List<Task>();
                Stopwatch sw = new Stopwatch();
                imageProcessAsync1.Clean(destinationFolder);

                sw.Start();
                foreach (var imagePath in imageProcessAsync1.GetImagesPath(sourceFolder))
                {
                    var task = imageProcessAsync1.ProcessResizeImageAsync(imagePath, destinationFolder, _scale);
                    processTasks.Add(task);
                }
                await Task.WhenAll(processTasks);
                sw.Stop();

                _log.Trace($"async-1|{sw.ElapsedMilliseconds}");
                Console.WriteLine($"花費時間: {sw.ElapsedMilliseconds} ms current count: {asyncCount1}");
                asyncCost1.Add(sw.ElapsedMilliseconds);

                asyncCount1++;
            }
            Console.WriteLine($"sample:{asyncCount1 - 1}, mean:{asyncCost1.Average():F2} ms, std:{asyncCost1.Std():F2} ms");
            Console.WriteLine($"--- 非同步 (個別處理存取放大圖片) ---");
            Console.ResetColor();
            #endregion -非同步 (個別處理存取放大圖片)-

            GC.Collect();

            #region -非同步 (同時處理存取放大圖片)-
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"--- 非同步 (同時處理存取放大圖片) ---");
            ImageProcessAsync2 imageProcessAsync2 = new ImageProcessAsync2();
            var asyncCost2 = new List<long>();
            int asyncCount2 = 1;
            while (asyncCount2 <= _processAmount)
            {
                var processTasks = new List<Task>();
                Stopwatch sw = new Stopwatch();
                imageProcessAsync2.Clean(destinationFolder);

                sw.Start();
                foreach (var imagePath in imageProcessAsync2.GetImagesPath(sourceFolder))
                {
                    var task = imageProcessAsync2.ResizeImagesAsync(imagePath, destinationFolder, _scale);
                    processTasks.Add(task);
                }
                await Task.WhenAll(processTasks);
                sw.Stop();

                _log.Trace($"async-2|{sw.ElapsedMilliseconds}");
                Console.WriteLine($"花費時間: {sw.ElapsedMilliseconds} ms current count: {asyncCount2}");
                asyncCost2.Add(sw.ElapsedMilliseconds);

                asyncCount2++;
            }
            Console.WriteLine($"sample:{asyncCount2 - 1}, mean:{asyncCost2.Average():F2} ms, std:{asyncCost2.Std():F2} ms");
            Console.WriteLine($"--- 非同步 (同時處理存取放大圖片) ---");
            Console.ResetColor();
            #endregion -非同步 (同時處理存取放大圖片)-

            GC.Collect();

            #region -同步-
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"--- 同步 ---");
            var syncCost = new List<long>();
            ImageProcess imageProcess = new ImageProcess();
            int syncCount = 1;
            while (syncCount <= _processAmount)
            {
                imageProcess.Clean(destinationFolder);
                Stopwatch sw = new Stopwatch();

                sw.Start();
                imageProcess.ResizeImages(sourceFolder, destinationFolder, _scale);
                sw.Stop();

                _log.Trace($"sync|{sw.ElapsedMilliseconds}");
                Console.WriteLine($"花費時間: {sw.ElapsedMilliseconds} ms current count: {syncCount}");
                syncCost.Add(sw.ElapsedMilliseconds);

                syncCount++;
            }
            Console.WriteLine($"sample:{syncCount - 1}, mean:{syncCost.Average():F2} ms, std:{syncCost.Std():F2} ms");
            Console.WriteLine($"--- 同步 ---");
            Console.ResetColor();
            #endregion -同步-

            Console.ReadKey();
        }
    }
}
