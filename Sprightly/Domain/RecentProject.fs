namespace Sprightly.Domain

open Sprightly.Common

/// <summary>
/// The <see cref="RecentProject"/> describes a recent project.
/// </summary>
type public RecentProject = 
    { Path : Path.T
      LastOpened : System.DateTime
    }

