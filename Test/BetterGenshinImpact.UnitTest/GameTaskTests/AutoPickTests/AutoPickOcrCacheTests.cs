using BetterGenshinImpact.GameTask.AutoPick;

namespace BetterGenshinImpact.UnitTest.GameTaskTests.AutoPickTests;

public class AutoPickOcrCacheTests
{
    [Fact]
    public void GetMeanAbsoluteDifference_IdenticalFingerprints_ReturnsZero()
    {
        byte[] fingerprint = [0, 32, 128, 255];

        var difference = AutoPickTrigger.GetMeanAbsoluteDifference(fingerprint, fingerprint);

        Assert.Equal(0, difference);
    }

    [Fact]
    public void GetMeanAbsoluteDifference_ChangedText_ExceedsCacheThreshold()
    {
        byte[] previous = [0, 0, 0, 0];
        byte[] current = [0, 0, 0, 255];

        var difference = AutoPickTrigger.GetMeanAbsoluteDifference(previous, current);

        Assert.Equal(63.75, difference);
        Assert.True(difference > 4);
    }

    [Fact]
    public void GetMeanAbsoluteDifference_DifferentSizes_CannotMatch()
    {
        var difference = AutoPickTrigger.GetMeanAbsoluteDifference([0], [0, 0]);

        Assert.Equal(double.MaxValue, difference);
    }
}
