using System;
using OpenCvSharp;

namespace BetterGenshinImpact.Core.Recognition.OCR;

/// <summary>
///     Prepares bright, low-saturation game UI text for OCR by removing the dynamic scene behind it.
/// </summary>
public static class UiTextOcrPreprocessor
{
    private static readonly Scalar WhiteTextLowerBound = new(0, 0, 160);
    private static readonly Scalar WhiteTextUpperBound = new(180, 96, 255);

    /// <summary>
    ///     Converts white UI text to black text on a white background.
    /// </summary>
    public static Mat CreateWhiteTextImage(Mat src)
    {
        ArgumentNullException.ThrowIfNull(src);
        if (src.Empty())
        {
            throw new ArgumentException("OCR source image must not be empty.", nameof(src));
        }

        Mat? bgr = null;
        try
        {
            bgr = src.Channels() switch
            {
                4 => src.CvtColor(ColorConversionCodes.BGRA2BGR),
                3 => src,
                1 => src.CvtColor(ColorConversionCodes.GRAY2BGR),
                var channels => throw new ArgumentException(
                    $"Unsupported OCR source channel count: {channels}.", nameof(src))
            };

            using var hsv = bgr.CvtColor(ColorConversionCodes.BGR2HSV);
            var textMask = hsv.InRange(WhiteTextLowerBound, WhiteTextUpperBound);
            Cv2.BitwiseNot(textMask, textMask);
            return textMask;
        }
        finally
        {
            if (bgr != null && !ReferenceEquals(bgr, src))
            {
                bgr.Dispose();
            }
        }
    }
}
