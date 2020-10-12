namespace Sprightly.DataAccess
    /// <summary>
    /// <see cref="RecentProject"/> defines a single recent project with a 
    /// path and a date when it was last opened.
    /// </summary>
    type public RecentProject = 
        { Path : Sprightly.Domain.Path.T 
          LastOpened : System.DateTime
        }

