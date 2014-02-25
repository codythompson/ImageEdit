using System;
using System.Drawing;

namespace ImageEdit
{
    public class BoxMe : Command, IGraphicsExecutable
    {
        public const string BOXME_COMMAND_ARG = "boxme";
        public const string BOXME_USAGE = "boxme <start x>,<start y> <width>,<height> <start color>,<end color> <int boxCount> <int borderpercentsize>";

        private Rectangle rect;
        private Color startColor, endColor;
        private int boxCount;
        private PointF boxSize, boxSizeWithBorder;

        public BoxMe() : base(BOXME_COMMAND_ARG, BOXME_COMMAND_ARG, BOXME_USAGE) { }

        /* 0     1                   2                3                         4             5
         * boxme <start x>,<start y> <width>,<height> <start color>,<end color> <int boxCount> <float borderpercentsize>
         */
        public override void ParseArgs(string[] args)
        {
            Point startPos = ArgUtils.ParsePoint(CommandArg, args, 1);
            Point dims = ArgUtils.ParsePoint(CommandArg, args, 2);
            rect = new Rectangle(startPos.X, startPos.Y, dims.X, dims.Y);

            Point colors = ArgUtils.ParsePoint(CommandArg, args, 3);
            startColor = Color.FromArgb(colors.X);
            endColor = Color.FromArgb(colors.Y);

            boxCount = ArgUtils.ParseInt(CommandArg, args, 4);
            float boxCountF = (float)boxCount;
            float borderPercentSize = (float)ArgUtils.ParseInt(CommandArg, args, 5);
            borderPercentSize = Math.Min(Math.Max(borderPercentSize, 0), 100) / 100f;
            boxSizeWithBorder = new PointF((float)dims.X / boxCountF, (float)dims.Y / boxCountF);
            PointF borderSize = new PointF(boxSizeWithBorder.X * borderPercentSize, boxSizeWithBorder.Y * borderPercentSize);
            boxSize = new PointF(boxSizeWithBorder.X - borderSize.X, boxSizeWithBorder.Y - borderSize.Y);
        }

        public virtual ExecutableReturnValues Execute(Graphics g)
        {
            for (int j = 0; j < boxCount; j++)
            {
                for (int i = 0; i < boxCount; i++)
                {
                    float distance = calcluateDistance(i, j);
                    Color color = ColorUtils.Lerp(startColor, endColor, distance);
                    SolidBrush sb = new SolidBrush(color);
                    PointF startPoint = new PointF(rect.X + (boxSizeWithBorder.X * i), rect.Y + boxSizeWithBorder.Y * j);
                    RectangleF fillRect = new RectangleF(startPoint, new SizeF(boxSize.X, boxSize.Y));
                    g.FillRectangle(sb, fillRect);
                }
            }

            return new ExecutableReturnValues();
        }

        private float calcluateDistance(int i, int j)
        {
            int x2 = (int)Math.Pow(i, 2);
            int y2 = (int)Math.Pow(j, 2);
            float distance = (float)Math.Sqrt(x2 + y2);
            return distance / (float)Math.Sqrt(2 * Math.Pow(boxCount, 2));
        }
    }
}