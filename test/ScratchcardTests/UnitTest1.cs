using ScratchCards;
namespace ScratchcardTests;

public class Tests
{
    [TestCase(@"../../../Example.txt", 13)]
    [TestCase(@"../../../Input.txt", 24706)]
    public void RightAmountOfPointsShouldForTheScratchcards(string inputPath, int winningPoints)
    {
        Assert.That(Day4.Part1(inputPath), Is.EqualTo(winningPoints));
    }
    [TestCase(@"../../../Example.txt", 30)]
    [TestCase(@"../../../Input.txt", 13114317)]
    public void RightAmountOfScratchcards(string inputPath, int winningPoints)
    {
        Assert.That(Day4.Part2(inputPath), Is.EqualTo(winningPoints));
    }
}
