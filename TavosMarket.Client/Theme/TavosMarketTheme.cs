using MudBlazor;
using MudBlazor.Utilities;

namespace TavosMarket.Client.Theme;

public static class TavosMarketTheme
{
	public static readonly MudTheme Theme = new()
	{
		PaletteLight = new PaletteLight
		{
			Primary = "#555724",
			Secondary = "#8c8f4b",
			Info = "#4a8a77",
			Success = "#3d6b3d",
			Warning = "#b38b3d",
			Error = "#a13d3d",
			Surface = "#ffffff",
			Background = "#fbfbf8",
			AppbarBackground = "#555724",
			AppbarText = "#ffffff",
			TextPrimary = "#27272a",
			TextSecondary = "#52525b",
			LinesInputs = "#DDD9C9",
			ActionDefault = "#555724",
			TableLines = "#e5e7eb",
			Divider = "#e5e7eb",
			OverlayLight = "rgba(255,255,255,0.5)"
		},
		LayoutProperties = new LayoutProperties
		{
			DefaultBorderRadius = "8px"
		}
	};
}