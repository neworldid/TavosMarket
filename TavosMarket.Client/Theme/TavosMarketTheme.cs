using MudBlazor;
using MudBlazor.Utilities;

namespace TavosMarket.Client.Theme;

public static class TavosMarketTheme
{
	public static readonly MudTheme Theme = new()
	{
		PaletteLight = new PaletteLight
		{
			Primary = new MudColor("#555724"),
			Surface = new MudColor("#ffffff"),
			Background = new MudColor("#f7f6f2"),
			AppbarBackground = new MudColor("#ffffff"),
			AppbarText = new MudColor("#374151"),
			TextPrimary = new MudColor("#111827"),
			LinesInputs = new MudColor("#DDD9C9"),
			ActionDefault = new MudColor("#9ca3af"),
		},
		LayoutProperties = new LayoutProperties
		{
			DefaultBorderRadius = "6px",
		}
	};
}