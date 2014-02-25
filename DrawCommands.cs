using System;
using System.Drawing;

namespace ImageEdit
{
    public class DrawCircleCommand : Command, IGraphicsExecutable
    {
        public const string DRAW_CIRCLE_COMMAND_ARG = "circle";

        //                                       0      1           2                          3
        public const string DRAW_CIRCLE_USAGE = "circle <int color> <left bound>,<right bound> <int radius>";

        public DrawCircleCommand() : base(DRAW_CIRCLE_COMMAND_ARG, DRAW_CIRCLE_COMMAND_ARG, DRAW_CIRCLE_USAGE) { }

        private Brush brush;
        private Rectangle rect;

        public override void ParseArgs(string[] args)
        {
            int color = ArgUtils.ParseInt(args[0], args, 1);
            brush = new SolidBrush(Color.FromArgb(color));
            Point topLeft = ArgUtils.ParsePoint(args[0], args, 2);
            int width = ArgUtils.ParseInt(args[0], args, 3) * 2;
            rect = new Rectangle(topLeft.X, topLeft.Y, width, width);
        }

        public virtual ExecutableReturnValues Execute(Graphics g)
        {
            g.FillEllipse(brush, rect);

            return null;
        }
    }
}