using Spectre.Console;

namespace rpg;

public static class AnimationHelper
{
    // 定義 4 格動畫幀 (尺寸 50 x 20)
    private static readonly string[] Frames = new string[]
    {
        // ── Frame 1: 蓄力起手 (刀光初現) ──
        @"[bold yellow]
                                                  
                                                  
                                                  
                       █                          
                      ██                          
                     ██                           
                    ██                            
                   ██                             
                  ██                              
                 ██                               
                ██                                
               ██                                 
              ██                                  
             ██                                   
            ██                                    
           ██                                     
                                                  
                                                  
                                                  
                                                  [/]",

        // ── Frame 2: 揮刀斬擊 (高亮巨刃劃過) ──
        @"[bold white]
                                                  
                            ██                    
                          ████                    
                        ████▓                     
                      ████▓▒                      
                    ████▓▒                        
                  ████▓▒                          
                ████▓▒                            
              ████▓▒                              
            ████▓▒                                
          ████▓▒                                  
        ████▓▒                                    
      ████▓▒                                      
    ████▓▒                                        
  ████▓▒                                          
                                                  
                                                  
                                                  
                                                  
                                                  [/]",

        // ── Frame 3: 擊中爆破 (殘影與衝擊波) ──
        @"[bold red]
                                                  
                                                  
                       ░▒▓█▓▒░                    
                    ░▒▓███████▓▒░                 
                  ░▒▓███████████▓▒░               
                 ░▒▓██  █████  ██▓▒░              
                ░▒▓██   █████   ██▓▒░             
                ░▒▓███████████████▓▒░             
                ░▒▓███████████████▓▒░             
                ░▒▓██   █████   ██▓▒░             
                 ░▒▓██  █████  ██▓▒░              
                  ░▒▓███████████▓▒░               
                    ░▒▓███████▓▒░                 
                       ░▒▓█▓▒░                    
                                                  
                                                  
                                                  
                                                  
                                                  
                                                  [/]",

        // ── Frame 4: 餘波與散開 (火花消散) ──
        @"[bold darkorange3]
                                                  
                                                  
                      .  *  .  *                  
                   *  .  ░ ▒ ░  .  *              
                    . ░ ▒ ▓ ▒ ░ .                 
                   *  ░ ▒ ▓ ▒ ░  *                
                    . ░ ▒ ▓ ▒ ░ .                 
                   *  .  ░ ▒ ░  .  *              
                      .  *  .  *                  
                                                  
                                                  
                                                  
                                                  
                                                  
                                                  
                                                  
                                                  
                                                  
                                                  
                                                  [/]"
    };

    // 💡 播放動畫方法
    public static async Task PlayAsync(string attackerName, string defenderName)
    {
        // 每格停留時間 (毫秒)，可自行調整節奏
        int frameDelay = 100;

        foreach (var frame in Frames)
        {
            AnsiConsole.Clear();

            // 建立 50x20 規格的 Panel 彈窗
            var animPanel = new Panel(new Align(new Markup(frame), HorizontalAlignment.Center))
            {
                Header = new PanelHeader($"[bold red]⚔️ {attackerName} 發動斬擊 ⚔️[/]"),
                Border = BoxBorder.Double,
                BorderStyle = new Style(Color.Red),
                Width = 54,  // 內容 50 + 邊框 4
                Height = 22  // 內容 20 + 邊框 2
            };

            // 畫在螢幕正中央
            AnsiConsole.Write(new Align(animPanel, HorizontalAlignment.Center, VerticalAlignment.Middle));

            await Task.Delay(frameDelay);
        }

        // 動畫結束稍微停頓一下
        await Task.Delay(150);
    }
}