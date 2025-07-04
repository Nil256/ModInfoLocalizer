using Microsoft.Xna.Framework;
using System;

namespace ModInfoLocalizer
{
    internal static class ModCall
    {
        internal static object Call(object[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            if (args.Length == 0)
            {
                throw new ArgumentException("Arguments cannot be empty.");
            }
            if (args[0] is not string methodName)
            {
                throw new ArgumentException("1st argument should be string, this is method name.");
            }
            switch (methodName)
            {
                case "Register":
                    RegisterLocalizedInfo(methodName, args);
                    return null;
                default:
                    throw new ArgumentException($"Unknown method name \"{methodName}\".");
            }
        }

        private static void RegisterLocalizedInfo(in string methodName, object[] args)
        {
            string howToUse = $"{methodName}(string modName, string languageCode, string localizedDescription, int priority = -1)";

            ThrowIfArgumentsCountIsLowerThanRequired(args, 3, methodName, howToUse);

            GetConvertedArgumentObject(args, 1, out string modName, "modName", howToUse);
            ThrowIfStringIsNullOrWhiteSpace(modName, "modName", howToUse);

            GetConvertedArgumentObject(args, 2, out string languageCode, "languageCode", howToUse);
            ThrowIfStringIsNullOrEmpty(languageCode, "languageCode", howToUse);

            GetConvertedArgumentObject(args, 3, out string localizedDescription, "localizedDescription", howToUse);
            ThrowIfStringIsNull(localizedDescription, "localizedDescription", howToUse);

            int priority = -1;
            if (args.Length >= 3)
            {
                GetConvertedArgumentObject(args, 4, out int argPriority, "priority", howToUse);
                priority = argPriority;
                if (priority == 0)
                {
                    throw new ArgumentOutOfRangeException("priority", $"priority cannot be assigned 0. {howToUse}\nIf you are {modName}'s author, please consider to make file \"description_<Language Code>.txt\".");
                }
            }

            LocalizedInfoRegistry.RegisterFromModCall(modName, languageCode, localizedDescription, priority);
        }

        private static void ThrowIfArgumentsCountIsLowerThanRequired(object[] args, in int requiredCount, in string methodName, in string howToUse)
        {
            if (args.Length < requiredCount + 1)
            {
                throw new ArgumentException($"{methodName} requires {requiredCount} {(requiredCount > 1 ? "arguments" : "argument")} at least. {howToUse}");
            }
        }

        private static void GetConvertedArgumentObject<T>(object[] args, in int argCount, out T convertedArg, in string argumentName, in string howToUse)
        {
            if (args[argCount] is not T converted)
            {
                throw new ArgumentException($"{argumentName} should be {typeof(T)}, but {args[argCount].GetType()}. {howToUse}");
            }
            convertedArg = converted;
        }

        private static void ThrowIfStringIsNull(string arg, in string argumentName, in string howToUse)
        {
            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentException($"{argumentName} cannot be null. {howToUse}");
            }
        }
        private static void ThrowIfStringIsNullOrEmpty(string arg, in string argumentName, in string howToUse)
        {
            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentException($"{argumentName} cannot be null or empty. {howToUse}");
            }
        }
        private static void ThrowIfStringIsNullOrWhiteSpace(string arg, in string argumentName, in string howToUse)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                throw new ArgumentException($"{argumentName} cannot be null, empty, or whitespace. {howToUse}");
            }
        }
    }
}
