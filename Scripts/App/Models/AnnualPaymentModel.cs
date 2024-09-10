using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnualPaymentModel : SqliteModel
{
    protected override string Table { get => "annual_payment"; }
}
