using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
   public class Reaction
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public int UserId { get; set; }
        public ReactionType ReactionType { get; set; }
    }
}
