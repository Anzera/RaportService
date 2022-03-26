using RaportService.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RaportService.Core
{
    public class GenerateHtmlEmail
    {
        public string GenerateErrors(List<Error> errors, int interval)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            if (!errors.Any())
                return string.Empty;

            var html = $"Błędy z ostatnich {interval} minut.<br/><br/>";

            html += @"
                        <table border=1 cellpadding=5 cellspacing=1>
                            <tr>
                                <td algin=center bgcolor=lightgrey>Wiadomość</td>
                                <td algin=center bgcolor=lightgrey>Data</td>
                            </tr>
                     ";

            foreach(var error in errors)
            {
                html +=
                    $@"<tr>
                        <td algin=center>{error.Message}</td>
                        <td algin=center>{error.Date.ToString("dd-MM-yyyy HH:mm")}</td>
                    </td>";
            }
            html += @"</table><br/><br/><i>Automatyczna wiadomość wysłana z aplikacji ReportService</i>";
            return html;
        }

        public string GenerateRaport(Raport raport)
        {
            if (raport == null)
                throw new ArgumentNullException(nameof(raport));

            var html = $"Raport {raport.Title} z dnia {raport.Date.ToString("dd-MM-yyyy")}.<br/><br/>";

            if (raport.Positions != null && raport.Positions.Any())
            {
                html += @"
                        <table border=1 cellpadding=5 cellspacing=1>
                            <tr>
                                <td algin=center bgcolor=lightgrey>Wiadomość</td>
                                <td algin=center bgcolor=lightgrey>Opis</td>
                                <td algin=center bgcolor=lightgrey>Wartość</td>
                            </tr>";

                foreach (var position in raport.Positions)
                {
                    html +=
                        $@"<tr>
                        <td algin=center>{position.Title}</td>
                        <td algin=center>{position.Description}</td>
                        <td algin=center>{position.Value.ToString("0.00")} zł</td>
                    </td>";
                }

                html += "</table>";
            }
            else
                html += "--Brak danych do wyświetlenia--";

            html += @"<br/><br/><i>Automatyczna wiadomość wysłana z aplikacji ReportService</i>";
            return html;
        }
    }
}
