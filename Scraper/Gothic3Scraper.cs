using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Shared;
using System.Text.RegularExpressions;
using System.Text.Json;
using Shared.Extensions;
using System;

namespace Scraper;

internal class Gothic3Scraper
{
    [SetUp]
    public void StartBrowser()
    {
        /* Local Selenium WebDriver */
       
    }
    [Test]
    public async Task MassScraping()
    {
        Dictionary<string, string> areasAndTheirUrls = new()
        {
            { "Ardea", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1473" },
            { "Cape Dun", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1474" },
            { "Faring", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1475" },
            { "Geldern", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1476" },
            { "Gotha", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1477" },
            { "Montera", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1478" },
            { "Nemora", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1479" },
            { "Okara", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1480" },
            { "Reddock", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1481" },
            { "Silden", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1482" },
            { "Trelis", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1483" },
            { "Vengard", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1484" },

            { "Bakaresh", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1485" },
            { "Ben Erai", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1486" },
            { "Ben Sala", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1487" },
            { "Braga", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1488" },
            { "Ishtar", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1489" },
            { "Mora Sul", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1490" },
            { "Lago", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1491" },

            { "Fire Clan", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1492" },
            { "Hammer Clan", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1493" },
            { "Monastery", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1494" },
            { "Wolf Clan", "https://guides.gamepressure.com/gothic3/guide.asp?ID=1495" },
        };

        List<Gothic3QuestArea> gothic3QuestAreas = new();


        foreach (var areaUrl in areasAndTheirUrls)
        {
            gothic3QuestAreas.Add(new Gothic3QuestArea()
            {
                Name = areaUrl.Key,
                Quests = Gothic3Scraping(areaUrl.Key, areaUrl.Value)
            });
        }

        string jsonString = JsonSerializer.Serialize(gothic3QuestAreas);

        File.WriteAllText($"gothic3quests", jsonString);
        Assert.Pass();
    }
  

    private List<Gothic3Quest> Gothic3Scraping(string areaName, string url)
    {
        var driver = new ChromeDriver();

        driver.Manage().Window.Maximize();
        driver.Url = url;
        /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
        var timeout = 10000; /* Maximum wait time of 10 seconds */
        var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

        Thread.Sleep(5000);

        /* Once the page has loaded, scroll to the end of the page to load all the videos */
        /* Scroll to the end of the page to load all the videos in the channel */
        /* Reference - https://stackoverflow.com/a/51702698/126105 */
        /* Get scroll height */
        Int64 last_height = (Int64)(((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight"));
        while (true)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.documentElement.scrollHeight);");
            /* Wait to load page */
            Thread.Sleep(2000);
            /* Calculate new scroll height and compare with last scroll height */
            Int64 new_height = (Int64)((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight");
            if (new_height == last_height)
                /* If heights are the same it will exit the function */
                break;
            last_height = new_height;
        }

        IEnumerable<string> titles = driver.FindElements(By.XPath("/html/body/main/div/article/div/h3")).Select(title => title.Text).Where(title => !string.IsNullOrWhiteSpace(title));

        IWebElement allArticle = driver.FindElement(By.XPath("/html/body/main/div[1]/article/div[1]"));
        string allArticleText = allArticle.Text;
        string[] lines = allArticleText.Split(Environment.NewLine);

        List<Gothic3Quest> quests = new();

        foreach (string title in titles)
        {
            int nextTitleIndex = titles.ToList().IndexOf(title) + 1;
            string? nextTitle = nextTitleIndex > 0 ? titles.ElementAtOrDefault(nextTitleIndex) : null;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines.ElementAt(i).Trim() == title.Trim())
                {
                    IEnumerable<string> allQuestText = lines.Skip(i + 1).TakeWhile(line => line != nextTitle);

                    Gothic3Quest quest = CreateQuest(title, allQuestText, areaName);

                    quests.Add(quest);
                }
            }
        }
        driver.Quit();

        return quests;
    }

    private Gothic3Quest CreateQuest(string title, IEnumerable<string> textData, string areaName)
    {
        int indexOfBenefits = textData.ToList().IndexOf("Benefits:");
        if (indexOfBenefits == -1)
            return new Gothic3Quest()
            {
                Title = title,
                Decription = string.Join(Environment.NewLine, textData)
            };

        var quest = new Gothic3Quest()
        {
            Title = title,
            Decription = string.Join(Environment.NewLine, textData.Take(indexOfBenefits))
        };

        var expLine = textData.FirstOrDefault(line => line.Contains("Experience"));

        if(expLine != null)
            quest.Exp = TextToInt(expLine);
        
        var reputationLine = textData.FirstOrDefault(line => line.Contains("Reputation") && !IsWrongReputationTextWorkaround(line));

        if (reputationLine != null)
        {
            quest.ReputationValue = TextToInt(reputationLine);
            quest.ReputationType = ReputationFromText(reputationLine, areaName);
        }

        return quest;
    }

    private bool IsWrongReputationTextWorkaround(string reputationLine)
    {
        return reputationLine.Contains("Reputation points: 1500");
    }

    private Gothic3Reputation ReputationFromText(string text, string areaName)
    {
        foreach (Gothic3Reputation reputationType in Enum.GetValues(typeof(Gothic3Reputation)).Cast<Gothic3Reputation>())
        {
            if (text.Contains(reputationType.ToString()))
                return reputationType;
        }
        if(text.Contains(areaName))
        {
            return Gothic3Reputation.City;
        }

        throw new InvalidOperationException("Reputation type invalid");
    }

    private int TextToInt(string text)
    {
        text = Regex.Match(text, @"\d+").Value;
        return int.Parse(text);
    }
}
