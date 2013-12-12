using System;
using System.Drawing;
using System.Globalization;

namespace ImageEdit
{
    public static class ArgUtils
    {
        public const string HEX_SPECIFIER = "0x";

        public static bool ParseInt(string numberStr, out int number)
        {
            if (numberStr.StartsWith("0x"))
            {
                numberStr = numberStr.Substring(2);
                return int.TryParse(numberStr, NumberStyles.AllowHexSpecifier, CultureInfo.InstalledUICulture, out number);
            }
            else
            {
                return int.TryParse(numberStr, out number);
            }
        }

        public static int ParseInt(string commandArg, string[] args, int argIndex)
        {
            if (argIndex >= args.Length)
            {
                throw new MissingCommandArgException(commandArg, argIndex, args.Length);
            }

            int n;
            if (!ParseInt(args[argIndex], out n))
            {
                throw new InvalidCommandArgException(commandArg, "<integer>", argIndex, args[argIndex]);
            }
            return n;
        }

        public static Point ParsePoint(string commandArg, string[] args, int argIndex)
        {
            if (argIndex >= args.Length)
            {
                throw new MissingCommandArgException(commandArg, argIndex, args.Length);
            }

            string[] xAndY = args[argIndex].Split(',');
            if (xAndY.Length != 2)
            {
                throw new InvalidCommandArgException(commandArg, "<integer x>,<integer y>", argIndex, args[argIndex]);
            }
            int x;
            if (!ParseInt(xAndY[0], out x))
            {
                throw new InvalidCommandArgException(commandArg, "<integer x>,<integer y>", argIndex, args[argIndex]);
            }
            int y;
            if (!ParseInt(xAndY[1], out y))
            {
                throw new InvalidCommandArgException(commandArg, "<integer x>,<integer y>", argIndex, args[argIndex]);
            }

            return new Point(x, y);
        }
    }
}