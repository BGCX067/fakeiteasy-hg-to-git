﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FakeItEasy.Creation.CastleDynamicProxy {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class DynamicProxyResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DynamicProxyResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FakeItEasy.Creation.CastleDynamicProxy.DynamicProxyResources", typeof(DynamicProxyResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No constructor matches the passed arguments for constructor..
        /// </summary>
        internal static string ArgumentsForConstructorDoesNotMatchAnyConstructorMessage {
            get {
                return ResourceManager.GetString("ArgumentsForConstructorDoesNotMatchAnyConstructorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Arguments for constructor specified for interface type..
        /// </summary>
        internal static string ArgumentsForConstructorOnInterfaceTypeMessage {
            get {
                return ResourceManager.GetString("ArgumentsForConstructorOnInterfaceTypeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type of proxy &quot;{0}&quot; is sealed..
        /// </summary>
        internal static string ProxyIsSealedTypeMessage {
            get {
                return ResourceManager.GetString("ProxyIsSealedTypeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type of proxy must be an interface or a class but it was {0}..
        /// </summary>
        internal static string ProxyIsValueTypeMessage {
            get {
                return ResourceManager.GetString("ProxyIsValueTypeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No default constructor was found on the type {0}..
        /// </summary>
        internal static string ProxyTypeWithNoDefaultConstructorMessage {
            get {
                return ResourceManager.GetString("ProxyTypeWithNoDefaultConstructorMessage", resourceCulture);
            }
        }
    }
}
