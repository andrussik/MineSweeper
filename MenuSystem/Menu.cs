using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuSystem
{
    public class Menu
    {
        public string Title { get; set; } = default!;

        public List<MenuItem> MenuItems { get; set; } 
            = new List<MenuItem>();

        public Action? Execute { get; set; }

        public void Run()
        {
            if (MenuItems.Any())
            {
                foreach (var menuItem in MenuItems)
                {
                    menuItem.IsSelected = false;
                }
                
                MenuItems[0].IsSelected = true;
                
                MenuUi();
            }
        }

        private void MenuUi()
        {
            Console.Clear();
            if (Execute != null) Execute();
            Console.WriteLine(Title);
            Console.WriteLine("========================");
            
            foreach (var menuItem in MenuItems)
            {
                if (menuItem.IsSelected) Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(menuItem);  
                Console.ResetColor();
            }

            Console.WriteLine("========================");
            Select();
        }

        private void Select()
        {
            var selectedIndex = MenuItems.FindIndex(x => x.IsSelected);
            var command = Console.ReadKey().Key;

            switch (command)
            {
                case ConsoleKey.UpArrow:
                    if (selectedIndex > 0)
                    {
                        MenuItems[selectedIndex].IsSelected = false;
                        MenuItems[selectedIndex - 1].IsSelected = true;
                    }

                    break;
                case ConsoleKey.DownArrow:
                    if (selectedIndex < MenuItems.Count - 1)
                    {
                        MenuItems[selectedIndex].IsSelected = false;
                        MenuItems[selectedIndex + 1].IsSelected = true;
                    }

                    break;
                case ConsoleKey.Spacebar:
                    MenuItems[selectedIndex].Execute();
                    return;
                case ConsoleKey.Enter:
                    MenuItems[selectedIndex].Execute();
                    return;
            }

            MenuUi();
        }
    }
}