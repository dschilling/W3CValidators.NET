// Copyright (c) 2011 Daniel A. Schilling

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("W3CValidators")]
[assembly: AssemblyDescription(".NET library for interacting with the W3C Validators (Markup, CSS, and Feed)")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ebf5e549-2ad7-4bf8-917d-6a4f90323dcc")]

[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Validators")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "namespace", Target = "W3CValidators.Markup", MessageId = "Validators")]