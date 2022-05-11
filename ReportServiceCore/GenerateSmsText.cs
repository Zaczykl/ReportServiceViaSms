using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportServiceCore
{
    public class GenerateSmsText
    {
        public string GenerateTextFromErrors(List<Error> errors, int intervalInMinutes)
        {            
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            if (!errors.Any())
                return string.Empty;

            string smsText = $"Lista błędów z ostatnich {intervalInMinutes} minut:";
            foreach (var error in errors)
            {
                smsText += $"\nData: {error.Date.ToString("dd-MM HH:mm")}\n{error.ErrorMessage}\n";
            }

            return smsText;
        }
    }
}
