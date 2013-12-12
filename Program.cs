using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace ImageEdit
{
    public class Program
    {
        // Default values - should be moved to settings file
        private const string DEFAULT_OUTPUT_FILENAME = "image_edit_output";
        private const bool CONTINUE_AFTER_EXECUTION_ERROR = false;
        private const char COMMAND_ARG_SEPARATOR = ' ';
        public const int DEFAULT_IMAGE_WIDTH = 800;
        public const int DEFAULT_IMAGE_HEIGHT = 600;

        // misc. shouldn't be moved to settins file
        private const string PNG_FILE_EXTENSION = ".png";
        private const string CANT_SAVE_FILE_ERROR_MESSAGE = "Can't save file {0}";
        
        // exit codes - shouldn't be in settings file
        private const int NO_ERROR_EXIT_CODE = 0x0;
        private const int INVALID_ARGS_EXIT_CODE = 0x1;
        private const int COMMAND_ARGS_EXCEPTION_EXIT_CODE = 0x2;
        private const int EXECUTION_EXCEPTION_EXIT_CODE = 0x4;

        //actual vars
        public static string OutputFileName;
        private static int imageWidth, imageHeight;

        public static int Main(string[] args)
        {
            try
            {
                ParseArgs(args);
            }
            catch (NonIntProgramArgExceptoin e)
            {
                Output.DisplayErrorMessage(e.GetErrorMessage());
                return INVALID_ARGS_EXIT_CODE;
            }

            Output.DisplayInitialPrompt(OutputFileName);

            Output.DisplayColoredMessage("Loading Commands...", Output.INITIAL_PROMPT_BACKGROUND_COLOR, Output.STANDARD_FOREGROUND_COLOR_DARK, false, false);
            Command[] availableCommands = GetCommands();
            Output.DisplayColoredMessage("done.", Output.INITIAL_PROMPT_BACKGROUND_COLOR, Output.STANDARD_FOREGROUND_COLOR_DARK, true, false);

            return MainLoop(availableCommands, new Bitmap(imageWidth, imageHeight));
        }

        public static void ParseArgs(string[] args)
        {
            ImageEditArgs pArgs = new ImageEditArgs(args);

            if (pArgs.HasOutputFileName())
            {
                OutputFileName = pArgs.GetOutputFilename();
            }

            if (pArgs.HasImageWidth())
            {
                if (!pArgs.TryGetWidth(out imageWidth))
                {
                    throw new NonIntProgramArgExceptoin(ImageEditArgs.WIDTH_LABEL);
                }
            }
            else
            {
                imageWidth = DEFAULT_IMAGE_WIDTH;
            }

            if (pArgs.HasImageHeight())
            {
                if (!pArgs.TryGetHeight(out imageHeight))
                {
                    throw new NonIntProgramArgExceptoin(ImageEditArgs.HEIGHT_LABEL);
                }
            }
            else
            {
                imageHeight = DEFAULT_IMAGE_HEIGHT;
            }
        }

        public static int MainLoop(Command[] availableCommands, Bitmap bmp)
        {
            Graphics g = Graphics.FromImage(bmp);

            bool loop = true;
            while (loop)
            {
                Output.DisplayCommandPrompt();
                string commandStr = Output.ReadLine();
                if (!string.IsNullOrEmpty(commandStr))
                {
                    string[] commandArgs = commandStr.Split(COMMAND_ARG_SEPARATOR);

                    // Get the appropriate command
                    Command command;
                    try
                    {
                        command = GetAppropriateCommand(availableCommands, commandArgs);
                        command.ParseArgs(commandArgs);
                    }
                    catch (CommandArgException e)
                    {
                        Output.DisplayErrorMessage(e.GetErrorMessage());
                        continue;
                    }
                    
                    // execute the command
                    try
                    {
                        ExecutableReturnValues returnVals = null;

                        if (command is IExecutable)
                        {
                            returnVals = ((IExecutable)command).Execute();
                        }

                        if (command is IGraphicsExecutable)
                        {
                            returnVals = ((IGraphicsExecutable)command).Execute(g);
                        }

                        if (returnVals != null)
                        {
                            if (returnVals.Save)
                            {
                                Save(bmp);
                            }

                            if (returnVals.Exit)
                            {
                                return NO_ERROR_EXIT_CODE;
                            }
                        }
                    }
                    catch (ExitProgramException e)
                    {
                        return e.ExitCode;
                    }
                    catch (ExecutionException e)
                    {
                        Output.DisplayErrorMessage(e.GetErrorMessage());

                        if (CONTINUE_AFTER_EXECUTION_ERROR)
                        {
                            continue;
                        }
                        else
                        {
                            return EXECUTION_EXCEPTION_EXIT_CODE;
                        }
                    }
                }
            }

            return NO_ERROR_EXIT_CODE;
        }

        public static Command GetAppropriateCommand(Command[] availableCommands, string[] commandArgs)
        {
            List<Command> matchingCommands = new List<Command>();

            foreach (Command command in availableCommands)
            {
                if (command.ShouldExecute(commandArgs))
                {
                    matchingCommands.Add(command);
                }
            }

            if (matchingCommands.Count == 0)
            {
                throw new CommandNotFoundExeption(commandArgs[0]);
            }
            else if (matchingCommands.Count > 1)
            {
                throw new NotImplementedException(string.Format("Two or more commands were with found associated with '{0}'. Multiple command support not yet implemented.", commandArgs[0]));
            }
            else
            {
                return matchingCommands[0];
            }
        }

        //TODO: this could use a refactor to handle different file types and existing files
        private static bool Save(Bitmap bmp)
        {
            try
            {
                bmp.Save(OutputFileName + PNG_FILE_EXTENSION, ImageFormat.Png);
            }
            catch
            {
                Output.DisplayErrorMessage(string.Format(CANT_SAVE_FILE_ERROR_MESSAGE, OutputFileName));
                return false;
            }

            return true;
        }

        public static Command[] GetCommands()
        {
            Command[] commands = new Command[3];

            //---------------------------------
            // Hard Coded command instantiating
            //
            // should maybe be moved
            //---------------------------------
            commands[0] = new ExitCommand();
            commands[1] = new SaveCommand();

            commands[2] = new FadeLine1();

            return commands;
        }
    }
}
