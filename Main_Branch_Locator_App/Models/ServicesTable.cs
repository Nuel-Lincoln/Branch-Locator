using System;
using System.Collections.Generic;

namespace Main_Branch_Locator_App.Models;

public partial class ServicesTable
{
    public long ServiceId { get; set; }

    public string ServiceSubject { get; set; } = null!;

    public string ServiceDescription { get; set; } = null!;

    public string ServiceRequiredDocuments { get; set; } = null!;

    public string AvailableOnline { get; set; } = null!;

    public string LinkToService { get; set; } = null!;
}
