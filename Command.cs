using System;
using System.Drawing;
using System.Xml.Linq;

namespace ImageEdit
{
    public abstract class Command
    {
        public readonly string CommandArg;
        public readonly string Description;
        public readonly string Usage;

        public Command(string commandArg, string description, string usage)
        {
            CommandArg = commandArg;
            Description = description;
            Usage = usage;
        }

        public virtual bool ShouldExecute(string[] args)
        {
            return args[0] == CommandArg;
        }

        public abstract void ParseArgs(string[] args);
    }

    public interface IExecutable
    {
        ExecutableReturnValues Execute();
    }

    public interface IGraphicsExecutable
    {
        ExecutableReturnValues Execute(Graphics g);
    }

    public class ExecutableReturnValues
    {
        public readonly bool Exit, Save;

        public ExecutableReturnValues(bool exit, bool save)
        {
            Exit = exit;
            Save = save;
        }

        public ExecutableReturnValues()
        {
            Exit = false;
            Save = false;
        }
    }

    /*
     * Built in commands
     */
    public class ExitCommand : Command, IExecutable
    {
        public const string EXIT_COMMAND_ARG = "exit";
        public const string EXIT_DESCRIPTION = "This command will exit the ImageEdit program";

        public ExitCommand() : base(EXIT_COMMAND_ARG, EXIT_DESCRIPTION, EXIT_COMMAND_ARG) { }

        public override void ParseArgs(string[] args)
        {
            if (args.Length > 1)
            {
                throw new TooManyCommandArgsException(args[0], 1, args.Length);
            }
        }

        public virtual ExecutableReturnValues Execute()
        {
            return new ExecutableReturnValues(true, true);
        }
    }

    public class SaveCommand : Command, IExecutable
    {
        public const string SAVE_COMMAND_ARG = "save";
        public const string SAVE_DESCRIPTION = "This command will save the current image in memory.";

        public SaveCommand() : base(SAVE_COMMAND_ARG, SAVE_DESCRIPTION, SAVE_COMMAND_ARG) { }

        public override void ParseArgs(string[] args)
        {
            if (args.Length > 1)
            {
                throw new TooManyCommandArgsException(args[0], 1, args.Length);
            }
        }

        public virtual ExecutableReturnValues Execute()
        {
            return new ExecutableReturnValues(false, true);
        }
    }
}