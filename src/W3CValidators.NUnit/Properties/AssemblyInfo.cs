// Copyright (c) 2011 Daniel A. Schilling

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("W3CValidators.NUnit")]
[assembly: AssemblyDescription(".NET library for interacting with the W3C Validators (Markup, CSS, and Feed) - NUnit Integration")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("e7b52bf5-635b-429a-8485-e2dc2d05622a")]

[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Validators")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "namespace", Target = "W3CValidators.NUnit", MessageId = "Validators")]