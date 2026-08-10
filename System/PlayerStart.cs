using System.Diagnostics;
using Spectre.Console;
namespace rpg;

public class PlayerStart
{
    public record MenuItem(int Id, string DisplayName);
    public static Player player = null!;

    // 初始化玩家與怪物實例（作為遊戲狀態）
    public static Player SelectedPlayer()
    {

        int job = getJob();

        CurrentType selectedType = (CurrentType)getType(); // 直接作為區域變數
        Personality selectedNature = (Personality)getNature();
        
        switch (job)
        {
            case 1:
                player = new Woodsman(1, selectedType, selectedNature);
                break;
            case 2:
                player = new Farmer(1, selectedType, selectedNature);
                break;
            case 3:
                player = new Hunter(1, selectedType, selectedNature);
                break;
            case 4:
                player = new Scholar(1, selectedType, selectedNature);
                break;
            case 5:
                player = new Thief(1, selectedType, selectedNature);
                break;
            default:
                break;
        }

        return player;
    }

    private static int getJob()
    {
        string jobInfo = "", jobName = "";
        while (true)
        {
            AnsiConsole.Clear();
            var playerChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .Title("[cyan]請選擇你的身份：[/]")
                    .UseConverter(item => item.DisplayName)
                    .PageSize(5)
                    .AddChoices(new[] {
                        new MenuItem(1, "樵夫"),
                        new MenuItem(2, "農夫"),
                        new MenuItem(3, "獵人"),
                        new MenuItem(4, "學者"),
                        new MenuItem(5, "扒手"),
                    }));

            switch (playerChoice.Id)
            {
                case 1:
                    jobInfo = "村莊中的樵夫，因長年揮舞斧頭，爆發力不可小覷，也是村莊中力量最大的，肌肉與長年累積的厚繭也增加自身不少保護力，但腳程較慢，對魔法不擅長";
                    jobName = "樵夫";
                    break;
                case 2:
                    jobInfo = "村莊中的農夫，日出而作，日落而息，因長期下田耕地，擁有扎實的下盤功夫，跟樵夫比相撲從沒輸過，像是牆一樣屹立不倒，但腳程較慢，對魔法不擅長";
                    jobName = "農夫";
                    break;
                case 3:
                    jobInfo = "村莊中的獵人，成天在山林狩獵，適應各種地形移動，雖然力量與體魄沒有樵夫和農夫來的強，但也是村莊中排名前幾的存在，對魔法相關小小有研究。";
                    jobName = "獵人";
                    break;
                case 4:
                    jobInfo = "住在村莊邊緣的學者，說是村莊中最有智慧的也不為過，整天關在房裡研究外地買來的各種書籍，對魔法書籍研究興趣極高，因常廢寢忘食的研究，導致體力不好。";
                    jobName = "學者";                    
                    break;
                case 5:
                    jobInfo = "流浪到村莊的扒手，腳程極快，雖然常常被發現行竊，但沒有一次被抓到，常去學者家偷快放到過期的食物，理解力好，不知不覺記下放在食物旁的魔法防禦書籍的內容。";
                    jobName = "扒手";                    
                    break;
                default:
                    break;
            }

            var panel = new Panel(jobInfo)
            {
                Header = new PanelHeader($"[bold yellow] {jobName} [/]"),
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.Cyan1),
                Width = 80,
                Height = 8
            };

            AnsiConsole.Write(new Align(panel, HorizontalAlignment.Left));
            AnsiConsole.WriteLine();

            var d = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .Title("[cyan]確定身份?[/]")
                    .UseConverter(item => item.DisplayName)
                    .PageSize(5)
                    .AddChoices(new[] {
                        new MenuItem(1, "是"),
                        new MenuItem(2, "否"),
                    }));

            switch (d.Id)
            {
                case 1:
                    return playerChoice.Id;
                case 2:
                    break;
                default:                    
                    break;
            }
        }
    }
    
    private static int getType()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var playerChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .Title("[cyan]請選擇你的屬性：[/]")
                    .UseConverter(item => item.DisplayName)
                    .PageSize(8)
                    .AddChoices(new[] {
                        new MenuItem(0, "無"),
                        new MenuItem(1, "火"),
                        new MenuItem(2, "水"),
                        new MenuItem(3, "土"),
                        new MenuItem(4, "風"),
                        new MenuItem(5, "暗"),
                        new MenuItem(6, "光"),
                    }));

            var d = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .Title($"[cyan]確定屬性為 {playerChoice.DisplayName} 嗎?[/]")
                    .UseConverter(item => item.DisplayName)
                    .PageSize(5)
                    .AddChoices(new[] {
                        new MenuItem(1, "是"),
                        new MenuItem(2, "否"),
                    }));

            switch (d.Id)
            {
                case 1:
                    return playerChoice.Id;
                case 2:
                    break;
                default:                    
                    break;
            }
        }
    }

    private static int getNature()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var playerChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .Title("[cyan]請選擇你的性格：[/]")
                    .UseConverter(item => item.DisplayName)
                    .PageSize(7)
                    .AddChoices(new[] {
                        new MenuItem(0, "平衡(能力平衡)"),
                        new MenuItem(1, "暴躁(攻擊上升快，防禦上升慢)"),
                        new MenuItem(2, "謹慎(防禦上升快，攻擊上升慢)"),
                        new MenuItem(3, "專注(魔攻上升快，魔防上升慢)"),
                        new MenuItem(4, "冥想(魔防上升快，魔攻上升慢)"),
                        new MenuItem(5, "神行(速度上升快，雙防上升慢)"),
                    }));

            var d = AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .Title($"[cyan]確定你是這種個性的人嗎?[/]")
                    .UseConverter(item => item.DisplayName)
                    .PageSize(5)
                    .AddChoices(new[] {
                        new MenuItem(1, "是"),
                        new MenuItem(2, "否"),
                    }));

            switch (d.Id)
            {
                case 1:
                    return playerChoice.Id;
                case 2:
                    break;
                default:                    
                    break;
            }
        }
    }
}
