﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Proposal
    {
        [Key]
        public int Id { get; set; }
        public string AddtionalDetails { get; set; }
        public int SessionRequestId { get; set; }
        public string PhotographerId { get; set; }
        public ApplicationUser Photographer { get; set; } // Navigational property
        public string ClientId { get; set; }
        public ApplicationUser Client { get; set; } // Navigational property
        public string ProposalText { get; set; }
        public bool IsAccepted { get; set; }
        public SessionRequest SessionRequest { get; set; } // Navigational property
    }
}
