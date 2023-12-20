using System.Diagnostics.Metrics;
using bojpawnapi.Common.OpenTelemetry;

namespace bojpawnapi.Service.Metric;

public class PawnMetrics
{
    private  Counter<int> CollateralPawnCounter { get; }
    private  Counter<int> CollateralRollOverCounter { get; }
    private  Counter<int> CollateralRedeemCounter { get; }
    private  UpDownCounter<int> TotalCollateralUpDownCounter { get; }

    private Histogram<double> CollateralContractsAmtHistogram { get; }
    private Histogram<int> NumberOfCollateralPerContractsHistogram { get; }
    private  UpDownCounter<int> CurrentCustomerCounter { get; }

    public PawnMetrics(IMeterFactory meterFactory, IConfiguration configuration)
    {
        ObservabilityOptions observabilityOptions = new();

        configuration
            .GetRequiredSection(nameof(ObservabilityOptions))
            .Bind(observabilityOptions);
        var meter = meterFactory.Create(observabilityOptions.ServiceName);

        //Collateral
        CollateralPawnCounter = meter.CreateCounter<int>("Collateral-Pawn", "Collateral");
        CollateralRollOverCounter = meter.CreateCounter<int>("Collateral-RollOver", "Collateral");
        CollateralRedeemCounter = meter.CreateCounter<int>("Collateral-Redeem", "Collateral");

        TotalCollateralUpDownCounter = meter.CreateUpDownCounter<int>("Total-Collateral", "Collateral");

        CollateralContractsAmtHistogram = meter.CreateHistogram<double>("Contract-Amt", "THB", "Collateral-Amt distribution of PAWN");
        NumberOfCollateralPerContractsHistogram = meter.CreateHistogram<int>("Contract-number-of-Collateral", "Items", "Number of Collateral per Contract");
        
        //Customer
        CurrentCustomerCounter = meter.CreateUpDownCounter<int>("Customer-Current-Count", "Customer");
    }

    //Collateral Metric
    public void CollateralPawn() => CollateralPawnCounter.Add(1);
    public void CollateralRollOver() => CollateralRollOverCounter.Add(1);
    public void CollateralRedeem() => CollateralRedeemCounter.Add(1);

    public void IncreaseCollateralContracts() => TotalCollateralUpDownCounter.Add(1);
    public void DecreaseCollateralContracts() => TotalCollateralUpDownCounter.Add(-1);

    //Orders Metric
    public void RecordContractAmt(double amount) => CollateralContractsAmtHistogram.Record(amount);
    public void RecordNumberOfCollateral(int count) => NumberOfCollateralPerContractsHistogram.Record(count);

    //Customer Metric
    public void IncreaseCustomer() => CurrentCustomerCounter.Add(1);
    public void DecreaseCustomer() => CurrentCustomerCounter.Add(-1);

}