﻿using System.Collections.Generic;
using DDDEastAnglia.Models;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData
{
    public class SponsorRepository : ISponsorRepository
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public IEnumerable<Sponsor> GetAllSponsors()
        {
            return db.Sponsors.All();
        }
    }
}
