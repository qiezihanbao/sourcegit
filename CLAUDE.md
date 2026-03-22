
## Project Overview

SourceGit is an open-source Git GUI client built with **C#/.NET** and **Avalonia UI** framework. It provides a visual interface for Git operations with support for Windows, macOS, and Linux.

## Build Commands

### Development
```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run --project src/SourceGit.csproj
```

### Manual Build/Publish
```bash
# Publish for specific runtime (replace $RUNTIME_IDENTIFIER with win-x64, linux-x64, osx-x64, etc.)
dotnet publish -c Release -r $RUNTIME_IDENTIFIER -o $DESTINATION_FOLDER src/SourceGit.csproj
```

## Architecture

### Project Structure
- **src/SourceGit.csproj** - Main application project targeting .NET 10
- **src/Views/** - Avalonia AXAML view files (UI definitions)
- **src/ViewModels/** - ViewModels implementing MVVM pattern (using CommunityToolkit.Mvvm)
- **src/Models/** - Data models and business logic
- **src/Converters/** - Value converters for data binding
- **src/Controls/** - Custom user controls (ChromelessWindow, etc.)
- **src/Resources/** - Resources including:
  - Icons.axaml - Icon definitions
  - Themes.axaml - Color theme definitions (Light/Dark)
  - Styles.axaml - Global styles
  - Locales/*.axaml - Localization files

### MVVM Pattern
The application strictly follows the Model-View-ViewModel (MVVM) pattern:
- **Views** (.axaml) define the UI structure
- **ViewModels** contain the presentation logic and data binding properties
- **Models** contain business data and logic

ViewModels typically use:
- `CommunityToolkit.Mvvm.SourceGenerator` for generating observable properties and commands
- `[ObservableProperty]` attribute for auto-generated properties
- `[RelayCommand]` attribute for command implementations

### Key UI Classes

#### Windows
- **Launcher** - Main window showing repository workspace and launcher pages
- **Repository** - Main repository view window
- Various dialog windows (Clone, Checkout, Merge, etc.)

#### Custom Controls
- **ChromelessWindow** - Base window class with custom title bar
- **CaptionButtons** - Custom window caption buttons (minimize, maximize, close)

## Theming System

### Theme Structure
Themes are defined in `src/Resources/Themes.axaml` using Avalonia's `ThemeDictionaries`:

```xml
<ResourceDictionary.ThemeDictionaries>
  <ResourceDictionary x:Key="Light">
    <!-- Light theme colors -->
  </ResourceDictionary>
  <ResourceDictionary x:Key="Dark">
    <!-- Dark theme colors -->
  </ResourceDictionary>
</ResourceDictionary.ThemeDictionaries>
```

### Color Keys (Customization Points)
All colors use the `Color.` prefix:
- `Color.Window` - Main window background
- `Color.TitleBar` - Title bar background
- `Color.ToolBar` - Toolbar background
- `Color.Popup` - Popup/menu background
- `Color.Contents` - Content area background
- `Color.Border0/1/2` - Border colors at different levels
- `Color.FG1/FG2` - Foreground text colors
- `Color.Diff.*` - Diff view colors
- `Color.Conflict.*` - Merge conflict colors

Brushes are defined from colors: `Brush.Window`, `Brush.TitleBar`, etc.

### Custom Theme Support
Users can create custom themes (see [sourcegit-theme](https://github.com/sourcegit-scm/sourcegit-theme)).
Theme overrides are stored in `Models/ThemeOverrides.cs`:
```csharp
public class ThemeOverrides
{
    public Dictionary<string, Color> BasicColors { get; set; }
    public double GraphPenThickness { get; set; }
    public double OpacityForNotMergedCommits { get; set; }
    public List<Color> GraphColors { get; set; }
}
```

## Styling System

### Global Styles
Located in `src/Resources/Styles.axaml`, this file contains:
- Window styles (including custom window frame with resize borders)
- Control styles (Button, TextBox, ComboBox, CheckBox, etc.)
- ScrollBar customization
- Custom style classes (icon_button, flat, etc.)

### Style Classes
Common CSS-like classes for controls:
- `.icon_button` - Transparent icon-only buttons
- `.flat` - Flat button style with border
- `.flat.primary` - Primary action button (uses accent color)
- `.custom_window_frame` - Custom frame window
- `.small` - Smaller text size
- `.bold` - Bold text

### Fonts
- `Fonts.Default` - Inter font family (UI default)
- `Fonts.Monospace` - JetBrains Mono (code/diff views)

Font sizes are bound to `Preferences.DefaultFontSize` for scaling.

## UI Extension Points

### Adding New Views
1. Create `.axaml` file in `src/Views/`
2. Create corresponding `.axaml.cs` code-behind file
3. Create ViewModel in `src/ViewModels/`
4. Follow MVVM pattern with proper data binding

### Adding New Styles
1. Add to `src/Resources/Styles.axaml` for global styles
2. Use CSS-like selectors: `Button.my_class:pointerover`
3. Reference dynamic resources: `{DynamicResource Brush.Window}`

### Adding Icons
Icons are defined in `src/Resources/Icons.axaml` as PathGeometry objects.
Reference using: `Data="{StaticResource Icons.IconName}"`

### Localization
Add translation keys to locale files in `src/Resources/Locales/`:
- `en_US.axaml` - Base English translations
- `zh_CN.axaml`, `ja_JP.axaml`, etc. - Other languages

Use in XAML: `{DynamicResource Text.TranslationKey}`

## Important Implementation Details

### Zoom/Scaling
Application-wide zoom is controlled by `Preferences.Instance.Zoom` and applied via `LayoutTransformControl` in window templates.

### ScrollBars
Auto-hiding scrollbars can be disabled via `Preferences.UseAutoHideScrollBars`.
Static scrollbars are applied via `.static_scrollbar` style class.

### Native Menu Integration
The application uses Avalonia's `NativeMenu` for platform-specific menu integration (see `App.axaml`).

### Custom Window Frame
Windows use a custom frame with resize borders (`Window.custom_window_frame` class).
Maximized windows have different padding and border behavior.

### Platform-Specific Code
Use `OnPlatform` markup extension for platform differences:
```xml
{OnPlatform Windows_Value, macOS=MacValue, Linux=LinuxValue}
```

### Data Binding Patterns
- ViewModel properties: `{Binding PropertyName}`
- Commands: `{Binding CommandName}`
- Dynamic resources: `{DynamicResource ResourceKey}`
- Static resources: `{StaticResource ResourceKey}`

## Key Dependencies

- **Avalonia 11.3.12** - UI framework
- **AvaloniaEdit** - Code editor component (for diff viewing)
- **CommunityToolkit.Mvvm** - MVVM helpers
- **LiveChartsCore** - Charts and graphs
- **Azure.AI.OpenAI / OpenAI** - AI commit message generation
