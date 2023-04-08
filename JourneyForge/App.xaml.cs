using Shared;

namespace JourneyForge;

public partial class App : Application
{
    private static List<Gothic3QuestArea> questAreas;

    public static List<Gothic3QuestArea> QuestAreas
    {
        get => questAreas; set
        {
            questAreas = value;

        }
    }
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
