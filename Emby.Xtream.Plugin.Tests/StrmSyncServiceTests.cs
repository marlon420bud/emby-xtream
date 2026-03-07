using Emby.Xtream.Plugin.Service;
using Xunit;

namespace Emby.Xtream.Plugin.Tests
{
    public class StrmSyncServiceTests
    {
        // -----------------------------------------------------------------
        // StripEpisodeTitleDuplicate
        // -----------------------------------------------------------------

        [Theory]
        // Provider embeds short series name (without year) + episode code
        [InlineData("Yago - S01E33 - Episode 33", "Yago (2016)", 1, 33, "Episode 33")]
        // Provider embeds full series name + episode code — country prefix "EN" is left as-is
        [InlineData("EN - American Gigolo - S01E01", "American Gigolo", 1, 1, "EN")]
        // Title is just the episode code — no human-readable part left
        [InlineData("Show S02E05", "Show", 2, 5, "")]
        // Title has no episode code — returned as-is
        [InlineData("The Lost City", "Show", 1, 1, "The Lost City")]
        // Title is null/empty — returns empty
        [InlineData(null, "Show", 1, 1, "")]
        [InlineData("", "Show", 1, 1, "")]
        // Code is case-insensitive
        [InlineData("Yago - s01e33 - Episode 33", "Yago (2016)", 1, 33, "Episode 33")]
        // No leading series prefix — title starts with episode code
        [InlineData("S03E07 - The Aftermath", "SomeShow", 3, 7, "The Aftermath")]
        // Issue #9 — provider embeds series name + code; series name in Xtream matches exactly
        [InlineData("EN - Barbie It Takes Two - S01E01", "EN - Barbie It Takes Two", 1, 1, "")]
        // Issue #9 — provider series name differs from Xtream series name; Pass 2 strips code
        [InlineData("EN - Arcane - S01E01", "4K-NF - Arcane (2021) (4K-NF)", 1, 1, "")]
        // Issue #9 — provider prefix + episode title preserved after stripping series + code
        [InlineData("EN - G.I. Joe A Real American Hero (1983) (4K-NF) - S01E01 - The M.A.S.S. Device The Cobra Strikes (1)",
            "EN - G.I. Joe A Real American Hero (1983) (4K-NF)", 1, 1, "The M.A.S.S. Device The Cobra Strikes (1)")]
        [InlineData("EN - Batman The Animated Series (1992) (4K-NF) - S01E01 - The Cat and the Claw (1)",
            "EN - Batman The Animated Series (1992) (4K-NF)", 1, 1, "The Cat and the Claw (1)")]
        public void StripEpisodeTitleDuplicate_ReturnsExpected(
            string title, string seriesName, int season, int episode, string expected)
        {
            var result = StrmSyncService.StripEpisodeTitleDuplicate(title, seriesName, season, episode);
            Assert.Equal(expected, result);
        }
    }
}
