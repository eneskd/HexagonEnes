namespace HexagonalGrid
{
    public static class HexTransform
    {
        // for conversion 
        private const float Sqrt3 = 1.7320508075688773f;
        public static readonly float EdgeLength = 2 / Sqrt3;


        private static readonly float[] X = new float[2] {Sqrt3, 1};
        private static readonly float[] Y = new float[2] {0, 2};


        private static readonly float[] InvX = new float[2] {1/Sqrt3, -0.5f/Sqrt3};
        private static readonly float[] InvY = new float[2] {0, 0.5f};

        public static readonly float[][] Transform = new float[2][] {X, Y};
        public static readonly float[][] InvTransform = new float[2][] {InvX, InvY};
    }
}