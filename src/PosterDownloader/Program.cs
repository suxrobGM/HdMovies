using System;
using System.IO;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace PosterDownloader
{
    class Program
    {
        static IWebDriver _driver;

        static void Main(string[] args)
        {
            if (!Directory.Exists("Posters"))
            {
                Directory.CreateDirectory("Posters");
            }

            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://www.moviefone.com/movies/?page=2");
            DownloadPosters();

            Console.WriteLine("\nDownloaded all posters!");
            Console.ReadKey();
        }

        static void DownloadPosters()
        {
            using var webClient = new WebClient();
            WaitForReady(By.XPath("//img[@class='movie-poster lazy']"));
            var posters = _driver.FindElements(By.XPath("//img[@class='movie-poster lazy']"));

            Console.WriteLine($"\nParsed {posters.Count} posters");
            int count = 0;
            foreach (var poster in posters)
            {
                var imgUrl = poster.GetAttribute("data-src");
                var urlWithoutQuery = imgUrl.Substring(0, imgUrl.IndexOf("?"));
                var posterName = poster.GetAttribute("alt");
                Console.WriteLine($"{++count}). Downloading \'{posterName}\' from {urlWithoutQuery}");

                var filename = posterName.Trim().ToLower().Replace(' ', '_');
                webClient.DownloadFile(urlWithoutQuery, $"Posters\\{filename}.jpg");
            }
        }

        static void WaitForReady(By by)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            wait.Until(driver =>
            {
                //bool isAjaxFinished = (bool)((IJavaScriptExecutor)driver).ExecuteScript("return jQuery.active == 0");
                return IsElementPresent(by);
            });
        }

        static bool IsElementPresent(By by)
        {
            try
            {
                _driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
