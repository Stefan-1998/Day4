using System.Text.RegularExpressions;

namespace ScratchCards;

public static class Day4
{
    static public int Part1(string inputPath)
    {
        if (!File.Exists(Path.GetFullPath(inputPath))) { throw new ArgumentException($"The path {inputPath} is invalid!"); }
        var cards = File.ReadAllLines(inputPath).Select(x => ParseScratchCard(x)).ToList();
        int points = 0;
        cards.ForEach(card => points += GetNumberOfPoints(GetWinningLots(card.WinningLots, card.YourLots)));

        return points;
    }
    static public int Part2(string inputPath)
    {
        if (!File.Exists(Path.GetFullPath(inputPath))) { throw new ArgumentException($"The path {inputPath} is invalid!"); }
        var originalCards = File.ReadAllLines(inputPath).Select(x => ParseScratchCard(x)).ToList();
        int[] amountPerCardType = Enumerable.Repeat(1, originalCards.Count).ToArray();
        var cardResults = GatherCardResults(originalCards);

        for (int i = 0; i < originalCards.Count; i++)
        {
            for (int j = 0; j < amountPerCardType[i]; j++)
            {
                for (int k = 1; k < cardResults[i + 1] + 1; k++)
                {
                    amountPerCardType[i + k]++;
                }
            }
        }
        return amountPerCardType.Sum();
    }

    static private Dictionary<int, int> GatherCardResults(List<(int Number, IEnumerable<int> WinningLots, IEnumerable<int> YourLots)> cards)
    {
        Dictionary<int, int> cardResults = new();
        foreach (var card in cards)
        {
            if (!cardResults.ContainsKey(card.Number)) { cardResults[card.Number] = GetWinningLots(card.WinningLots, card.YourLots).Count; }
        }
        return cardResults;
    }
    static private (int Number, IEnumerable<int> WinningLots, IEnumerable<int> YourLots) ParseScratchCard(string input)
    {
        var number = GetCardNumber(input.Split(':')[0]);
        var winningLots = ReadOutLots(input.Split(':')[1].Split('|')[0]);
        var yourLots = ReadOutLots(input.Split(':')[1].Split('|')[1]);
        return (Number: number, WinningLots: winningLots, YourLots: yourLots);
    }
    private static int GetCardNumber(string input) => Int32.Parse(new Regex(@"\d+").Match(input).Value);

    private static IEnumerable<int> ReadOutLots(string input) => new Regex(@"\d+").Matches(input).Select(x => Int32.Parse(x.Value));
    private static List<int> GetWinningLots(IEnumerable<int> winningNumbers, IEnumerable<int> yourLots) => yourLots.Where(x => winningNumbers.Contains(x)).ToList();

    private static int GetNumberOfPoints(List<int> winningLots)
    {
        int numberOfWonLots = winningLots.Count;
        if (numberOfWonLots == 0) { return 0; }
        if (numberOfWonLots == 1) { return 1; }
        return (int)Math.Pow(2d, (double)numberOfWonLots - 1);
    }
}
