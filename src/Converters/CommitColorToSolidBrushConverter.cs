using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SourceGit.Converters
{
    // 返回提交的路线图颜色的实色笔刷（用于左侧高亮条）
    public class CommitColorToSolidBrushConverter : IValueConverter
    {
        // Default graph colors matching CommitGraph.s_defaultPenColors
        private static readonly Color[] s_graphColors =
        [
            Colors.Orange,
            Colors.ForestGreen,
            Colors.Turquoise,
            Colors.Olive,
            Colors.Magenta,
            Colors.Red,
            Colors.Khaki,
            Colors.Lime,
            Colors.RoyalBlue,
            Colors.Teal,
        ];

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // 检查是否启用渐变效果
            if (!ViewModels.Preferences.Instance.EnableRowGradientEffects)
            {
                return null;
            }

            if (value is Models.Commit commit)
            {
                int colorIndex = commit.Color;

                // Get the color from the default colors list
                if (colorIndex >= 0 && colorIndex < s_graphColors.Length)
                {
                    return new SolidColorBrush(s_graphColors[colorIndex]);
                }
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
