namespace FakeItEasy.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// A collection of method arguments.
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "Best name to describe the type.")]
    public class ArgumentCollection
        : IEnumerable<object>
    {
        /// <summary>
        /// The arguments this collection contains.
        /// </summary>
        private readonly object[] arguments;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentCollection"/> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="argumentNames">The argument names.</param>
        [DebuggerStepThrough]
        public ArgumentCollection(object[] arguments, IEnumerable<string> argumentNames)
        {
            Guard.AgainstNull(arguments, "arguments");
            Guard.AgainstNull(argumentNames, "argumentNames");

            if (arguments.Length != argumentNames.Count())
            {
                throw new ArgumentException(ExceptionMessages.WrongNumberOfArgumentNamesMessage, "argumentNames");
            }

            this.arguments = arguments;
            this.ArgumentNames = argumentNames;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentCollection"/> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="method">The method.</param>
        [DebuggerStepThrough]
        public ArgumentCollection(object[] arguments, MethodInfo method)
            : this(arguments, GetArgumentNames(method)) 
        {
        }
        
        /// <summary>
        /// Gets an empty ArgumentList.
        /// </summary>
        public static ArgumentCollection Empty
        {
            [DebuggerStepThrough]
            get
            {
                return new ArgumentCollection(new object[] { }, new string[] { });
            }
        }

        /// <summary>
        /// Gets the number of arguments in the list.
        /// </summary>
        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return this.arguments.Length;
            }
        }

        /// <summary>
        /// Gets the names of the arguments in the list.
        /// </summary>
        public IEnumerable<string> ArgumentNames { get; private set; }

        /// <summary>
        /// Gets the argument at the specified index.
        /// </summary>
        /// <param name="argumentIndex">The index of the argument to get.</param>
        /// <returns>The argument at the specified index.</returns>
        public object this[int argumentIndex]
        {
            [DebuggerStepThrough]
            get
            {
                return this.arguments[argumentIndex];
            }
        }

        /// <summary>
        /// Gets the argument at the specified index.
        /// </summary>
        /// <typeparam name="T">The type of the argument to get.</typeparam>
        /// <param name="index">The index of the argument.</param>
        /// <returns>The argument at the specified index.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Used to cast the argument to the specified type.")]
        public T Get<T>(int index)
        {
            return (T)this.arguments[index];
        }

        /// <summary>
        /// Gets the argument with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the argument to get.</typeparam>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>The argument with the specified name.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Used to cast the argument to the specified type.")]
        public T Get<T>(string argumentName)
        {
            var index = this.GetArgumentIndex(argumentName);

            return (T)this.arguments[index];
        }

        internal object[] GetUnderlyingArgumentsArray()
        {
            return this.arguments;
        }

        [DebuggerStepThrough]
        private static IEnumerable<string> GetArgumentNames(MethodInfo method)
        {
            Guard.AgainstNull(method, "method");

            return method.GetParameters().Select(x => x.Name);
        }

        private int GetArgumentIndex(string argumentName)
        {
            int index = 0;

            foreach (var name in this.ArgumentNames)
            {
                if (name.Equals(argumentName, StringComparison.Ordinal))
                {
                    return index;
                }

                index++;
            }

            throw new ArgumentException(ExceptionMessages.ArgumentNameDoesNotExist, "argumentName");
        }

        public IEnumerator<object> GetEnumerator()
        {
            return this.arguments.Cast<object>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}