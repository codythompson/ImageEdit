using System;

namespace ImageEdit
{
    public class MainLoopException : Exception
    {
        public const string ERROR_MESSAGE_PREFIX = "Error:\n";

        public MainLoopException(string errorMessage) : base(errorMessage) { }

        public virtual string GetErrorMessage()
        {
            return ERROR_MESSAGE_PREFIX + Message;
        }
    }

    public class NonIntProgramArgExceptoin : MainLoopException
    {
        public const string NON_INT_ERROR_MESSAGE_FORMAT_STRING = "Unable to parse value for {0} as integer.";

        public NonIntProgramArgExceptoin(string argLabel) : base(string.Format(NON_INT_ERROR_MESSAGE_FORMAT_STRING, argLabel)) { }
    }

    public class CommandArgException : MainLoopException
    {
        public CommandArgException(string message) : base (message) { }
    }

    public class InvalidCommandArgException : CommandArgException
    {
        public const string TYPE_ERROR_MESSAGE_FORMAT_STRING = "Invalid argument for command {0}, expected arg of type {1} at index {2}, received '{3}'";

        public InvalidCommandArgException(string commandArg, string expectedTypeName, int argIndex, string arg) :
            base(string.Format(TYPE_ERROR_MESSAGE_FORMAT_STRING, commandArg, expectedTypeName, argIndex, arg)) { }
    }

    public class MissingCommandArgException : CommandArgException
    {
        public const string MISSING_ARG_ERROR_MESSAGE_FORMAT_STRING = "Expecting an arg at index {0} for command {1}, only {2} arg(s) given.";

        public MissingCommandArgException(string commandArg, int argIndex, int totalArgs) :
            base(string.Format(MISSING_ARG_ERROR_MESSAGE_FORMAT_STRING, argIndex, commandArg, totalArgs)) { }
    }

    public class TooManyCommandArgsException : CommandArgException
    {
        public const string TOO_MANY_ARGS_ERROR_MESSAGE_FORMAT_STRING = "Expecting at most {0} arg(s) for command {1}, given {2}";

        public TooManyCommandArgsException(string commandArg, int argsExpected, int argsGiven) :
            base(string.Format(TOO_MANY_ARGS_ERROR_MESSAGE_FORMAT_STRING, argsExpected, commandArg, argsGiven)) { }
    }

    public class CommandNotFoundExeption : CommandArgException
    {
        public const string COMMAND_NOT_FOUND_ERROR_MESSAGE_FORMAT_STRING = "Command '{0}' not found.";

        public CommandNotFoundExeption(string commandGiven) :
            base(string.Format(COMMAND_NOT_FOUND_ERROR_MESSAGE_FORMAT_STRING, commandGiven)) { }
    }


    public class ExecutionException : MainLoopException
    {
        public ExecutionException(string message) : base(message) { }
    }

    public class ExitProgramException : Exception
    {
        public readonly int ExitCode;

        public ExitProgramException(int exitCode)
        {
            ExitCode = exitCode;
        }

        public ExitProgramException()
        {
            ExitCode = 0;
        }
    }
}