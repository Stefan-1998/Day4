using System.Text.RegularExpressions;

namespace ScratchCards;

public class ScratchCard
{
    public readonly int Number;
    public readonly IEnumerable<int> WinningNumbers;
    public readonly IEnumerable<int> Lots;
    public readonly int NumberOfWonLots;
    public ScratchCard(string input)
    {
        Number = GetCardNumber(input.Split(':')[0]);
        WinningNumbers = ReadOutLots(input.Split(':')[1].Split('|')[0]);
        Lots = ReadOutLots(input.Split(':')[1].Split('|')[1]);
        NumberOfWonLots = GetNumberOfWinningLots();
    }
    private int GetCardNumber(string input) => Int32.Parse(new Regex(@"\d+").Match(input).Value);
    private IEnumerable<int> ReadOutLots(string input) => new Regex(@"\d+").Matches(input).Select(x => Int32.Parse(x.Value));
    private int GetNumberOfWinningLots() => Lots.Where(x => WinningNumbers.Contains(x)).ToList().Count;

    public int GetNumberOfPoints()
    {
        int numberOfWonLots = GetNumberOfWinningLots();
        if (numberOfWonLots == 0) { return 0; }
        if (numberOfWonLots == 1) { return 1; }
        return (int)Math.Pow(2d, (double)numberOfWonLots - 1);
    }
}
public static class Day4
{
    static public int Part1(string inputPath)
    {
        if (!File.Exists(Path.GetFullPath(inputPath))) { throw new ArgumentException($"The path {inputPath} is invalid!"); }
        var cards = File.ReadAllLines(inputPath).Select(x => new ScratchCard(x)).ToList();
        int points = 0;
        cards.ForEach(card => points += card.GetNumberOfPoints());

        return points;
    }
    static public int Part2(string inputPath)
    {
        if (!File.Exists(Path.GetFullPath(inputPath))) { throw new ArgumentException($"The path {inputPath} is invalid!"); }
        var originalCards = File.ReadAllLines(inputPath).Select(x => new ScratchCard(x)).ToList();
        int[] amountPerCardType = Enumerable.Repeat(1, originalCards.Count).ToArray();

        return CalculateAmountOfOriginalAndCopiedCards(amountPerCardType, originalCards);
    }
    static private int CalculateAmountOfOriginalAndCopiedCards(int[] amountPerCardType, List<ScratchCard> originalCards)
    {
        for (int i = 0; i < originalCards.Count; i++)
        {
            AddCopiesToFollowingCards(i, ref amountPerCardType, originalCards);
        }
        return amountPerCardType.Sum();
    }
    static private void AddCopiesToFollowingCards(int currentCardIndex, ref int[] amountPerCardType, List<ScratchCard> originalCards)
    {
        for (int j = 0; j < amountPerCardType[currentCardIndex]; j++)
        {
            for (int k = 1; k < originalCards[currentCardIndex].NumberOfWonLots + 1; k++)
            {
                amountPerCardType[currentCardIndex + k]++;
            }
        }
    }
}
