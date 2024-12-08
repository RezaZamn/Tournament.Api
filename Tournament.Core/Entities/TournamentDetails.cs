using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Entities
{
    public class TournamentDetails
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "The title is required.")]
        [StringLength(50, ErrorMessage = "The title should not be more than 50 characters.")]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
