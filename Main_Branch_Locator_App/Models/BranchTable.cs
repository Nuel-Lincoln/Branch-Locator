using System;
using System.Collections.Generic;

namespace Main_Branch_Locator_App.Models;

public partial class BranchTable
{
    public string BranchName { get; set; } = null!;

    public int BranchCode { get; set; }

    public string BranchPassword { get; set; } = null!;

    public double BranchGpsXCoordinate { get; set; }

    public double BranchGpsYCoordinate { get; set; }

    public string BranchManagerName { get; set; } = null!;

    public long BranchManagerTel { get; set; }

    public string BranchManagerEmail { get; set; } = null!;

    public string BranchAtmOperatorName { get; set; } = null!;

    public long BranchAtmOperatorTel { get; set; }

    public string BranchAtmOperatorEmail { get; set; } = null!;

    public string FxTrnx { get; set; } = null!;

    public string FxTransfer { get; set; } = null!;

    public string AtmCollection { get; set; } = null!;

    public string AtmCashWithdrawal { get; set; } = null!;

    public string CashDeposit { get; set; } = null!;

    public string TokenCollection { get; set; } = null!;

    public string PayDirect { get; set; } = null!;

    public string RemittaTrnx { get; set; } = null!;

    public string FundsTransfer { get; set; } = null!;

    public string? BranchCity { get; set; }

    public string? BranchAddress { get; set; }

    public string? AccountOpening { get; set; }

    public string? FormAtrnx { get; set; }
}
