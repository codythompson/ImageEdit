using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageEdit
{
    public class SaveBMP : Command, IBMPExecutable
    {
        public const string SAVEBMP_COMMAND_ARG = "savebmp";
        public const string SAVEBMP_DESCRIPTION = "This command saves the current image as a bitmap file";
        public const string BMP_EXT = ".bmp";

        public SaveBMP() : base(SAVEBMP_COMMAND_ARG, SAVEBMP_DESCRIPTION, SAVEBMP_COMMAND_ARG) { }

        public override void ParseArgs(string[] args)
        {
            if (args.Length > 1)
            {
                throw new TooManyCommandArgsException(args[0], 1, args.Length);
            }
        }

        public virtual ExecutableReturnValues Execute(Bitmap bmp, string filename)
        {
            bmp.Save(filename + BMP_EXT, ImageFormat.Bmp);

            return null;
        }
    }
}