#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion


namespace Virulent
{
  static class Program
  {
    private static VirulentGame game;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
      {
        game = new VirulentGame ();
        game.Run();
      }
  }
}
