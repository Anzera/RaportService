using RaportService.Core.Domains;
using System;
using System.Collections.Generic;

namespace RaportService.Core.Repositories
{
    public class RaportRepository
    {
        public Raport GetLastNotSendRaport()
        {
            //pobieranie z bazy danych

            return new Raport
            {
                Id = 1,
                Title = "R/1/2022",
                Date = new DateTime(2022, 1, 1, 12, 0, 0),
                Positions = new List<RaportPosition>
                {
                    new RaportPosition {Id = 1, RaportId = 1, Title = "Position 1", Description = "Description 1", Value = 44.99m},
                    new RaportPosition {Id = 2, RaportId = 1, Title = "Position 2", Description = "Description 2", Value = 23.99m},
                    new RaportPosition {Id = 3, RaportId = 1, Title = "Position 3", Description = "Description 3", Value = 98.99m}
                }
            };
        }

        public void RaportSend(Raport raport)
        {
            raport.IsSend = true;
            //zapis w bazie
        }
    }
}
