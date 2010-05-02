namespace FakeItEasy.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Access all types in all assemblies in the same directory as the FakeItEasy dll.
    /// </summary>
    public class ApplicationDirectoryAssembliesTypeAccessor
        : ITypeAccessor
    {
        private List<Type> availableTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDirectoryAssembliesTypeAccessor"/> class.
        /// </summary>
        public ApplicationDirectoryAssembliesTypeAccessor()
        {
            this.availableTypes = new List<Type>();

            this.InitializeAvailableTypes();
        }

        /// <summary>
        /// Gets a collection of available types.
        /// </summary>
        /// <returns>The available types.</returns>
        public IEnumerable<Type> GetAvailableTypes()
        {
            return this.availableTypes;
        }

        private void InitializeAvailableTypes()
        {
            var applicationDirectory = Path.GetDirectoryName(typeof(ApplicationDirectoryAssembliesTypeAccessor).Assembly.Location);

            foreach (var assemblyFile in Directory.GetFiles(applicationDirectory, "*.dll"))
            {
                this.LoadAllTypesFromAssemblyFile(assemblyFile);
            }
        }

        private void LoadAllTypesFromAssemblyFile(string assemblyFile)
        {
            try
            {
                var assembly = Assembly.LoadFile(assemblyFile);

                this.LoadAllTypesFromAssembly(assembly);
            }
            catch
            {
            }
        }

        private void LoadAllTypesFromAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                this.availableTypes.Add(type);
            }
        }
    }
}
