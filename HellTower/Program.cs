using HellTower.Controller;
using HellTower.View;
using System;
using System.Windows.Forms;

namespace HellTower
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var controller = new GameController();
            Application.Run(new GameForm(controller));
        }
    }
}