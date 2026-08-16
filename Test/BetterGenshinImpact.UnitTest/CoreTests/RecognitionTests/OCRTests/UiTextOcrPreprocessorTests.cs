using BetterGenshinImpact.Core.Recognition.OCR;
using OpenCvSharp;

namespace BetterGenshinImpact.UnitTest.CoreTests.RecognitionTests.OCRTests;

public class UiTextOcrPreprocessorTests
{
    [Fact]
    public void CreateWhiteTextImage_RemovesSaturatedBackgroundAndInvertsWhiteText()
    {
        using var src = new Mat(20, 40, MatType.CV_8UC3, new Scalar(40, 180, 40));
        Cv2.Rectangle(src, new Rect(10, 5, 15, 8), Scalar.White, -1);

        using var result = UiTextOcrPreprocessor.CreateWhiteTextImage(src);

        Assert.Equal(1, result.Channels());
        Assert.Equal(255, result.At<byte>(0, 0));
        Assert.Equal(0, result.At<byte>(8, 15));
    }

    [Fact]
    public void CreateWhiteTextImage_AcceptsGrayInput()
    {
        using var src = new Mat(10, 10, MatType.CV_8UC1, Scalar.White);

        using var result = UiTextOcrPreprocessor.CreateWhiteTextImage(src);

        Assert.Equal(0, result.At<byte>(5, 5));
    }
}
