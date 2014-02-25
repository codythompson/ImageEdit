using System;
using System.Drawing;

namespace ImageEdit
{
    public class FadeLine1 : Command, IGraphicsExecutable
    {
        public const string FADELINE1_COMMAND_ARG = "fadeline1";
        public const string FADELINE1_DESCRIPTION = "A command that draws a line that fades from one color to another.";
        public const string FADELINE1_USAGE = "fadeline1 <start x>,<start y> <end x>,<end y> <int width> <start color>,<end color>";
        public const string ANGLED_FLAG = "-a";

        private Point StartPos, EndPos, StartEndColors;
        private int width;
        private bool angled;

        public FadeLine1() : base(FADELINE1_COMMAND_ARG, FADELINE1_DESCRIPTION, FADELINE1_USAGE) { }

        /*
         * 0         1                   2               3           4
         * fadeline1 <start x>,<start y> <end x>,<end y> <int width> <start color>,<end color> 
         */
        public override void ParseArgs(string[] args)
        {
            StartPos = ArgUtils.ParsePoint(CommandArg, args, 1);
            EndPos = ArgUtils.ParsePoint(CommandArg, args, 2);
            width = ArgUtils.ParseInt(CommandArg, args, 3);
            StartEndColors = ArgUtils.ParsePoint(CommandArg, args, 4);
            angled = ArgUtils.HasFlag(args, ANGLED_FLAG);
        }

        public virtual ExecutableReturnValues Execute(Graphics g)
        {
            // TODO change to use direction instead of just down
            Pen p = new Pen(Color.FromArgb(StartEndColors.X), 1);
            for (int i = 0; i < width; i++)
            {
                float progress = (float)i / (float)width;
                p.Color = ColorUtils.Lerp(Color.FromArgb(StartEndColors.X), Color.FromArgb(StartEndColors.Y), progress);
                g.DrawLine(p, StartPos, EndPos);
                StartPos.Y += 1;
                EndPos.Y += 1;
                if (angled)
                {
                    StartPos.X++;
                    EndPos.X--;
                }
            }

            return new ExecutableReturnValues();
        }
    }
}