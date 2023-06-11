using System;
using System.Collections.Generic;

namespace Main_Branch_Locator_App.Models;

public partial class ReportTable
{
    public long ReportId { get; set; }

    public string ReportSubject { get; set; } = null!;

    public string ReportDescription { get; set; } = null!;

    public int FkBranchCode { get; set; }

    public DateTime ReportDateTime { get; set; }

    public string AdminRead { get; set; } = null!;

    public double Rating { get; set; }
}
