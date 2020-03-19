using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Game : PageModel
    {
        private readonly AppDbContext _context;
        public Domain.GameBoard? GameBoard { get; set; }
        [BindProperty] public string? Mode { get; set; }
        [BindProperty] public int? Height { get; set; }
        [BindProperty] public int? Width { get; set; }
        [BindProperty] public int? Mines { get; set; }
        public List<Domain.GameBoard> GameBoards { get; set; } = new List<Domain.GameBoard>();

        public Game(AppDbContext context)
        {
            _context = context;
        }
        
        public void OnGet()
        {
        }

        public ActionResult OnPostNewGame()
        {
            if (Mines >= Height * Width)
            {
                Mines = Height * Width - 1;
            }

            return Page();
        }

        public async Task<ActionResult> OnPostLoad(string boardId, string load)
        {
            if (!string.IsNullOrEmpty(load) && !string.IsNullOrEmpty(boardId))
            {
                GameBoard = await _context.GameBoards.FindAsync(int.Parse(boardId));

                Console.WriteLine($"Loaded game {GameBoard.BoardName}");
                
                return Page();
            }
            
            return NotFound();
        }

        public async Task<ActionResult> OnPostDelete([FromBody]string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var gameBoard = await _context.GameBoards.FindAsync(int.Parse(id));
                _context.GameBoards.Remove(gameBoard);
                _context.SaveChanges();
                
                Console.WriteLine($"Deleted game {gameBoard.BoardName}");
                
                return new JsonResult(id);
            }

            return NotFound();
        }
        
        public async Task<ActionResult> OnPostSave([FromBody]Domain.GameBoard gameBoard)
        {
            if (gameBoard != null)
            {
                GameBoard = gameBoard;
                await _context.GameBoards.AddAsync(gameBoard);
                await _context.SaveChangesAsync();
                
                Console.WriteLine($"Saved game {gameBoard.BoardName}");
                
                return new JsonResult(GameBoard.GameBoardId);
            }

            return NotFound();
        }

        public async Task<ActionResult> OnGetGame(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                GameBoard = await _context.GameBoards.FindAsync(int.Parse(id));

                return Page();
            }
            
            return NotFound();
        }

        public void LoadGameBoards()
        {
            if (_context.GameBoards.Any())
            {
                foreach (var board in _context.GameBoards)
                {
                    GameBoards.Add(board);
                }
            }
        }
    }
}