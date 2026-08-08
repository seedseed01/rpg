namespace rpg;

public static class EnumExtensions
{
    // 1. 狀態中文轉換
    public static string ToChinese(this CurrentStatus status) => status switch
    {
        CurrentStatus.Normal => "正常",
        CurrentStatus.Poisoned => "中毒",
        CurrentStatus.ManaDrain => "迷幻",
        CurrentStatus.Paralyzed => "麻痺",
        CurrentStatus.Asleep => "睡眠",
        CurrentStatus.Weakened => "無力",
        CurrentStatus.Muddled => "無神",
        _ => status.ToString()
    };

    // 2. 屬性中文轉換
    public static string ToChinese(this CurrentType type) => type switch
    {
        CurrentType.Normal => "無",
        CurrentType.Fire => "火",
        CurrentType.Water => "水",
        CurrentType.Wind => "風",
        CurrentType.Earth => "土",
        CurrentType.Light => "光",
        CurrentType.Dark => "暗",
        _ => type.ToString()
    };

    // 3. 個性/性格中文轉換
    public static string ToChinese(this Personality nature) => nature switch
    {
        Personality.Aggressive => "暴躁",
        Personality.Cautious => "謹慎",
        Personality.Focused => "專注",
        Personality.Meditative => "冥想",
        Personality.Swift => "神行",
        Personality.Balanced => "平衡",
        _ => nature.ToString()
    };
}