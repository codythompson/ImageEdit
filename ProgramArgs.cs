using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageEdit
{
    public class ProgramArgs
    {
        private const char ARG_LABEL_INDICATOR = '-';
        private const char ARG_SPLIT_SYMBOL = '=';

        private Dictionary<string, string> argPairs;

        //TODO: Error checking/handling
        //index out of range, etc.
        public ProgramArgs(string[] args)
        {
            argPairs = new Dictionary<string, string>();

            int i = 0;
            string arg;
            while (i < args.Length)
            {
                arg = args[i];
                if (arg[0] == ARG_LABEL_INDICATOR)
                {
                    //todo check that there actually is a next arg
                    //and that there actually are characters after '-'
                    argPairs.Add(arg.Substring(1), args[++i]);
                }
                else if (arg.Contains(ARG_SPLIT_SYMBOL))
                {
                    string[] splitArg = arg.Split(new char[] {ARG_SPLIT_SYMBOL}, 2);
                    if (splitArg.Length > 1)
                    {
                        argPairs.Add(splitArg[0], splitArg[1]);
                    }
                    else
                    {
                        argPairs.Add(splitArg[0], null);
                    }
                }
                else
                {
                    argPairs.Add(arg, null);
                }
                i++;
            }
        }

        public string this[string label]
        {
            get { return argPairs[label]; }
        }

        public bool TryGetInt(string argName, out int value)
        {
            return int.TryParse(this[argName], out value);
        }

        public bool HasArg(string label)
        {
            return argPairs.ContainsKey(label);
        }
    }

    public class ImageEditArgs : ProgramArgs
    {
        public const string OUTPUT_FILE_LABEL = "o";
        public const string WIDTH_LABEL = "w";
        public const string HEIGHT_LABEL = "h";

        public ImageEditArgs(string[] args) : base(args) { }

        public string GetOutputFilename()
        {
            if (HasArg(OUTPUT_FILE_LABEL))
            {
                return this[OUTPUT_FILE_LABEL];
            }
            else
            {
                return null;
            }
        }

        public bool TryGetWidth(out int width)
        {
            width = -1;
            return HasArg(WIDTH_LABEL) && TryGetInt(WIDTH_LABEL, out width);
        }

        public bool TryGetHeight(out int height)
        {
            height = -1;
            return HasArg(HEIGHT_LABEL) && TryGetInt(HEIGHT_LABEL, out height);
        }

        public bool HasOutputFileName()
        {
            return HasArg(OUTPUT_FILE_LABEL);
        }

        public bool HasImageWidth()
        {
            return HasArg(WIDTH_LABEL);
        }

        public bool HasImageHeight()
        {
            return HasArg(HEIGHT_LABEL);
        }
    }
}