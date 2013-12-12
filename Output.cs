using System;

namespace ImageEdit
{
    public static class Output
    {
        public const ConsoleColor STANDARD_FOREGROUND_COLOR_LIGHT = ConsoleColor.White;
        public const ConsoleColor STANDARD_FOREGROUND_COLOR_DARK = ConsoleColor.Gray;

        public const ConsoleColor ERROR_MESSAGE_FOREGROUND_COLOR = ConsoleColor.Red;
        public const ConsoleColor ERROR_MESSAGE_BACKGROUND_COLOR = ConsoleColor.DarkGray;

        public const ConsoleColor WARNING_MESSAGE_FOREGROUND_COLOR = ConsoleColor.DarkYellow;
        public const ConsoleColor WARNING_MESSAGE_BACKGROUND_COLOR = ConsoleColor.DarkGray;

        public const ConsoleColor INITIAL_PROMPT_FOREGROUND_COLOR = ConsoleColor.Green;
        public const ConsoleColor INITIAL_PROMPT_BACKGROUND_COLOR = ConsoleColor.DarkGray;

        public const ConsoleColor COMMAND_PROMPT_FOREGROUND_COLOR = ConsoleColor.Magenta;
        public const ConsoleColor COMMAND_PROMPT_BACKGROUND_COLOR = ConsoleColor.DarkGray;

        private const string INITIAL_PROMPT_FORMAT_STRING = "ImageEdit - version 0.1\noutput file name ='{0}'\n\nplease enter a command";
        private const string COMMAND_PROMPT = ">";

        /*
         * PADDING DOESN"T WORK WITH STRINGS WILL LINEBREAKS IN THE MIDDLE
         */
        public static void DisplayMessage(string message, bool newLine, bool useErrorIO)
        {
            if (newLine)
            {
                //message = message.PadRight(80, ' ') + "\n";
                message += "\n";
            }

            if (useErrorIO)
            {
                Console.Error.WriteLine(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }

        public static void DisplayMessage(string message)
        {
            DisplayMessage(message, true, false);
        }

        public static void DisplayColoredMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor, bool newLine, bool useErrorIO)
        {
            ConsoleColor originalFG = Console.ForegroundColor;
            ConsoleColor originalBG = Console.BackgroundColor;

            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;

            if (newLine)
            {
                //message = message.PadRight(80, ' ') + "\n";
                message += "\n";
            }

            if (useErrorIO)
            {
                Console.Error.Write(message);
            }
            else
            {
                Console.Write(message);
            }

            Console.ForegroundColor = originalFG;
            Console.BackgroundColor = originalBG;
        }

        public static void DisplayErrorMessage(string message)
        {
            DisplayColoredMessage(message, ERROR_MESSAGE_FOREGROUND_COLOR, ERROR_MESSAGE_BACKGROUND_COLOR, true, true);
        }

        public static void DisplayWarningMessage(string message)
        {
            DisplayColoredMessage(message, WARNING_MESSAGE_FOREGROUND_COLOR, WARNING_MESSAGE_BACKGROUND_COLOR, true, false);
        }

        public static void DisplayInitialPrompt(string outputFileName)
        {
            DisplayColoredMessage(string.Format(INITIAL_PROMPT_FORMAT_STRING, outputFileName), INITIAL_PROMPT_FOREGROUND_COLOR, INITIAL_PROMPT_BACKGROUND_COLOR, true, false);
        }

        public static void DisplayCommandPrompt()
        {
            DisplayColoredMessage(COMMAND_PROMPT, COMMAND_PROMPT_FOREGROUND_COLOR, COMMAND_PROMPT_BACKGROUND_COLOR, false, false);
        }

        public static string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}