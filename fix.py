import os
import glob

for filepath in glob.glob('src/Views/*.axaml'):
    try:
        with open(filepath, 'r', encoding='utf-8-sig') as f:
            content = f.read()
        modified = content.replace('<Grid Grid.Row="0" Height="28" IsVisible="{Binding !#ThisControl.UseSystemWindowFrame}">', '<Grid Grid.Row="0" Height="36" IsVisible="{Binding !#ThisControl.UseSystemWindowFrame}">')
        if modified != content:
            with open(filepath, 'w', encoding='utf-8-sig') as f:
                f.write(modified)
            print(f'Modified {filepath}')
    except Exception as e:
        print(f"Skipping {filepath} due to {e}")
