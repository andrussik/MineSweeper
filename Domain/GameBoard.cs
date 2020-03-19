using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameBoard
    {
        public int GameBoardId { get; set; }
        [MaxLength(255)]
        public string BoardName { get; set; } = default!;
        public string JsonString { get; set; } = default!;

        public override string ToString()
        {
            return $"ID: {GameBoardId}\n" +
                   $"Board Name: {BoardName}\n";
        }
    }
}