namespace Recognizer
{
    internal static class HoughTransformTask
    {
        public static Line[] HoughAlgorithm(double[,] originalImage)
        {
            var width = originalImage.GetLength(0);
            var height = originalImage.GetLength(1);
            
            return new[]
            {
                new Line(0, 0, width, height),
                new Line(0, height, width, 0)
            };
        }
    }
}