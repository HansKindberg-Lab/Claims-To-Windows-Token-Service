using System.Reflection;

#pragma warning disable 436

[assembly: AssemblyFileVersion(SolutionVersionInfo.AssemblyVersion + ".0")]
[assembly: AssemblyInformationalVersion(SolutionVersionInfo.AssemblyVersion + "")]
[assembly: AssemblyVersion(SolutionVersionInfo.AssemblyVersion)]
#pragma warning restore 436
internal static class SolutionVersionInfo
{
	#region Fields

	internal const string AssemblyVersion = "1.0.0";

	#endregion
}