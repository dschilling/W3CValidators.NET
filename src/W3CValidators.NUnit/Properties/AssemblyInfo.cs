// Copyright (c) 2009 Daniel A. Schilling

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("W3CValidators.NUnit")]
[assembly: AssemblyDescription(".NET library for interacting with the W3C Validators (Markup, CSS, and Feed) - NUnit Integration")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Daniel A. Schilling")]
[assembly: AssemblyProduct("W3CValidators")]
[assembly: AssemblyCopyright("Copyright © 2009 Daniel A. Schilling")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("e7b52bf5-635b-429a-8485-e2dc2d05622a")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.2.0.0")]
[assembly: AssemblyFileVersion("0.2.0.0")]

[assembly: CLSCompliant(true)]

[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Validators")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "namespace", Target = "W3CValidators.NUnit", MessageId = "Validators")]