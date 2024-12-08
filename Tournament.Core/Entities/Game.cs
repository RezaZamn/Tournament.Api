using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Entities
{
    public class Game
    {   
        public int Id { get; set; }
        public string Title { get; set; }

        [Required(ErrorMessage ="The title is required.")]
        [StringLength(50, ErrorMessage = "The title should not be more than 50 characters.")]
        public DateTime Time { get; set; }

        //ForeginKey and navigation property
        public int? TournamentId { get; set; } 
        public TournamentDetails TournamentDetails { get; set; }
    }
}
